using Hover.Items;
using Hover.Layouts.Rect;
using Hover.Renderers.Packs.Alpha.Rect;
using Hover.Renderers.Utils;
using Hover.Utils;

namespace Hover.Renderers.Shapes.Rect {

	/*================================================================================================*/
	public class HoverRendererRectController : HoverRendererController, IRectLayoutable {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";

		[DisableWhenControlled(RangeMin=0)]
		public float SizeX = 0.1f;

		[DisableWhenControlled(RangeMin=0)]
		public float SizeY = 0.1f;


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
		public void SetRectLayout(float pSizeX, float pSizeY, ISettingsController pController) {
			Controllers.Set(SizeXName, pController);
			Controllers.Set(SizeYName, pController);

			SizeX = pSizeX;
			SizeY = pSizeY;
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
