using Hover.Items;
using Hover.Layouts.Arc;
using Hover.Renderers.Packs.Alpha.Arc;
using Hover.Renderers.Utils;
using Hover.Utils;

namespace Hover.Renderers.Shapes.Arc {

	/*================================================================================================*/
	public class HoverRendererArcController : HoverRendererController, IArcLayoutable {
	
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcAngleName = "ArcAngle";
		
		[DisableWhenControlled(RangeMin=0)]
		public float OuterRadius = 0.1f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float InnerRadius = 0.04f;

		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		public float ArcAngle = 60;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IRendererArcButton ButtonArcRenderer {
			get { return (_ButtonRenderer as IRendererArcButton); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public IRendererArcSlider SliderArcRenderer {
			get { return (_SliderRenderer as IRendererArcSlider); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override string DefaultButtonPrefabResourcePath {
			get { return "Prefabs/HoverAlphaRendererArcButton-Default"; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override string DefaultSliderPrefabResourcePath {
			get { return "Prefabs/HoverAlphaRendererArcSlider-Default"; }
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetArcLayout(float pOuterRadius, float pInnerRadius, 
													float pArcAngle, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);
			Controllers.Set(InnerRadiusName, pController);
			Controllers.Set(ArcAngleName, pController);

			OuterRadius = pOuterRadius;
			InnerRadius = pInnerRadius;
			ArcAngle = pArcAngle;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override IRendererButton FindOrBuildButton() {
			return RendererUtil.FindOrBuildRenderer<IRendererArcButton>(
				gameObject.transform, ButtonRendererPrefab, "Button", 
				typeof(HoverAlphaRendererArcButton));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected override IRendererSlider FindOrBuildSlider() {
			return RendererUtil.FindOrBuildRenderer<HoverAlphaRendererArcSlider>(
				gameObject.transform, SliderRendererPrefab, "Slider", 
				typeof(HoverAlphaRendererArcSlider));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateButtonSettings(HoverItem pHoverItem) {
			base.UpdateButtonSettings(pHoverItem);
			
			ButtonArcRenderer.OuterRadius = OuterRadius;
			ButtonArcRenderer.InnerRadius = InnerRadius;
			ButtonArcRenderer.ArcAngle = ArcAngle;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateSliderSettings(HoverItem pHoverItem) {
			base.UpdateSliderSettings(pHoverItem);
			
			SliderArcRenderer.OuterRadius = OuterRadius;
			SliderArcRenderer.InnerRadius = InnerRadius;
			SliderArcRenderer.ArcAngle = ArcAngle;
		}
		
	}

}
