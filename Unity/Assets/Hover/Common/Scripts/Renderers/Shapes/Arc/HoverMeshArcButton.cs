using Hover.Common.Utils;

namespace Hover.Common.Renderers.Shapes.Arc {

	/*================================================================================================*/
	public abstract class HoverMeshArcButton : HoverMesh {
	
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcAngleName = "ArcAngle";
		public const string OuterAmountName = "OuterAmount";
		public const string InnerAmountName = "InnerAmount";
		public const string UseUvRelativeToSizeName = "UseUvRelativeToSize";

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float OuterRadius = 10;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float InnerRadius = 3;

		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		public float ArcAngle = 60;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float OuterAmount = 1;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float InnerAmount = 0.5f;
		
		[DisableWhenControlled]
		public bool UseUvRelativeToSize = false;

		private float vPrevOuterRadius;
		private float vPrevInnerRadius;
		private float vPrevArcAngle;
		private float vPrevInner;
		private float vPrevOuter;
		private bool vPrevUseUv;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool ShouldUpdateMesh() {
			bool shouldUpdate = (
				base.ShouldUpdateMesh() ||
				OuterRadius != vPrevOuterRadius ||
				InnerRadius != vPrevInnerRadius ||
				ArcAngle != vPrevArcAngle ||
				InnerAmount != vPrevInner ||
				OuterAmount != vPrevOuter ||
				UseUvRelativeToSize != vPrevUseUv
			);

			vPrevOuterRadius = OuterRadius;
			vPrevInnerRadius = InnerRadius;
			vPrevArcAngle = ArcAngle;
			vPrevInner = InnerAmount;
			vPrevOuter = OuterAmount;
			vPrevUseUv = UseUvRelativeToSize;

			return shouldUpdate;
		}
		
	}

}
