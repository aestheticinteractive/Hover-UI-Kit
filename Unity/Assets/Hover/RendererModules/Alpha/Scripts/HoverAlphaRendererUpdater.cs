using Hover.Renderers;
using Hover.Renderers.Contents;
using Hover.Renderers.Elements;
using Hover.Utils;
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

		[DisableWhenControlled(DisplayMessage=true)]
		public string SortingLayer = "Default";

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float MasterAlpha = 1;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float EnabledAlpha = 1;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float DisabledAlpha = 0.35f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HoverRenderer hoverRend = GetComponent<HoverRenderer>();
			int fillCount = hoverRend.GetChildFillCount();
			float currAlpha = MasterAlpha*(hoverRend.IsEnabled ? EnabledAlpha : DisabledAlpha);

			for ( int i = 0 ; i < fillCount ; i++ ) {
				UpdateChildFill(hoverRend.GetChildFill(i), currAlpha);
			}

			UpdateChildCanvas(hoverRend.GetCanvas(), currAlpha);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
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

			pChildCanvas.Controllers.Set("canvas.sortingLayer", this);
			pChildCanvas.Controllers.Set("canvasGroup.alpha", this);

			pChildCanvas.CanvasComponent.sortingLayerName = SortingLayer;
			pChildCanvas.CanvasGroupComponent.alpha = pAlpha;
		}

	}

}
