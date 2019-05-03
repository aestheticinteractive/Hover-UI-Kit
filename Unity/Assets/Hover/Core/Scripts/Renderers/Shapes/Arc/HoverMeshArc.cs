using System;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Shapes.Arc {

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

		[SerializeField]
		[DisableWhenControlled(RangeMin=0.05f, RangeMax=10)]
		[FormerlySerializedAs("ArcSegmentsPerDegree")]
		private float _ArcSegmentsPerDegree = 0.5f;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("InnerRadiusType")]
		private RadiusType _InnerRadiusType = RadiusType.Min;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("OuterRadiusType")]
		private RadiusType _OuterRadiusType = RadiusType.Max;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("AutoUvViaRadiusType")]
		private bool _AutoUvViaRadiusType = false;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("UvInnerRadius")]
		private float _UvInnerRadius = 0;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("UvOuterRadius")]
		private float _UvOuterRadius = 1;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("UvMinArcDegree")]
		private float _UvMinArcDegree = 1;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("UvMaxArcDegree")]
		private float _UvMaxArcDegree = 0;

		private int vArcSteps;
		private float vPrevArcSegs;
		private RadiusType vPrevInnerType;
		private RadiusType vPrevOuterType;
		private bool vPrevAutoUv;
		private float vPrevUvInner;
		private float vPrevUvOuter;
		private float vPrevUvMinDeg;
		private float vPrevUvMaxDeg;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float ArcSegmentsPerDegree {
			get => _ArcSegmentsPerDegree;
			set => this.UpdateValueWithTreeMessage(ref _ArcSegmentsPerDegree, value, "ArcSegsPerDeg");
		}

		/*--------------------------------------------------------------------------------------------*/
		public RadiusType InnerRadiusType {
			get => _InnerRadiusType;
			set => this.UpdateValueWithTreeMessage(ref _InnerRadiusType, value, "InnerRadiusType");
		}

		/*--------------------------------------------------------------------------------------------*/
		public RadiusType OuterRadiusType {
			get => _OuterRadiusType;
			set => this.UpdateValueWithTreeMessage(ref _OuterRadiusType, value, "OuterRadiusType");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool AutoUvViaRadiusType {
			get => _AutoUvViaRadiusType;
			set => this.UpdateValueWithTreeMessage(ref _AutoUvViaRadiusType, value, "AutoUvViaRadType");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float UvInnerRadius {
			get => _UvInnerRadius;
			set => this.UpdateValueWithTreeMessage(ref _UvInnerRadius, value, "UvInnerRadius");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float UvOuterRadius {
			get => _UvOuterRadius;
			set => this.UpdateValueWithTreeMessage(ref _UvOuterRadius, value, "UvOuterRadius");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float UvMinArcDegree {
			get => _UvMinArcDegree;
			set => this.UpdateValueWithTreeMessage(ref _UvMinArcDegree, value, "UvMinArcDegree");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float UvMaxArcDegree {
			get => _UvMaxArcDegree;
			set => this.UpdateValueWithTreeMessage(ref _UvMaxArcDegree, value, "UvMaxArcDegree");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override bool IsMeshVisible {
			get {
				HoverShapeArc shape = GetComponent<HoverShapeArc>();
				float innerRadProg = GetRadiusProgress(InnerRadiusType);
				float outerRadProg = GetRadiusProgress(OuterRadiusType);
				return (shape.OuterRadius != shape.InnerRadius && outerRadProg != innerRadProg);
			}
		}


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
				AutoUvViaRadiusType != vPrevAutoUv ||
				UvInnerRadius != vPrevUvInner ||
				UvOuterRadius != vPrevUvOuter ||
				UvMinArcDegree != vPrevUvMinDeg ||
				UvMaxArcDegree != vPrevUvMaxDeg
			);

			vPrevArcSegs = ArcSegmentsPerDegree;
			vPrevInnerType = InnerRadiusType;
			vPrevOuterType = OuterRadiusType;
			vPrevAutoUv = AutoUvViaRadiusType;
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
			Vector3 innerOff = Vector3.Lerp(shape.InnerOffset, shape.OuterOffset, innerRadProg);
			Vector3 outerOff = Vector3.Lerp(shape.InnerOffset, shape.OuterOffset, outerRadProg);
			float halfRadians = shape.ArcDegrees/180*Mathf.PI/2;
			int steps = GetArcMeshSteps();

			MeshUtil.BuildRingMesh(vMeshBuild, innerRad, outerRad, -halfRadians, halfRadians,
				innerOff, outerOff, steps);

			UpdateAutoUv(shape, innerRadProg, outerRadProg);
			UpdateMeshUvAndColors(steps);
			vMeshBuild.Commit();
			vMeshBuild.CommitColors();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void UpdateAutoUv(HoverShapeArc pShapeArc, float pInnerProg, float pOuterProg) {
			if ( !AutoUvViaRadiusType ) {
				return;
			}

			Controllers.Set(UvInnerRadiusName, this);
			Controllers.Set(UvOuterRadiusName, this);

			UvInnerRadius = pInnerProg;
			UvOuterRadius = pOuterProg;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void UpdateMeshUvAndColors(int pSteps) {
			for ( int i = 0 ; i < vMeshBuild.Uvs.Length ; i++ ) {
				int stepI = i/2;
				float arcProg = (float)stepI/pSteps;
				bool isInner = (i%2 == 0);

				Vector2 uv = vMeshBuild.Uvs[i];
				uv.x = Mathf.Lerp(UvMinArcDegree, UvMaxArcDegree, arcProg);
				uv.y = (isInner ? UvInnerRadius : UvOuterRadius);
				vMeshBuild.Uvs[i] = uv;
				vMeshBuild.Colors[i] = Color.white;
			}
		}

	}

}
