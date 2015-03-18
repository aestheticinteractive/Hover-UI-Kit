using Hover.Cursor.Custom;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cursor.Display {

	/*================================================================================================*/
	public class UiCursor : MonoBehaviour {

		private CursorState vCursorState;
		private Transform vCameraTx;

		private GameObject vCursorRendererHold;
		private GameObject vCursorRendererObj;
		private IUiCursorRenderer vCursorRenderer;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(CursorState pCursorState, CursorSettings pSettings, Transform pCameraTx) {
			vCursorState = pCursorState;
			vCameraTx = pCameraTx;
			
			vCursorRendererHold = new GameObject("CursorRendererHold");
			vCursorRendererHold.transform.SetParent(gameObject.transform, false);

			vCursorRendererObj = new GameObject("CursorRenderer");
			vCursorRendererObj.transform.SetParent(vCursorRendererHold.transform, false);

			vCursorRenderer = (IUiCursorRenderer)vCursorRendererObj.AddComponent(pSettings.Renderer);
			vCursorRenderer.Build(vCursorState, pSettings);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !vCursorState.IsInputAvailable || vCursorState.GetMaxDisplayStrength() <= 0 ) {
				vCursorRendererHold.SetActive(false);
				return;
			}

			vCursorRendererHold.SetActive(true);

			Transform holdTx = vCursorRendererHold.transform;
			holdTx.localPosition = vCursorState.Position;
			holdTx.localRotation = Quaternion.identity;

			Vector3 camWorld = vCameraTx.TransformPoint(Vector3.zero);
			Vector3 camLocal = holdTx.InverseTransformPoint(camWorld);
			holdTx.localRotation = Quaternion.FromToRotation(Vector3.down, camLocal);
		}

	}

}
