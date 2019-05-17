using System;
using Hover.Core.Renderers;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverMesh))]
	public class HoverAlphaMeshUpdater : TreeUpdateableBehavior, ISettingsController {
	
		public const string SortingLayerName = "SortingLayer";
		public const string AlphaName = "Alpha";

		public ISettingsControllerMap Controllers { get; private set; }

		[SerializeField]
		[DisableWhenControlled(DisplaySpecials=true)]
		[FormerlySerializedAs("SortingLayer")]
		private string _SortingLayer = "Default";

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("SortingOrder")]
		private int _SortingOrder = 0;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		[FormerlySerializedAs("Alpha")]
		private float _Alpha = 1;

		[SerializeField]
		[ColorUsage(true, true)]
		[FormerlySerializedAs("StandardColor")]
		private Color _StandardColor = Color.gray;

		[SerializeField]
		[ColorUsage(true, true)]
		[FormerlySerializedAs("SliderFillColor")]
		private Color _SliderFillColor = Color.white;

		[SerializeField]
		[ColorUsage(true, true)]
		[FormerlySerializedAs("FlashColor")]
		private Color _FlashColor = Color.white;

		[SerializeField]
		[Range(0, 2000)]
		[FormerlySerializedAs("FlashColorMilliseconds")]
		private float _FlashColorMilliseconds = 400;

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
		public string SortingLayer {
			get => _SortingLayer;
			set => this.UpdateValueWithTreeMessage(ref _SortingLayer, value, "SortingLayer");
		}

		/*--------------------------------------------------------------------------------------------*/
		public int SortingOrder {
			get => _SortingOrder;
			set => this.UpdateValueWithTreeMessage(ref _SortingOrder, value, "SortingOrder");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float Alpha {
			get => _Alpha;
			set => this.UpdateValueWithTreeMessage(ref _Alpha, value, "Alpha");
		}

		/*--------------------------------------------------------------------------------------------*/
		public Color StandardColor {
			get => _StandardColor;
			set => this.UpdateValueWithTreeMessage(ref _StandardColor, value, "StandardColor");
		}

		/*--------------------------------------------------------------------------------------------*/
		public Color SliderFillColor {
			get => _SliderFillColor;
			set => this.UpdateValueWithTreeMessage(ref _SliderFillColor, value, "SliderFillColor");
		}

		/*--------------------------------------------------------------------------------------------*/
		public Color FlashColor {
			get => _FlashColor;
			set => this.UpdateValueWithTreeMessage(ref _FlashColor, value, "FlashColor");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float FlashColorMilliseconds {
			get => _FlashColorMilliseconds;
			set => this.UpdateValueWithTreeMessage(ref _FlashColorMilliseconds, value, "FlashColorMs");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vPrevColor = GetDisplayColor(GetComponent<HoverMesh>());
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
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
					TreeUpdater.SendTreeUpdatableChanged(this, "Flash");
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
