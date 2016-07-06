using System.Collections.Generic;
using UnityEngine;

namespace Hover.Utils {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class TreeUpdater : MonoBehaviour {
		
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
			if ( DidTreeUpdateThisFrame ) {
				return;
			}
			
			AscendTreeOrBegin(true);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void LateUpdate() {
			DidTreeUpdateThisFrame = false;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnDestroy() {
			vIsDestroyed = true;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AscendTreeOrBegin(bool pFromUpdate) {
			//Debug.Log("AscendTreeOrBegin: "+gameObject.name, gameObject);
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

			gameObject.GetComponents<ITreeUpdateable>(TreeUpdatablesThisFrame);
			FindTreeChildren();

			for ( int i = 0 ; i < TreeUpdatablesThisFrame.Count ; i++ ) {
				ITreeUpdateable treeUpdatable = TreeUpdatablesThisFrame[i];
				
				if ( !treeUpdatable.isActiveAndEnabled ) {
					continue;
				}
				
				treeUpdatable.TreeUpdate();
			}
			
			DidTreeUpdateThisFrame = true;
			TreeDepthLevelThisFrame = pDepth;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void FindTreeChildren() {
			TreeChildrenThisFrame.Clear();
			
			foreach ( Transform childTx in transform ) {
				TreeUpdater childTreeUp = childTx.GetComponent<TreeUpdater>();
				
				if ( childTreeUp == null || !childTreeUp.isActiveAndEnabled ) {
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
