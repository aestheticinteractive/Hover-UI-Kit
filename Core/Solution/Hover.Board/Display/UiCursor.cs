using System;
using Hover.Board.Custom;
using Hover.Board.Display.Default;
using Hover.Board.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiCursor : MonoBehaviour {

		private CursorState vCursorState;
		private Transform vCameraTx;
		private float vCurrInnerRadius;

		private GameObject vCursorRendererHold;
		private GameObject vCursorRendererObj;
		private IUiCursorRenderer vCursorRenderer;

		private GameObject vProjRendererHold;
		private GameObject vProjRendererObj;
		private UiProjectionRenderer vProjRenderer; //TODO: use interface
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(CursorState pCursorState, ICustomCursor pCustom, Transform pCameraTx) {
			vCursorState = pCursorState;
			vCameraTx = pCameraTx;

			////
			
			vCursorRendererHold = new GameObject("CursorRendererHold");
			vCursorRendererHold.transform.SetParent(gameObject.transform, false);

			vCursorRendererObj = new GameObject("CursorRenderer");
			vCursorRendererObj.transform.SetParent(vCursorRendererHold.transform, false);

			Type rendType = pCustom.GetCursorRenderer();

			vCursorRenderer = (IUiCursorRenderer)vCursorRendererObj.AddComponent(rendType);
			vCursorRenderer.Build(vCursorState, pCustom.GetCursorSettings());

			////

			vProjRendererHold = new GameObject("ProjectionRendererHold");
			vProjRendererHold.transform.SetParent(gameObject.transform, false);

			vProjRendererObj = new GameObject("ProjectionRenderer");
			vProjRendererObj.transform.SetParent(vProjRendererHold.transform, false);

			vProjRenderer = vProjRendererObj.AddComponent<UiProjectionRenderer>();
			vProjRenderer.Build(vCursorState, pCustom.GetCursorSettings());
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !vCursorState.IsInputAvailable ) {
				vCursorRendererHold.SetActive(false);
				vProjRendererHold.SetActive(false);
				return;
			}

			////

			vCursorRendererHold.SetActive(true);

			Transform tx = vCursorRendererHold.transform;
			tx.localPosition = vCursorState.Position;
			tx.localRotation = Quaternion.identity;

			Vector3 camWorld = vCameraTx.TransformPoint(Vector3.zero);
			Vector3 camLocal = tx.InverseTransformPoint(camWorld);
			tx.localRotation = Quaternion.FromToRotation(Vector3.down, camLocal);

			////

			if ( vCursorState.ProjectedPanelPosition == null || 
					vCursorState.ProjectedPanelProgress <= 0 ) {
				vProjRendererHold.SetActive(false);
				return;
			}

			Vector3 projPos = (Vector3)vCursorState.ProjectedPanelPosition;
			Vector3 projPosToCursor = (vCursorState.Position-projPos);

			vProjRendererHold.SetActive(true);
			vProjRendererHold.transform.localPosition = projPos;
			vProjRendererHold.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.up, projPosToCursor);
		}

	}

}
