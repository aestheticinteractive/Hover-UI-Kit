using System.Collections.Generic;
using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class TreeUpdater : MonoBehaviour {

		//NOTE: use this with renamed ITreeUpdateable "TreeUpdate()" => "Update()"
		//private const bool IsProfilingMode = false;

		public bool DidTreeUpdateThisFrame { get; private set; }
		public TreeUpdater TreeParentThisFrame { get; private set; }
		public int TreeDepthLevelThisFrame { get; private set; }
		public List<ITreeUpdateable> TreeUpdatablesThisFrame { get; private set; }
		public List<TreeUpdater> TreeChildrenThisFrame { get; private set; }

		private bool vIsDestroyed;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TreeUpdater() {
			TreeUpdatablesThisFrame = new List<ITreeUpdateable>();
			TreeChildrenThisFrame = new List<TreeUpdater>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			/*if ( IsProfilingMode && Application.isPlaying ) {
				return;
			}*/

			if ( DidTreeUpdateThisFrame ) {
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

			Transform parTx = transform.parent;
			TreeUpdater parTreeUp = (parTx == null ? null : parTx.GetComponent<TreeUpdater>());
			
			if ( parTreeUp == null || !parTreeUp.isActiveAndEnabled ) {
				BeginAtThisTreeLevel();
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

			gameObject.GetComponents(TreeUpdatablesThisFrame);
			FindTreeChildren();

			for ( int i = 0 ; i < TreeUpdatablesThisFrame.Count ; i++ ) {
				ITreeUpdateable treeUp = TreeUpdatablesThisFrame[i];

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
			TreeChildrenThisFrame.Clear();

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

			int childDepth = pDepth+1;
			
			for ( int i = 0 ; i < TreeChildrenThisFrame.Count ; i++ ) {
				TreeUpdater childTreeUp = TreeChildrenThisFrame[i];
				childTreeUp.SendTreeUpdates(childDepth);
				childTreeUp.DescendTree(childDepth);
			}
		}

	}

}
