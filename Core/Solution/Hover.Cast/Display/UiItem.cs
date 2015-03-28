using System;
using Hover.Cast.State;
using Hover.Common.Custom;
using Hover.Common.Items.Types;
using Hover.Common.State;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public class UiItem : MonoBehaviour {

		public float ArcAngle { get; private set; }

		private MenuState vMenuState;
		private BaseItemState vItemState;

		private float vSlideDegrees;
		private Vector3 vSlideDir0;

		private GameObject vRendererObj;
		private IUiItemRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(MenuState pMenuState, BaseItemState pItemState, float pArcAngle, 
																IItemVisualSettings pVisualSettings) {
			vMenuState = pMenuState;
			vItemState = pItemState;
			ArcAngle = pArcAngle;

			const float pi = (float)Math.PI;
			const float slideBufferAngle = pi/80f;

			vSlideDegrees = (pArcAngle-slideBufferAngle*2)/(float)Math.PI*180;
			vSlideDir0 = MeshUtil.GetRingPoint(1, -pArcAngle/2f+slideBufferAngle);

			////

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(gameObject.transform, false);

			vRenderer = (IUiItemRenderer)vRendererObj.AddComponent(pVisualSettings.Renderer);
			vRenderer.Build(vMenuState, vItemState, pArcAngle, pVisualSettings);

			vItemState.HoverPointUpdater = vRenderer.UpdateHoverPoints;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			TryUpdateSliderValue();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			vItemState.PreventSelectionViaDisplay("anim", (pProgress < 1));
			vRenderer.HandleChangeAnimation(pFadeIn, pDirection, pProgress);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void TryUpdateSliderValue() {
			ISliderItem sliderItem = (vItemState.Item as ISliderItem);

			if ( sliderItem == null ) {
				return;
			}

			Vector3? cursorWorld = vItemState.NearestCursorWorldPos;
			bool ignoreSlider = (!vItemState.IsNearestHighlight || 
				vItemState.MaxHighlightProgress <= 0 || cursorWorld == null);

			if ( ignoreSlider ) {
				sliderItem.HoverValue = null;
				return;
			}

			Vector3 cursorLocal = gameObject.transform.InverseTransformPoint((Vector3)cursorWorld);
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
