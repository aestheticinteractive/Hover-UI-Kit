using System;
using Hover.Core.Renderers;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverMesh))]
	public class HoverAlphaMeshUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {
	
		public const string SortingLayerName = "SortingLayer";
		public const string AlphaName = "Alpha";
		
		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(DisplaySpecials=true)]
		public string SortingLayer = "Default";
		
		[DisableWhenControlled]
		public int SortingOrder = 0;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;

		[ColorUsage(true, true, 0, 1000, 0, 1000)]
		public Color StandardColor = Color.gray;
		
		[ColorUsage(true, true, 0, 1000, 0, 1000)]
		public Color SliderFillColor = Color.white;

		[ColorUsage(true, true, 0, 1000, 0, 1000)]
		public Color FlashColor = Color.white;

		[Range(0, 2000)]
		public float FlashColorMilliseconds = 400;

		private string vPrevLayer;
		private int vPrevOrder;
		private float vPrevAlpha;
		private Color vPrevColor;
		private Color vCurrColor;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverAlphaMeshUpdater() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vPrevColor = GetDisplayColor(GetComponent<HoverMesh>());
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			vPrevColor = GetDisplayColor(GetComponent<HoverMesh>());
		}

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
			vPrevOrder = SortingOrder;
			vPrevAlpha = Alpha;
			vPrevColor = vCurrColor;

			Controllers.TryExpireControllers();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TryUpdateLayer(HoverMesh pHoverMesh) {
			Controllers.Set(SettingsControllerMap.MeshRendererSortingLayer, this);
			Controllers.Set(SettingsControllerMap.MeshRendererSortingOrder, this);

			if ( !pHoverMesh.DidRebuildMesh && SortingLayer == vPrevLayer && 
					SortingOrder == vPrevOrder ) {
				return;
			}

			MeshRenderer meshRend = gameObject.GetComponent<MeshRenderer>();
			meshRend.sortingLayerName = SortingLayer;
			meshRend.sortingOrder = SortingOrder;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void TryUpdateColor(HoverMesh pHoverMesh) {
			Controllers.Set(SettingsControllerMap.MeshColors, this);

			vCurrColor = GetDisplayColor(pHoverMesh);

			if ( FlashColorMilliseconds > 0 ) {
				TimeSpan test = DateTime.UtcNow-GetComponent<HoverIndicator>().LatestSelectionTime;

				if ( test.TotalMilliseconds < FlashColorMilliseconds ) {
					vCurrColor = Color.Lerp(FlashColor, vCurrColor, 
						(float)test.TotalMilliseconds/FlashColorMilliseconds);
				}
			}

			if ( !pHoverMesh.DidRebuildMesh && Alpha == vPrevAlpha && vCurrColor == vPrevColor ) {
				return;
			}

			Color colorForAllVertices = DisplayUtil.FadeColor(vCurrColor, Alpha);
			pHoverMesh.Builder.CommitColors(colorForAllVertices);
		}

		/*--------------------------------------------------------------------------------------------*/
		private Color GetDisplayColor(HoverMesh pHoverMesh) {
			switch ( pHoverMesh.DisplayMode ) {
				case HoverMesh.DisplayModeType.Standard:
					return StandardColor;

				case HoverMesh.DisplayModeType.SliderFill:
					return SliderFillColor;

				default:
					throw new Exception("Unhandled display mode: "+pHoverMesh.DisplayMode);
			}
		}

	}

}
