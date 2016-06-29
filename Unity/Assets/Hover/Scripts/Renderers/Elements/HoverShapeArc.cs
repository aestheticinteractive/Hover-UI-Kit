using Hover.Utils;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	public class HoverShapeArc : HoverShape {
		
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcDegreesName = "ArcDegrees";

		[DisableWhenControlled(RangeMin=0, DisplayMessage=true)]
		public float OuterRadius = 0.1f;

		[DisableWhenControlled(RangeMin=0)]
		public float InnerRadius = 0.04f;

		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		public float ArcDegrees = 60;

		private float vPrevOuter;
		private float vPrevInner;
		private float vPrevDegrees;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			DidSettingsChange = (
				DidSettingsChange ||
				OuterRadius != vPrevOuter ||
				InnerRadius != vPrevInner || 
				ArcDegrees != vPrevDegrees
			);

			vPrevOuter = OuterRadius;
			vPrevInner = InnerRadius;
			vPrevDegrees = ArcDegrees;
		}

	}

}
