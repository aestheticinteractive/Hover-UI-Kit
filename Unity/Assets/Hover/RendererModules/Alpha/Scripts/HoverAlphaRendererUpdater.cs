using Hover.Core.Renderers;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverRenderer))]
	public class HoverAlphaRendererUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public const string SortingLayerName = "SortingLayer";
		public const string MasterAlphaName = "MasterAlpha";
		public const string EnabledAlphaName = "EnabledAlpha";
		public const string DisabledAlphaName = "DisabledAlpha";

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(DisplaySpecials=true)]
		public string SortingLayer = "Default";

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float MasterAlpha = 1;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float EnabledAlpha = 1;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float DisabledAlpha = 0.35f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverAlphaRendererUpdater() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
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
