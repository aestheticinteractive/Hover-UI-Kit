using System;
using Hover.Board.Custom;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiItem : MonoBehaviour {

		public const float Size = 1;

		private BaseItemState vButtonState;

		private GameObject vRendererObj;
		private IUiItemRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(BaseItemState pButtonState, ICustomSegment pCustom) {
			vButtonState = pButtonState;
			vButtonState.SetCursorDistanceFunction(CalcCursorDistance);

			////

			Type rendType = pCustom.GetSegmentRenderer(vButtonState.Item);
			ItemSettings sett = pCustom.GetSegmentSettings(vButtonState.Item);

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(gameObject.transform, false);

			vRenderer = (IUiItemRenderer)vRendererObj.AddComponent(rendType);
			vRenderer.Build(vButtonState, sett);

			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private float CalcCursorDistance(Vector3 pCursorWorldPos) {
			Vector3 cursorLocalPos = vRendererObj.transform.InverseTransformPoint(pCursorWorldPos);
			Vector3 nearestLocal = vRenderer.GetPointNearestToCursor(cursorLocalPos);
			Vector3 nearestWorld = vRendererObj.transform.TransformPoint(nearestLocal);
			return (nearestWorld-pCursorWorldPos).magnitude;
		}

	}

}
