using System;
using Hoverboard.Core.Custom;
using Hoverboard.Core.State;
using UnityEngine;

namespace Hoverboard.Core.Display {

	/*================================================================================================*/
	public class UiCursor : MonoBehaviour {

		private CursorState vCursorState;
		private GameObject vRendererHold;
		private GameObject vRendererObj;
		private IUiCursorRenderer vRenderer;
		private Transform vCameraTx;

		private float vCurrInnerRadius;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(CursorState pCursorState, ICustomCursor pCustom, 
																		Transform pCameraTransform) {
			vCursorState = pCursorState;
			vCameraTx = pCameraTransform;

			////
			
			Type rendType = pCustom.GetCursorRenderer();

			vRendererHold = new GameObject("RendererHold");
			vRendererHold.transform.SetParent(gameObject.transform, false);

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(vRendererHold.transform, false);

			vRenderer = (IUiCursorRenderer)vRendererObj.AddComponent(rendType);
			vRenderer.Build(vCursorState, pCustom.GetCursorSettings());
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !vCursorState.IsInputAvailable ) {
				vRendererHold.SetActive(false);
				return;
			}

			vRendererHold.SetActive(true);

			Transform tx = gameObject.transform;
			tx.localPosition = vCursorState.Position;
			tx.localRotation = Quaternion.identity;
			tx.localScale = Vector3.one*vCursorState.Size;

			Vector3 camWorld = vCameraTx.TransformPoint(Vector3.zero);
			Vector3 camLocal = tx.InverseTransformPoint(camWorld);
			tx.localRotation = Quaternion.FromToRotation(Vector3.down, camLocal);
		}

	}

}
