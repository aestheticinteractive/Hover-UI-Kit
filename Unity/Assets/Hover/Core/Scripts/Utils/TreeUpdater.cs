using System.Collections.Generic;
using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class TreeUpdater : MonoBehaviour {

		public TreeUpdater TreeParentThisFrame { get; private set; }
		public List<ITreeUpdateable> TreeUpdatablesThisFrame { get; private set; }
		public List<TreeUpdater> TreeChildrenThisFrame { get; private set; }
		public bool ReloadTreeChildrenOnUpdate { get; set; }

		private bool vIsDestroyed;
		private int vTreeUpdatablesRequireUpdateUntilFrame;


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

			//Debug.Log(Time.frameCount+" | ############# LateUpdate: "+transform.ToDebugPath(), this);
			UpdateThisLevelAndDescend(0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnTransformParentChanged() {
			//Debug.Log(Time.frameCount+" | ParentChanged: "+transform.ToDebugPath(), this);
			CheckForParent();
			HandleTreeUpdatableChanged();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnTransformChildrenChanged() {
			//Debug.Log(Time.frameCount+" | ChildrenChanged: "+transform.ToDebugPath(), this);
			ReloadTreeChildrenOnUpdate = true;
			HandleTreeUpdatableChanged();
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
		public static void SendTreeUpdatableChanged<T>(T pSource, string pNote) where T : MonoBehaviour{
			//Debug.Log(Time.frameCount+" | TreeUpdater.SendTreeUpdatableChanged: "+
			//	pSource.GetType().Name+" / '"+pNote+"' / "+
			//	pSource.transform.ToDebugPath(), pSource);
			//pSource.SendMessage("HandleTreeUpdatableChanged", SendMessageOptions.RequireReceiver);
			pSource.GetComponent<TreeUpdater>().HandleTreeUpdatableChanged();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleTreeUpdatableChanged(bool pSendDownward=true, int pDepth=0) {
			int untilFrame = Time.frameCount+1;

			if ( vTreeUpdatablesRequireUpdateUntilFrame >= untilFrame ) {
				return;
			}

			//Debug.Log(Time.frameCount+" | HandleTreeUpdatableChanged: "+pDepth+" / "+untilFrame+" / "+
			//	pSendDownward+" / "+transform.ToDebugPath(), this);

			vTreeUpdatablesRequireUpdateUntilFrame = untilFrame;
			CheckForParent();
			TreeParentThisFrame?.HandleTreeUpdatableChanged(false, pDepth-1);

			for ( int i = 0 ; pSendDownward && i < TreeChildrenThisFrame.Count ; i++ ) {
				TreeUpdater childTreeUp = TreeChildrenThisFrame[i];

				if ( childTreeUp == null || childTreeUp.gameObject.activeSelf != true ) {
					continue;
				}

				childTreeUp.HandleTreeUpdatableChanged(true, pDepth+1);
			}
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
			//Debug.LogWarning(Time.frameCount+" | UpdateThisLevelAndDescend: "+pDepth+" / "+
			//	vTreeUpdatablesRequireUpdateUntilFrame+" / "+transform.ToDebugPath(), this);

			if ( vIsDestroyed ) {
				return;
			}

			if ( vTreeUpdatablesRequireUpdateUntilFrame < Time.frameCount ) {
				return;
			}

			if ( ReloadTreeChildrenOnUpdate ) {
				FindTreeUpdatablesAndChildren();
				ReloadTreeChildrenOnUpdate = false;
			}

			SendTreeUpdates(pDepth);
			DescendTree(pDepth);

			//Debug.DrawLine(transform.position, transform.position+Vector3.forward*0.02f, Color.red);
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

			//Debug.Log(Time.frameCount+" | FindTreeUpdatablesAndChildren: "+childCount+" / "+
			//	TreeChildrenThisFrame.Count, this);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void SendTreeUpdates(int pDepth) {
			if ( vIsDestroyed ) {
				return;
			}

			for ( int i = 0 ; i < TreeUpdatablesThisFrame.Count ; i++ ) {
				ITreeUpdateable treeUpdateable = TreeUpdatablesThisFrame[i];

				if ( treeUpdateable == null ) {
					if ( !ReloadTreeChildrenOnUpdate ) {
						ReloadTreeChildrenOnUpdate = true;
						Debug.LogError("Lost tree sibling, will refresh list next frame: "+i, this);
					}

					continue;
				}

				if ( !treeUpdateable.isActiveAndEnabled ) {
					continue;
				}

				//Profiler.BeginSample(treeUpdateable.TypeName ?? "TreeUpdate");
				treeUpdateable.TreeUpdate();
				//Profiler.EndSample();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void DescendTree(int pDepth) {
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
						Debug.LogError("Lost tree child, will refresh list next frame: "+i, this);
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
