using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class TreeUpdater : MonoBehaviour {

		//TODO:BUG: move tree child out of parent's hierarchy, the child's "TreeParentThisFrame" 
		//... does not get reset until/unless disable-then-reabling the former parent

		public TreeUpdater TreeParentThisFrame { get; private set; }
		public List<ITreeUpdateable> TreeUpdatablesThisFrame { get; private set; }
		public List<TreeUpdater> TreeChildrenThisFrame { get; private set; }
		public bool ReloadTreeChildrenOnUpdate { get; set; }

		private bool vIsDestroyed;
		private bool vTreeUpdatablesRequireUpdateThisFrame;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TreeUpdater() {
			TreeUpdatablesThisFrame = new List<ITreeUpdateable>();
			TreeChildrenThisFrame = new List<TreeUpdater>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			ReloadTreeChildrenOnUpdate = true;
			HandleTreeUpdatableChanged();
			FindTreeUpdatablesAndChildren();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			ReloadTreeChildrenOnUpdate = true;
			HandleTreeUpdatableChanged();
			FindTreeUpdatablesAndChildren();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void LateUpdate() {
			CheckForParent();

			if ( !Application.isPlaying && TreeParentThisFrame != null ) {
				return;
			}

			UpdateThisLevelAndDescend(0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnTransformParentChanged() {
			Debug.LogWarning("ParentChanged", this);
			CheckForParent();
		}

		/*--------------------------------------------------------------------------------------------* /
		public void OnDisable() {
			TellChildrenToCheckForParent();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnDestroy() {
			vIsDestroyed = true;
			TellChildrenToCheckForParent();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAtAndBelowThisLevel() {
			UpdateThisLevelAndDescend(0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ImmediateReloadTreeChildren() {
			//int before = TreeChildrenThisFrame.Count;
			FindTreeUpdatablesAndChildren();
			//int after = TreeChildrenThisFrame.Count;
			//Debug.Log("ImmediateReloadTreeChildren: "+name+" / "+before+" => "+after, gameObject);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void SendTreeUpdatableChanged<T>(T pSource, string pNote=null)
																			where T : MonoBehaviour {
			//Debug.Log("TreeUpdater.SendTreeUpdatableChanged: "+pSource.GetType().Name+" / "+pNote);
			//pSource.SendMessage("HandleTreeUpdatableChanged", SendMessageOptions.RequireReceiver);
			pSource.GetComponent<TreeUpdater>().HandleTreeUpdatableChanged();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleTreeUpdatableChanged() {
			if ( vTreeUpdatablesRequireUpdateThisFrame ) {
				return;
			}

			//Debug.Log(Time.frameCount+" | HandleTreeUpdatableChanged: "+name+" / "+
			//	TreeChildrenThisFrame.Count, this);

			Profiler.BeginSample("Changed");
			Profiler.BeginSample("A");
			vTreeUpdatablesRequireUpdateThisFrame = true;
			CheckForParent();
			Profiler.EndSample();
			Profiler.BeginSample("B");
			TreeParentThisFrame?.HandleTreeUpdatableChanged();
			Profiler.EndSample();

			Profiler.BeginSample("C");
			for ( int i = 0 ; i < TreeChildrenThisFrame.Count ; i++ ) {
				TreeUpdater childTreeUp = TreeChildrenThisFrame[i];

				if ( childTreeUp?.gameObject.activeSelf != true ) {
					continue;
				}

				childTreeUp.HandleTreeUpdatableChanged();
			}
			Profiler.EndSample();
			Profiler.EndSample();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void CheckForParent() {
			TreeParentThisFrame = transform.parent?.GetComponent<TreeUpdater>();
			enabled = (!Application.isPlaying || TreeParentThisFrame == null);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void TellChildrenToCheckForParent() {
			FindTreeUpdatablesAndChildren();

			for ( int i = 0 ; i < TreeChildrenThisFrame.Count ; i++ ) {
				TreeChildrenThisFrame[i].CheckForParent();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateThisLevelAndDescend(int pDepth) {
			if ( vIsDestroyed ) {
				return;
			}

			if ( Application.isPlaying && !vTreeUpdatablesRequireUpdateThisFrame ) {
				return;
			}

			if ( ReloadTreeChildrenOnUpdate ) {
				//Profiler.BeginSample("FTUaC");
				FindTreeUpdatablesAndChildren();
				ReloadTreeChildrenOnUpdate = false;
				//Profiler.EndSample();
			}

			SendTreeUpdates(pDepth);
			DescendTree(pDepth);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void FindTreeUpdatablesAndChildren() {
			if ( TreeChildrenThisFrame.Count > 0 ) {
				TreeChildrenThisFrame.Clear();
			}

			gameObject.GetComponents(TreeUpdatablesThisFrame);

			int childCount = transform.childCount;

			for ( int i = 0 ; i < childCount ; i++ ) {
				TreeUpdater childTreeUp = transform.GetChild(i).GetComponent<TreeUpdater>();

				if ( childTreeUp == null ) {
					continue;
				}

				TreeChildrenThisFrame.Add(childTreeUp);
			}

			//Debug.Log(Time.frameCount+" | CHILDREN: "+gameObject.name+" / "+
			//	TreeChildrenThisFrame.Count, this);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void SendTreeUpdates(int pDepth) {
			if ( vIsDestroyed ) {
				return;
			}

			for ( int i = 0 ; i < TreeUpdatablesThisFrame.Count ; i++ ) {
				ITreeUpdateable treeUp = TreeUpdatablesThisFrame[i];

				if ( treeUp == null ) {
					if ( !ReloadTreeChildrenOnUpdate ) {
						ReloadTreeChildrenOnUpdate = true;
						Debug.Log("Lost tree sibling, will refresh list next frame: "+i, this);
					}

					continue;
				}

				if ( !treeUp.gameObject.activeSelf ) {
					continue;
				}

				Profiler.BeginSample(treeUp.GetType().Name);
				treeUp.TreeUpdate();
				Profiler.EndSample();
			}

			vTreeUpdatablesRequireUpdateThisFrame = false;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void DescendTree(int pDepth) {
			if ( vIsDestroyed ) {
				return;
			}

			/*if ( TreeChildrenThisFrame.Count == 0 ) {
				FindTreeUpdatablesAndChildren();
			}*/

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

				if ( !childTreeUp.gameObject.activeSelf ) {
					continue;
				}

				childTreeUp.UpdateThisLevelAndDescend(childDepth);
			}
		}

	}

}
