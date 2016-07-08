using System;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Shapes.Arc {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverMeshArc : HoverMesh {

		public enum RadiusType {
			Min,
			Selection,
			Highlight,
			Max
		}

		public const string UvInnerRadiusName = "UvInnerRadius";
		public const string UvOuterRadiusName = "UvOuterRadius";
		public const string UvMinArcDegreeName = "UvMinArcDegree";
		public const string UvMaxArcDegreeName = "UvMaxArcDegree";

		[DisableWhenControlled(RangeMin=0.05f, RangeMax=10)]
		public float ArcSegmentsPerDegree = 0.5f;

		[DisableWhenControlled]
		public RadiusType InnerRadiusType = RadiusType.Min;

		[DisableWhenControlled]
		public RadiusType OuterRadiusType = RadiusType.Max;

		//TODO: add settings/script for automatic/dynamic control of UV values

		[DisableWhenControlled]
		public float UvInnerRadius = 0;

		[DisableWhenControlled]
		public float UvOuterRadius = 1;

		[DisableWhenControlled]
		public float UvMinArcDegree = 0;

		[DisableWhenControlled]
		public float UvMaxArcDegree = 0;

		private int vArcSteps;
		private float vPrevArcSegs;
		private RadiusType vPrevInnerType;
		private RadiusType vPrevOuterType;
		private float vPrevUvInner;
		private float vPrevUvOuter;
		private float vPrevUvMinDeg;
		private float vPrevUvMaxDeg;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool ShouldUpdateMesh() {
			var ind = GetComponent<HoverIndicator>();
			var shape = GetComponent<HoverShapeArc>();

			vArcSteps = (int)Mathf.Max(2, shape.ArcDegrees*ArcSegmentsPerDegree);

			bool shouldUpdate = (
				base.ShouldUpdateMesh() ||
				ind.DidSettingsChange ||
				shape.DidSettingsChange ||
				ArcSegmentsPerDegree != vPrevArcSegs ||
				InnerRadiusType != vPrevInnerType ||
				OuterRadiusType != vPrevOuterType ||
				UvInnerRadius != vPrevUvInner ||
				UvOuterRadius != vPrevUvOuter ||
				UvMinArcDegree != vPrevUvMinDeg ||
				UvMaxArcDegree != vPrevUvMaxDeg
			);

			vPrevArcSegs = ArcSegmentsPerDegree;
			vPrevInnerType = InnerRadiusType;
			vPrevOuterType = OuterRadiusType;
			vPrevUvInner = UvInnerRadius;
			vPrevUvOuter = UvOuterRadius;
			vPrevUvMinDeg = UvMinArcDegree;
			vPrevUvMaxDeg = UvMaxArcDegree;

			return shouldUpdate;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected int GetArcMeshSteps() {
			return vArcSteps;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected float GetRadiusProgress(RadiusType pType) {
			HoverIndicator ind = GetComponent<HoverIndicator>();

			switch ( pType ) {
				case RadiusType.Min: return 0;
				case RadiusType.Selection: return ind.SelectionProgress;
				case RadiusType.Highlight: return ind.HighlightProgress;
				case RadiusType.Max: return 1;
			}

			throw new Exception("Unexpected type: "+pType);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh() {
			HoverShapeArc shape = GetComponent<HoverShapeArc>();
			float innerRadProg = GetRadiusProgress(InnerRadiusType);
			float outerRadProg = GetRadiusProgress(OuterRadiusType);
			float innerRad = Mathf.Lerp(shape.InnerRadius, shape.OuterRadius, innerRadProg);
			float outerRad = Mathf.Lerp(shape.InnerRadius, shape.OuterRadius, outerRadProg);
			float halfRadians = shape.ArcDegrees/180*Mathf.PI/2;
			int steps = GetArcMeshSteps();

			MeshUtil.BuildRingMesh(vMeshBuild, innerRad, outerRad, -halfRadians, halfRadians, steps);

			for ( int i = 0 ; i < vMeshBuild.Uvs.Length ; i++ ) {
				int stepI = i/2;
				float arcProg = (float)stepI/steps;
				bool isInner = (i%2 == 0);

				Vector2 uv = vMeshBuild.Uvs[i];
				uv.x = Mathf.Lerp(UvMinArcDegree, UvMaxArcDegree, arcProg);
				uv.y = (isInner ? UvInnerRadius : UvOuterRadius);
				vMeshBuild.Uvs[i] = uv;
				vMeshBuild.Colors[i] = Color.white;
			}

			vMeshBuild.Commit();
			vMeshBuild.CommitColors();
		}

	}

}
