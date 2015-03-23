using System;
using Hover.Cast.Custom;
using Hover.Cast.State;
using Hover.Common.Items.Types;
using Hover.Common.Util;
using UnityEngine;
using Hover.Common.State;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public class UiItem : MonoBehaviour {

		public float ArcAngle { get; private set; }

		private ArcState vArcState;
		private BaseItemState vItemState;

		private Transform vCursorBaseTx;
		private float vSlideDegrees;
		private Vector3 vSlideDir0;
		private Vector3 vCursorLocalPos;

		private IUiItemRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, BaseItemState pItemState, float pArcAngle, 
																IItemVisualSettings pVisualSettings) {
			vArcState = pArcState;
			vItemState = pItemState;
			ArcAngle = pArcAngle;

			vItemState.SetCursorDistanceFunction(CalcCursorDistance);

			vCursorBaseTx = gameObject.transform.parent.parent.parent.parent; //HovercastSetup

			const float pi = (float)Math.PI;
			const float slideBufferAngle = pi/80f;

			vSlideDegrees = (pArcAngle-slideBufferAngle*2)/(float)Math.PI*180;
			vSlideDir0 = MeshUtil.GetRingPoint(1, -pArcAngle/2f+slideBufferAngle);

			////

			var rendObj = new GameObject("Renderer");
			rendObj.transform.SetParent(gameObject.transform, false);

			vRenderer = (IUiItemRenderer)rendObj.AddComponent(pVisualSettings.Renderer);
			vRenderer.Build(vArcState, vItemState, pArcAngle, pVisualSettings);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			TryUpdateSliderValue();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private float CalcCursorDistance(Vector3 pCursorPos) {
			var cursorWorldPos = vCursorBaseTx.TransformPoint(pCursorPos);
			vCursorLocalPos = gameObject.transform.InverseTransformPoint(cursorWorldPos);

			Vector3 nearest = vRenderer.GetPointNearestToCursor(vCursorLocalPos);
			return (nearest-vCursorLocalPos).magnitude;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		internal void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			//TODO: vItemState.SetIsAnimating(pProgress < 1);
			vRenderer.HandleChangeAnimation(pFadeIn, pDirection, pProgress);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void TryUpdateSliderValue() {
			ISliderItem sliderItem = (vItemState.Item as ISliderItem);

			if ( sliderItem == null ) {
				return;
			}

			if ( !vItemState.IsNearestHighlight || vItemState.MaxHighlightProgress <= 0 ) {
				sliderItem.HoverValue = null;
				return;
			}

			Vector3 cursorLocal = vCursorLocalPos;
			cursorLocal.y = 0;

			Vector3 cursorDir = cursorLocal.normalized;
			Quaternion diff = Quaternion.FromToRotation(vSlideDir0, cursorDir);

			float cursorDeg;
			Vector3 cursorAxis;
			diff.ToAngleAxis(out cursorDeg, out cursorAxis);

			if ( cursorAxis.y < 0 ) {
				cursorDeg = 0;
			}

			if ( sliderItem.IsStickySelected ) {
				sliderItem.Value = cursorDeg/vSlideDegrees;
				sliderItem.HoverValue = null;
			}
			else if ( sliderItem.AllowJump ) {
				sliderItem.HoverValue = cursorDeg/vSlideDegrees;
			}
			else {
				sliderItem.HoverValue = null;
			}
		}

	}

}
