using System;
using Henu.Settings;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiCursor : MonoBehaviour {

		private ArcState vArcState;
		private CursorState vCursorState;
		private GameObject vRendererObj;
		private IUiCursorRenderer vRenderer;
		private Transform vCameraTx;

		private float vCurrInnerRadius;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, CursorState pCursorState, ISettings pSettings) {
			vArcState = pArcState;
			vCursorState = pCursorState;
			vCameraTx = GameObject.Find("HandController").transform;

			////
			
			Type rendType = pSettings.GetUiCursorRendererType();

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(gameObject.transform, false);

			vRenderer = (IUiCursorRenderer)vRendererObj.AddComponent(rendType);
			vRenderer.Build(vArcState, vCursorState);
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

			Vector3 camWorld = vCameraTx.transform.TransformPoint(Vector3.zero);
			Vector3 camLocal = tx.InverseTransformPoint(camWorld);
			tx.localRotation = Quaternion.FromToRotation(Vector3.down, camLocal);
		}

	}

}
