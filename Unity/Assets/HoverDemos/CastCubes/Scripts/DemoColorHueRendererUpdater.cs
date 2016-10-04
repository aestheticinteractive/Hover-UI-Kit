using Hover.Core.Items.Types;
using Hover.Core.Renderers;
using Hover.Core.Renderers.Items.Buttons;
using Hover.Core.Renderers.Items.Sliders;
using Hover.Core.Utils;
using Hover.RendererModules.Alpha;
using UnityEngine;

namespace HoverDemos.CastCubes {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverItemDataSlider))]
	public class DemoColorHueRendererUpdater : MonoBehaviour, ITreeUpdateable {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HoverItemDataSlider slider = GetComponent<HoverItemDataSlider>();
			Color col = DisplayUtil.HsvToColor(slider.RangeValue, 1, 0.666f);

			UpdateButtonsColor(col);
			UpdateTrackColor(col);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateButtonsColor(Color pColor) {
			HoverFillButton[] buttons = GetComponentsInChildren<HoverFillButton>();

			foreach ( HoverFillButton fillButton in buttons ) {
				fillButton.Selection.GetComponent<HoverAlphaMeshUpdater>().StandardColor = pColor;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTrackColor(Color pColor) {
			HoverFillSlider fill = GetComponentInChildren<HoverFillSlider>();
			Color fadedColor = DisplayUtil.FadeColor(pColor, 0.25f);

			UpdateTrackSegmentColor(fill.SegmentA, fadedColor);
			UpdateTrackSegmentColor(fill.SegmentB, fadedColor);
			UpdateTrackSegmentColor(fill.SegmentC, fadedColor);
			UpdateTrackSegmentColor(fill.SegmentD, fadedColor);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTrackSegmentColor(HoverMesh pMesh, Color pColor) {
			HoverAlphaMeshUpdater alphaUp = pMesh.GetComponent<HoverAlphaMeshUpdater>();
			alphaUp.StandardColor = pColor;
			alphaUp.SliderFillColor = pColor;
		}

	}

}
