using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Shapes.Arc {

	/*================================================================================================*/
	public abstract class HoverMeshArc : HoverMesh {
	
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcAngleName = "ArcAngle";
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float OuterRadius = 10;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float InnerRadius = 3;

		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		public float ArcAngle = 60;

		[DisableWhenControlled(RangeMin=0.1f, RangeMax=10)]
		public float ArcSegmentsPerDegree = 0.5f;
		
		private float vPrevOuterRadius;
		private float vPrevInnerRadius;
		private float vPrevArcAngle;
		private float vPrevArcSegs;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool ShouldUpdateMesh() {
			bool shouldUpdate = (
				base.ShouldUpdateMesh() ||
				OuterRadius != vPrevOuterRadius ||
				InnerRadius != vPrevInnerRadius ||
				ArcAngle != vPrevArcAngle ||
				ArcSegmentsPerDegree != vPrevArcSegs
			);
			
			vPrevOuterRadius = OuterRadius;
			vPrevInnerRadius = InnerRadius;
			vPrevArcAngle = ArcAngle;
			vPrevArcSegs = ArcSegmentsPerDegree;

			return shouldUpdate;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected int GetArcMeshSteps() {
			return (int)Mathf.Max(2, ArcAngle*ArcSegmentsPerDegree);
		}
		
	}

}
