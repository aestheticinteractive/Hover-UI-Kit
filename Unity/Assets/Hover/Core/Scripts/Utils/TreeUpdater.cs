using System.Collections.Generic;
using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class TreeUpdater : MonoBehaviour {

		//NOTE: use this with renamed ITreeUpdateable "TreeUpdate()" => "Update()"
		//private const bool IsProfilingMode = false;

		//TODO:BUG: move tree child out of parent's hierarchy, the child's "TreeParentThisFrame" 
		//... does not get reset until/unless disable-then-reabling the former parent

		public bool DidTreeUpdateThisFrame { get; private set; }
		public TreeUpdater TreeParentThisFrame { get; private set; }
		public int TreeDepthLevelThisFrame { get; private set; }
		public List<ITreeUpdateable> TreeUpdatablesThisFrame { get; private set; }
		public List<TreeUpdater> TreeChildrenThisFrame { get; private set; }
		public bool ReloadTreeChildrenOnUpdate { get; set; }

		public bool UseLateUpdate = false;

		private bool vIsDestroyed;
		private bool vShouldBeginOnLateUpdate;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TreeUpdater() {
			TreeUpdatablesThisFrame = new List<ITreeUpdateable>();
			TreeChildrenThisFrame = new List<TreeUpdater>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			FindTreeChildren();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			/*if ( IsProfilingMode && Application.isPlaying ) {
				return;
			}*/

			if ( ReloadTreeChildrenOnUpdate || !Application.isPlaying ) {
				ReloadTreeChildrenOnUpdate = !Application.isPlaying;
				FindTreeChildren();
			}

			if ( DidTreeUpdateThisFrame ) {
				return;
			}
			
			if ( Application.isPlaying && TreeParentThisFrame != null ) { //experimental
				return;
			}

			AscendTreeOrBegin(true);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAtAndBelowThisLevel() {
			BeginAtThisTreeLevel();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void LateUpdate() {
			if ( vShouldBeginOnLateUpdate ) {
				BeginAtThisTreeLevel();
				vShouldBeginOnLateUpdate = true;
			}

			DidTreeUpdateThisFrame = false;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnDestroy() {
			vIsDestroyed = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ImmediateReloadTreeChildren() {
			//int before = TreeChildrenThisFrame.Count;
			FindTreeChildren();
			//int after = TreeChildrenThisFrame.Count;
			//Debug.Log("ImmediateReloadTreeChildren: "+name+" / "+before+" => "+after, gameObject);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AscendTreeOrBegin(bool pFromUpdate) {
			//if ( pFromUpdate ) { Debug.Log("AscendTreeOrBegin: "+gameObject.name, gameObject); }

			TreeUpdater parTreeUp = TreeParentThisFrame;
			
			if ( parTreeUp == null || !parTreeUp.isActiveAndEnabled ) {
				if ( !UseLateUpdate || !Application.isPlaying ) {
					BeginAtThisTreeLevel();
				}
				else {
					vShouldBeginOnLateUpdate = true;
				}
				return;
			}
			
			parTreeUp.AscendTreeOrBegin(false);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void BeginAtThisTreeLevel() {
			//Debug.Log("BeginAtThisTreeLevel: "+gameObject.name, gameObject);

			if ( vIsDestroyed ) {
				return;
			}

			SendTreeUpdates(0);
			DescendTree(0);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void SendTreeUpdates(int pDepth) {
			//Debug.Log(new string('-', pDepth)+"SendTreeUpdates: "+gameObject.name, gameObject);

			if ( vIsDestroyed ) {
				return;
			}

			/*if ( Application.isPlaying ) { //comment out TreeUpdate() call below for profiling
				SendMessage("TreeUpdate", SendMessageOptions.DontRequireReceiver); //for profiling
			}*/

			for ( int i = 0 ; i < TreeUpdatablesThisFrame.Count ; i++ ) {
				ITreeUpdateable treeUp = TreeUpdatablesThisFrame[i];

				if ( treeUp == null ) {
					if ( !ReloadTreeChildrenOnUpdate ) {
						ReloadTreeChildrenOnUpdate = true;
						Debug.Log("Lost tree sibling, will refresh list next frame: "+i, this);
					}
					continue;
				}

				if ( !treeUp.isActiveAndEnabled ) {
					continue;
				}

				treeUp.TreeUpdate();
			}

			DidTreeUpdateThisFrame = true;
			TreeDepthLevelThisFrame = pDepth;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void FindTreeChildren() {
			if ( TreeChildrenThisFrame.Count > 0 ) {
				TreeChildrenThisFrame.Clear();
			}

			if ( transform.parent != null && !transform.parent.gameObject.activeInHierarchy ) {
				return; //experimental
			}

			gameObject.GetComponents(TreeUpdatablesThisFrame);

			int childCount = transform.childCount;
			
			for ( int i = 0 ; i < childCount ; i++ ) {
				Transform childTx = transform.GetChild(i);
				TreeUpdater childTreeUp = childTx.GetComponent<TreeUpdater>();
				
				if ( childTreeUp == null ) {
					continue;
				}
				
				childTreeUp.TreeParentThisFrame = this;
				TreeChildrenThisFrame.Add(childTreeUp);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void DescendTree(int pDepth) {
			//Debug.Log(new string('-', pDepth)+"DescendTree: "+gameObject.name, gameObject);

			if ( vIsDestroyed ) {
				return;
			}

			int childCount = TreeChildrenThisFrame.Count;
			int childDepth = pDepth+1;

			for ( int i = 0 ; i < childCount ; i++ ) {
				TreeUpdater childTreeUp = TreeChildrenThisFrame[i];

				if ( childTreeUp == null ) {
					if ( !ReloadTreeChildrenOnUpdate ) {
						ReloadTreeChildrenOnUpdate = true;
						Debug.Log("Lost tree child, will refresh list next frame: "+i, this);
					}
					continue;
				}

				if ( !childTreeUp.isActiveAndEnabled ) {
					continue;
				}

				childTreeUp.SendTreeUpdates(childDepth);
				childTreeUp.DescendTree(childDepth);
			}
		}

	}

}
