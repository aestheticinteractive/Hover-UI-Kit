using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public abstract class HoverSceneAdjust : MonoBehaviour {

		private bool vIsComplete;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			TrySceneUpdates();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			TrySceneUpdates();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TrySceneUpdates() {
			if ( vIsComplete || !IsReadyToAdjust() ) {
				return;
			}

			PerformAdjustments();
			vIsComplete = true;

			////

			string completeMsg = GetType().Name+" ('"+name+"') is complete.";

			if ( Application.isPlaying ) {
				enabled = false;
				Debug.Log(completeMsg+" Component disabled for runtime.", this);
			}
			else {
				Debug.Log(completeMsg, this);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void Deactivate(GameObject pObj) {
			pObj.SetActive(false);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void Deactivate(MonoBehaviour pComp) {
			pComp.gameObject.SetActive(false);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract bool IsReadyToAdjust();
		protected abstract void PerformAdjustments();

	}

}
