using System;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Shapes.Rect {

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

		[DisableWhenControlled]
		public SizeType OuterSizeType = SizeType.Max;

		[DisableWhenControlled]
		public float UvTop = 0;

		[DisableWhenControlled]
		public float UvBottom = 1;

		[DisableWhenControlled]
		public float UvLeft = 0;

		[DisableWhenControlled]
		public float UvRight = 1;

		private SizeType vPrevOuterType;
		private float vPrevUvTop;
		private float vPrevUvBottom;
		private float vPrevUvLeft;
		private float vPrevUvRight;


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
				UvTop != vPrevUvTop ||
				UvBottom != vPrevUvBottom ||
				UvLeft != vPrevUvLeft ||
				UvRight != vPrevUvRight
			);

			vPrevOuterType = OuterSizeType;
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

			MeshUtil.BuildRectangleMesh(vMeshBuild, shape.SizeX, shape.SizeY, outerProg);

			for ( int i = 0 ; i < vMeshBuild.Uvs.Length ; i++ ) {
				Vector2 uv = vMeshBuild.Uvs[i];
				uv.x = Mathf.Lerp(UvLeft, UvRight, uv.x);
				uv.y = Mathf.Lerp(UvTop, UvBottom, uv.y);
				vMeshBuild.Uvs[i] = uv;
				vMeshBuild.Colors[i] = Color.white;
			}

			UpdateMeshUvAndColors();
			vMeshBuild.Commit();
			vMeshBuild.CommitColors();
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void UpdateMeshUvAndColors() {
			for ( int i = 0 ; i < vMeshBuild.Uvs.Length ; i++ ) {
				Vector2 uv = vMeshBuild.Uvs[i];
				uv.x = Mathf.Lerp(UvLeft, UvRight, uv.x);
				uv.y = Mathf.Lerp(UvTop, UvBottom, uv.y);
				vMeshBuild.Uvs[i] = uv;
				vMeshBuild.Colors[i] = Color.white;
			}
		}

	}

}
