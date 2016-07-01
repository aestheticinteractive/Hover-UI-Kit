using System;
using Hover.Renderers;
using Hover.Utils;
using UnityEngine;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverMesh))]
	public class HoverAlphaMeshUpdater : MonoBehaviour, ITreeUpdateable {
	
		public const string SortingLayerName = "SortingLayer";
		public const string AlphaName = "Alpha";
		
		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(DisplayMessage=true)]
		public string SortingLayer = "Default";

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;

		[DisableWhenControlled]
		public Color StandardColor = Color.gray;
		
		[DisableWhenControlled]
		public Color SliderFillColor = Color.white;

		private string vPrevLayer;
		private float vPrevAlpha;
		private Color vPrevColor;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverAlphaMeshUpdater() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HoverMesh hoverMesh = GetComponent<HoverMesh>();

			TryUpdateLayer(hoverMesh);
			TryUpdateColor(hoverMesh);

			vPrevLayer = SortingLayer;
			vPrevAlpha = Alpha;
			vPrevColor = StandardColor;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TryUpdateLayer(HoverMesh pHoverMesh) {
			if ( !pHoverMesh.DidRebuildMesh && SortingLayer == vPrevLayer ) {
				return;
			}

			gameObject.GetComponent<MeshRenderer>().sortingLayerName = SortingLayer;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void TryUpdateColor(HoverMesh pHoverMesh) {
			Color useColor;

			switch ( pHoverMesh.DisplayMode ) {
				case HoverMesh.DisplayModeType.Standard:
					useColor = StandardColor;
					break;

				case HoverMesh.DisplayModeType.SliderFill:
					useColor = SliderFillColor;
					break;

				default:
					throw new Exception("Unhandled display mode: "+pHoverMesh.DisplayMode);
			}

			if ( !pHoverMesh.DidRebuildMesh && Alpha == vPrevAlpha && useColor == vPrevColor ) {
				return;
			}

			Color colorForAllVertices = DisplayUtil.FadeColor(useColor, Alpha);
			pHoverMesh.Builder.CommitColors(colorForAllVertices);
		}
		
	}

}
