using Hover.Common.Items;
using Hover.Common.Layouts;
using Hover.Common.Layouts.Rect;
using Hover.Common.Renderers.Packs.Alpha.Rect;
using Hover.Common.Renderers.Utils;
using Hover.Common.Utils;

namespace Hover.Common.Renderers.Shapes.Rect {

	/*================================================================================================*/
	public class HoverRendererRectController : HoverRendererController, IRectLayoutable {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeX = 10;

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeY = 10;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IRendererRectButton ButtonRectRenderer {
			get { return (_ButtonRenderer as IRendererRectButton); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public IRendererRectSlider SliderRectRenderer {
			get { return (_SliderRenderer as IRendererRectSlider); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override string DefaultButtonPrefabResourcePath {
			get { return "Prefabs/HoverAlphaRendererRectButton-Default"; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override string DefaultSliderPrefabResourcePath {
			get { return "Prefabs/HoverAlphaRendererRectSlider-Default"; }
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetLayoutSize(float pSizeX, float pSizeY, ISettingsController pController) {
			Controllers.Set(SizeXName, pController);
			Controllers.Set(SizeYName, pController);

			SizeX = pSizeX;
			SizeY = pSizeY;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UnsetLayoutSize(ISettingsController pController) {
			Controllers.Unset(SizeXName, pController);
			Controllers.Unset(SizeYName, pController);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override IRendererButton FindOrBuildButton() {
			return RendererUtil.FindOrBuildRenderer<IRendererRectButton>(
				gameObject.transform, ButtonRendererPrefab, "Button", 
				typeof(HoverAlphaRendererRectButton));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected override IRendererSlider FindOrBuildSlider() {
			return RendererUtil.FindOrBuildRenderer<HoverAlphaRendererRectSlider>(
				gameObject.transform, SliderRendererPrefab, "Slider", 
				typeof(HoverAlphaRendererRectSlider));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateButtonSettings(HoverItem pHoverItem) {
			base.UpdateButtonSettings(pHoverItem);
			
			ButtonRectRenderer.SizeX = SizeX;
			ButtonRectRenderer.SizeY = SizeY;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateSliderSettings(HoverItem pHoverItem) {
			base.UpdateSliderSettings(pHoverItem);
			
			SliderRectRenderer.SizeX = SizeX;
			SliderRectRenderer.SizeY = SizeY;
		}
		
	}

}
