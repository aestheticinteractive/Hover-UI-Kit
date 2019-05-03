using System;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Shapes.Rect {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShapeRect))]
	public class HoverMeshRect : HoverMesh {

		public enum SizeType {
			Min,
			Selection,
			Highlight,
			Max
		}

		public const string UvTopName = "UvTop";
		public const string UvBottomName = "UvBottom";
		public const string UvLeftName = "UvLeft";
		public const string UvRightName = "UvRight";

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("OuterSizeType")]
		private SizeType _OuterSizeType = SizeType.Max;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("AutoUvViaSizeType")]
		private bool _AutoUvViaSizeType = false;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("UvTop")]
		private float _UvTop = 0;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("UvBottom")]
		private float _UvBottom = 1;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("UvLeft")]
		private float _UvLeft = 0;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("UvRight")]
		private float _UvRight = 1;

		private SizeType vPrevOuterType;
		private bool vPrevAutoUv;
		private float vPrevUvTop;
		private float vPrevUvBottom;
		private float vPrevUvLeft;
		private float vPrevUvRight;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SizeType OuterSizeType {
			get => _OuterSizeType;
			set => this.UpdateValueWithTreeMessage(ref _OuterSizeType, value, "OuterSizeType");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool AutoUvViaSizeType {
			get => _AutoUvViaSizeType;
			set => this.UpdateValueWithTreeMessage(ref _AutoUvViaSizeType, value, "AutoUvViaSizeType");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float UvTop {
			get => _UvTop;
			set => this.UpdateValueWithTreeMessage(ref _UvTop, value, "UvTop");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float UvBottom {
			get => _UvBottom;
			set => this.UpdateValueWithTreeMessage(ref _UvBottom, value, "UvBottom");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float UvLeft {
			get => _UvLeft;
			set => this.UpdateValueWithTreeMessage(ref _UvLeft, value, "UvLeft");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float UvRight {
			get => _UvRight;
			set => this.UpdateValueWithTreeMessage(ref _UvRight, value, "UvRight");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override bool IsMeshVisible {
			get {
				HoverShapeRect shape = GetComponent<HoverShapeRect>();
				float outerProg = GetDimensionProgress(OuterSizeType);
				return (shape.SizeX != 0 && shape.SizeY != 0 && outerProg != 0);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool ShouldUpdateMesh() {
			var ind = GetComponent<HoverIndicator>();
			var shape = GetComponent<HoverShapeRect>();

			bool shouldUpdate = (
				base.ShouldUpdateMesh() ||
				ind.DidSettingsChange ||
				shape.DidSettingsChange ||
				OuterSizeType != vPrevOuterType ||
				AutoUvViaSizeType != vPrevAutoUv ||
				UvTop != vPrevUvTop ||
				UvBottom != vPrevUvBottom ||
				UvLeft != vPrevUvLeft ||
				UvRight != vPrevUvRight
			);

			vPrevOuterType = OuterSizeType;
			vPrevAutoUv = AutoUvViaSizeType;
			vPrevUvTop = UvTop;
			vPrevUvBottom = UvBottom;
			vPrevUvLeft = UvLeft;
			vPrevUvRight = UvRight;

			return shouldUpdate;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected float GetDimensionProgress(SizeType pType) {
			HoverIndicator ind = GetComponent<HoverIndicator>();

			switch ( pType ) {
				case SizeType.Min: return 0;
				case SizeType.Selection: return ind.SelectionProgress;
				case SizeType.Highlight: return ind.HighlightProgress;
				case SizeType.Max: return 1;
			}

			throw new Exception("Unexpected type: "+pType);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh() {
			HoverShapeRect shape = GetComponent<HoverShapeRect>();
			float outerProg = GetDimensionProgress(OuterSizeType);
			float outerW;
			float outerH;

			if ( shape.SizeX >= shape.SizeY ) {
				outerH = shape.SizeY*outerProg;
				outerW = shape.SizeX-(shape.SizeY-outerH);
			}
			else {
				outerW = shape.SizeX*outerProg;
				outerH = shape.SizeY-(shape.SizeX-outerW);
			}

			MeshUtil.BuildQuadMesh(vMeshBuild, outerW, outerH);

			UpdateAutoUv(shape, outerW, outerH);
			UpdateMeshUvAndColors();
			vMeshBuild.Commit();
			vMeshBuild.CommitColors();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void UpdateAutoUv(HoverShapeRect pShapeRect, float pOuterW, float pOuterH) {
			if ( !AutoUvViaSizeType ) {
				return;
			}

			Controllers.Set(UvTopName, this);
			Controllers.Set(UvBottomName, this);
			Controllers.Set(UvLeftName, this);
			Controllers.Set(UvRightName, this);

			UvTop = Mathf.Lerp(0.5f, 0, pOuterH/pShapeRect.SizeY);
			UvBottom = 1-UvTop;
			UvLeft = Mathf.Lerp(0.5f, 0, pOuterW/pShapeRect.SizeX);
			UvRight = 1-UvLeft;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void UpdateMeshUvAndColors() {
			for ( int i = 0 ; i < vMeshBuild.Uvs.Length ; i++ ) {
				Vector2 uv = vMeshBuild.Uvs[i];
				uv.x = Mathf.LerpUnclamped(UvLeft, UvRight, uv.x);
				uv.y = Mathf.LerpUnclamped(UvTop, UvBottom, uv.y);
				vMeshBuild.Uvs[i] = uv;
				vMeshBuild.Colors[i] = Color.white;
			}
		}

	}

}
