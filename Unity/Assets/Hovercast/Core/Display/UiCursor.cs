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
		private Vector3 vStartRotDir;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, CursorState pCursorState, ISettings pSettings) {
			vArcState = pArcState;
			vCursorState = pCursorState;
			vCameraTx = gameObject.transform.parent; //HovercastSetup

			vStartRotDir = vCursorState.PalmDirection;

			if ( vCursorState.PalmDirection.z != 0 ) { //TODO: find general solution
				vStartRotDir *= -1; //HACK: this approximates thes desired result
			}

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
			if ( vCursorState.Position == null || vArcState.Strength <= 0 ) {
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
			tx.localRotation = Quaternion.FromToRotation(vStartRotDir, camLocal);
		}

	}

}
