using Hover.Core.Renderers;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverRenderer))]
	public class HoverAlphaRendererUpdater : TreeUpdateableBehavior, ISettingsController {

		public const string SortingLayerName = "SortingLayer";
		public const string MasterAlphaName = "MasterAlpha";
		public const string EnabledAlphaName = "EnabledAlpha";
		public const string DisabledAlphaName = "DisabledAlpha";

		public ISettingsControllerMap Controllers { get; private set; }

		[SerializeField]
		[DisableWhenControlled(DisplaySpecials=true)]
		[FormerlySerializedAs("SortingLayer")]
		private string _SortingLayer = "Default";

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		[FormerlySerializedAs("MasterAlpha")]
		private float _MasterAlpha = 1;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		[FormerlySerializedAs("EnabledAlpha")]
		private float _EnabledAlpha = 1;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		[FormerlySerializedAs("DisabledAlpha")]
		private float _DisabledAlpha = 0.35f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverAlphaRendererUpdater() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string SortingLayer {
			get => _SortingLayer;
			set => this.UpdateValueWithTreeMessage(ref _SortingLayer, value, "SortingLayer");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float MasterAlpha {
			get => _MasterAlpha;
			set => this.UpdateValueWithTreeMessage(ref _MasterAlpha, value, "MasterAlpha");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float EnabledAlpha {
			get => _EnabledAlpha;
			set => this.UpdateValueWithTreeMessage(ref _EnabledAlpha, value, "EnabledAlpha");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float DisabledAlpha {
			get => _DisabledAlpha;
			set => this.UpdateValueWithTreeMessage(ref _DisabledAlpha, value, "DisabledAlpha");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			HoverRenderer hoverRend = GetComponent<HoverRenderer>();
			int childRendCount = hoverRend.GetChildRendererCount();
			int childFillCount = hoverRend.GetChildFillCount();
			float currAlpha = MasterAlpha*(hoverRend.IsEnabled ? EnabledAlpha : DisabledAlpha);

			for ( int i = 0 ; i < childRendCount ; i++ ) {
				UpdateChildRenderer(hoverRend.GetChildRenderer(i));
			}

			for ( int i = 0 ; i < childFillCount ; i++ ) {
				UpdateChildFill(hoverRend.GetChildFill(i), currAlpha);
			}

			UpdateChildCanvas(hoverRend.GetCanvas(), currAlpha);
			Controllers.TryExpireControllers();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateChildRenderer(HoverRenderer pChildRend) {
			HoverAlphaRendererUpdater rendUp = pChildRend.GetComponent<HoverAlphaRendererUpdater>();

			if ( rendUp == null ) {
				return;
			}

			rendUp.Controllers.Set(SortingLayerName, this);
			rendUp.Controllers.Set(MasterAlphaName, this);
			rendUp.Controllers.Set(EnabledAlphaName, this);
			rendUp.Controllers.Set(DisabledAlphaName, this);

			rendUp.SortingLayer = SortingLayer;
			rendUp.MasterAlpha = MasterAlpha;
			rendUp.EnabledAlpha = EnabledAlpha;
			rendUp.DisabledAlpha = DisabledAlpha;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateChildFill(HoverFill pChildFill, float pAlpha) {
			HoverAlphaFillUpdater fillUp = pChildFill.GetComponent<HoverAlphaFillUpdater>();

			if ( fillUp == null ) {
				return;
			}

			fillUp.Controllers.Set(HoverAlphaFillUpdater.SortingLayerName, this);
			fillUp.Controllers.Set(HoverAlphaFillUpdater.AlphaName, this);

			fillUp.SortingLayer = SortingLayer;
			fillUp.Alpha = pAlpha;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateChildCanvas(HoverCanvas pChildCanvas, float pAlpha) {
			if ( pChildCanvas == null ) {
				return;
			}

			pChildCanvas.Controllers.Set(SettingsControllerMap.CanvasSortingLayer, this);
			pChildCanvas.Controllers.Set(SettingsControllerMap.CanvasGroupAlpha, this);

			pChildCanvas.CanvasComponent.sortingLayerName = SortingLayer;
			pChildCanvas.CanvasGroupComponent.alpha = pAlpha;
		}

	}

}
