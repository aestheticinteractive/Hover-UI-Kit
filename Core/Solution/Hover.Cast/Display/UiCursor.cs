using System;
using Hover.Cast.Custom;
using Hover.Cast.State;
using UnityEngine;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public class UiCursor : MonoBehaviour {

		private ArcState vArcState;
		private CursorState vCursorState;
		private GameObject vRendererHold;
		private GameObject vRendererObj;
		private IUiCursorRenderer vRenderer;
		private Transform vCameraTx;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, CursorState pCursorState, ICustomCursor pCustom, 
																		Transform pCameraTransform) {
			vArcState = pArcState;
			vCursorState = pCursorState;
			vCameraTx = pCameraTransform;

			////
			
			Type rendType = pCustom.GetCursorRenderer();

			vRendererHold = new GameObject("RendererHold");
			vRendererHold.transform.SetParent(gameObject.transform, false);

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(vRendererHold.transform, false);

			vRenderer = (IUiCursorRenderer)vRendererObj.AddComponent(rendType);
			vRenderer.Build(vArcState, vCursorState, pCustom.GetCursorSettings());
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !vCursorState.IsInputAvailable || vArcState.DisplayStrength <= 0 ) {
				vRendererHold.SetActive(false);
				return;
			}

			vRendererHold.SetActive(true);

			Transform tx = gameObject.transform;
			tx.localPosition = vCursorState.Position;
			tx.localRotation = Quaternion.identity;
			tx.localScale = Vector3.one*(vArcState.Size*UiMenu.ScaleArcSize);

			Vector3 camWorld = vCameraTx.TransformPoint(Vector3.zero);
			Vector3 camLocal = tx.InverseTransformPoint(camWorld);
			tx.localRotation = Quaternion.FromToRotation(Vector3.down, camLocal);
		}

	}

}
