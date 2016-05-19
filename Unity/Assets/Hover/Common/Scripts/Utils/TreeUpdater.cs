using System;
using UnityEngine;

namespace Hover.Common.Utils {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class TreeUpdater : MonoBehaviour {
		
		public bool DidTreeUpdateThisFrame { get; private set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( DidTreeUpdateThisFrame ) {
				return;
			}
			
			AscendOrBegin(true);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void LateUpdate() {
			DidTreeUpdateThisFrame = false;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AscendOrBegin(bool pFromUpdate) {
			//Debug.Log("AscendOrBegin: "+gameObject.name, gameObject);
			Transform parTx = transform.parent;
			TreeUpdater parTreeUp = (parTx == null ? null : parTx.GetComponent<TreeUpdater>());
			
			if ( parTreeUp == null || !parTreeUp.isActiveAndEnabled ) {
				BeginAtThisLevel();
				return;
			}
			
			parTreeUp.AscendOrBegin(false);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void BeginAtThisLevel() {
			//Debug.Log("BeginAtThisLevel: "+gameObject.name, gameObject);
			SendAndDescend(0);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void SendAndDescend(int pDepth) {
			//Debug.Log(new string('-', pDepth)+"SendAndDescend: "+gameObject.name, gameObject);
			
			//TODO: SendMessage() also sends to inactive components, which is not desired
			//TODO: use an ITreeUpdateable interface for this?
			//TODO: optionally provide an update-order of components within each GameObject
			SendMessage("TreeUpdate", SendMessageOptions.DontRequireReceiver);
			DidTreeUpdateThisFrame = true;
			
			int childDepth = pDepth+1;
			
			foreach ( Transform childTx in transform ) {
				TreeUpdater childTreeUp = childTx.GetComponent<TreeUpdater>();
				
				if ( childTreeUp == null || !childTreeUp.isActiveAndEnabled ) {
					continue;
				}
				
				childTreeUp.SendAndDescend(childDepth);
			}
		}

	}

}
