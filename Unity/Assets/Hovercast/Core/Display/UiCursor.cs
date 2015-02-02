using System;
using Hovercast.Core.Settings;
using Hovercast.Core.State;
using UnityEngine;

namespace Hovercast.Core.Display {

	/*================================================================================================*/
	public class UiCursor : MonoBehaviour {

		private ArcState vArcState;
		private CursorState vCursorState;
		private GameObject vRendererHold;
		private GameObject vRendererObj;
		private IUiCursorRenderer vRenderer;
		private Transform vCameraTx;

		private float vCurrInnerRadius;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, CursorState pCursorState, ISettings pSettings) {
			vArcState = pArcState;
			vCursorState = pCursorState;
			//TODO: get the camera's transform, not HovercastSetup's transform
			vCameraTx = gameObject.transform.parent; //HovercastSetup

			////
			
			Type rendType = pSettings.GetUiCursorRendererType();

			vRendererHold = new GameObject("RendererHold");
			vRendererHold.transform.SetParent(gameObject.transform, false);

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(vRendererHold.transform, false);

			vRenderer = (IUiCursorRenderer)vRendererObj.AddComponent(rendType);
			vRenderer.Build(vArcState, vCursorState, pSettings.GetCursorSettings());
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vCursorState.Position == null || vArcState.DisplayStrength <= 0 ) {
				vRendererObj.SetActive(false);
				return;
			}

			vRendererObj.SetActive(true);

			Transform tx = gameObject.transform;
			tx.localPosition = (Vector3)vCursorState.Position;
			tx.localRotation = Quaternion.identity;
			tx.localScale = Vector3.one*(vArcState.Size*UiMenu.ScaleArcSize);

			Vector3 camWorld = vCameraTx.transform.TransformPoint(Vector3.zero);
			Vector3 camLocal = tx.InverseTransformPoint(camWorld);
			tx.localRotation = Quaternion.FromToRotation(Vector3.down, camLocal);
		}

	}

}
