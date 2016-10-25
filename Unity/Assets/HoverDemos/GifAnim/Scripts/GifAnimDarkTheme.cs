using System;
using Hover.Core.Renderers;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Items.Buttons;
using Hover.Core.Renderers.Items.Sliders;
using Hover.RendererModules.Alpha;
using UnityEngine;
using UnityEngine.UI;

namespace HoverDemos.GifAnim {

	/*================================================================================================*/
	public class GifAnimDarkTheme : MonoBehaviour {

		[Serializable]
		public class Theme {
			public Color TextColor = new Color(1, 1, 1);
			public Color IconColor = new Color(1, 1, 1);
			public Color BackgroundColor = new Color(0.03f, 0.03f, 0.03f, 0.7f);
			public Color HighlightColor = new Color(0.14f, 0.14f, 0.14f, 0.7f);
			public Color SelectionColor = new Color(0.3f, 0.3f, 0.3f, 0.7f);
			public Color EdgeColor = new Color(0.7f, 0.7f, 0.7f, 1);
			public Color FlashColor = new Color(0.7f, 0.7f, 0.7f, 1);
			public Color SliderTrackColor = new Color(0.03f, 0.03f, 0.03f, 0.35f);
			public Color SliderFillColor = new Color(0.14f, 0.14f, 0.14f, 0.5f);
			public Color SliderTickColor = new Color(1, 1, 1, 0.15f);
		}

		public Theme DarkTheme = new Theme();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			SetTheme(DarkTheme);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void SetTheme(Theme pTheme) {
			HoverFillButton[] buttonFills = GetComponentsInChildren<HoverFillButton>(true);
			HoverFillSlider[] sliderFills = GetComponentsInChildren<HoverFillSlider>(true);
			HoverCanvas[] canvases = GetComponentsInChildren<HoverCanvas>(true);
			Color trackCol = pTheme.SliderTrackColor;

			foreach ( HoverFillButton fill in buttonFills ) {
				SetMeshColors(fill.Background, pTheme.BackgroundColor, pTheme.FlashColor);
				SetMeshColors(fill.Highlight, pTheme.HighlightColor, pTheme.FlashColor);
				SetMeshColors(fill.Selection, pTheme.SelectionColor, pTheme.FlashColor);
				SetMeshColors(fill.Edge, pTheme.EdgeColor, pTheme.FlashColor);
			}

			foreach ( HoverFillSlider fill in sliderFills ) {
				SetMeshColors(fill.SegmentA, trackCol, pTheme.FlashColor, pTheme.SliderFillColor);
				SetMeshColors(fill.SegmentB, trackCol, pTheme.FlashColor, pTheme.SliderFillColor);
				SetMeshColors(fill.SegmentC, trackCol, pTheme.FlashColor, pTheme.SliderFillColor);
				SetMeshColors(fill.SegmentD, trackCol, pTheme.FlashColor, pTheme.SliderFillColor);

				foreach ( HoverMesh tickMesh in fill.Ticks ) {
					SetMeshColors(tickMesh, pTheme.SliderTickColor, pTheme.FlashColor);
				}
			}

			foreach ( HoverCanvas canvas in canvases ) {
				canvas.Label.TextComponent.color = pTheme.TextColor;

				//get all icons, including custom ones (like the Hovercast "open" button)
				RawImage[] iconImages = canvas.GetComponentsInChildren<RawImage>(true);

				foreach ( RawImage iconImage in iconImages ) {
					iconImage.color = pTheme.IconColor;
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetMeshColors(HoverMesh pMesh, Color pStandardColor, Color pFlashColor) {
			SetMeshColors(pMesh, pStandardColor, pFlashColor, pStandardColor);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetMeshColors(HoverMesh pMesh, Color pStandardColor, 
																Color pFlashColor, Color pFillColor) {
			HoverAlphaMeshUpdater alphaUp = pMesh.GetComponent<HoverAlphaMeshUpdater>();
			alphaUp.StandardColor = pStandardColor;
			alphaUp.FlashColor = pFlashColor;
			alphaUp.SliderFillColor = pFillColor;
		}

	}

}
