using System;
using Hoverboard.Core.Custom;
using Hoverboard.Core.State;
using UnityEngine;

namespace Hoverboard.Core.Display {

	/*================================================================================================*/
	public class UiButton : MonoBehaviour {

		public const float Size = 1;

		private ButtonState vButtonState;
		private Transform vCursorBaseTx;
		private Vector3 vCursorLocalPos;

		private GameObject vRendererObj;
		private IUiSegmentRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ButtonState pButtonState, Transform pHoverboardTx, ICustomSegment pCustom) {
			vButtonState = pButtonState;
			vButtonState.SetCursorDistanceFunction(CalcCursorDistance);
			vCursorBaseTx = pHoverboardTx;

			////

			Type rendType = pCustom.GetSegmentRenderer(vButtonState.NavItem);
			ButtonSettings sett = pCustom.GetSegmentSettings(vButtonState.NavItem);

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(gameObject.transform, false);

			vRenderer = (IUiSegmentRenderer)vRendererObj.AddComponent(rendType);
			vRenderer.Build(vButtonState, sett);

			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private float CalcCursorDistance(Vector3 pCursorPos) {
			var cursorWorldPos = vCursorBaseTx.TransformPoint(pCursorPos);
			vCursorLocalPos = vRendererObj.transform.InverseTransformPoint(cursorWorldPos);

			Vector3 nearest = vRenderer.GetPointNearestToCursor(vCursorLocalPos);
			return (nearest-vCursorLocalPos).magnitude;
		}

	}

}
