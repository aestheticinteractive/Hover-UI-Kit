using System;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverFillArcButton : HoverFill {

		public enum EdgePositionType {
			Inner,
			Outer
		}

		[DisableWhenControlled(DisplayMessage=true)]
		public HoverMeshArc Background;

		[DisableWhenControlled]
		public HoverMeshArc Highlight;

		[DisableWhenControlled]
		public HoverMeshArc Selection;

		[DisableWhenControlled]
		public HoverMeshArc Edge;
		
		[DisableWhenControlled]
		public bool ShowEdge = true;

		[DisableWhenControlled(RangeMin=0)]
		public float EdgeThickness = 0.001f;

		[DisableWhenControlled]
		public EdgePositionType EdgePosition = EdgePositionType.Inner;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildMeshCount() {
			return 4;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMesh GetChildMesh(int pIndex) {
			switch ( pIndex ) {
				case 0: return Background;
				case 1: return Highlight;
				case 2: return Selection;
				case 3: return Edge;
			}

			throw new ArgumentOutOfRangeException();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			UpdateMeshes();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshes() {
			HoverIndicator indicator = gameObject.GetComponent<HoverIndicator>();
			HoverShapeArc shape = gameObject.GetComponent<HoverShapeArc>();

			bool isOuterEdge = (EdgePosition == EdgePositionType.Outer);
			float inset = EdgeThickness*Mathf.Sign(shape.OuterRadius-shape.InnerRadius);
			float insetOuterRadius = shape.OuterRadius - (isOuterEdge ? inset : 0);
			float insetInnerRadius = shape.InnerRadius + (isOuterEdge ? 0 : inset);
			float edgeOuterRadius = (isOuterEdge ? shape.OuterRadius : insetInnerRadius);
			float edgeInnerRadius = (isOuterEdge ? insetOuterRadius : shape.InnerRadius);

			if ( Background != null ) {
				UpdateMeshIndicator(Background, indicator);
				UpdateMeshShape(Background, shape, insetOuterRadius, insetInnerRadius);
				UpdateMesh(Background, true);
			}
			
			if ( Highlight != null ) {
				UpdateMeshIndicator(Highlight, indicator);
				UpdateMeshShape(Highlight, shape, insetOuterRadius, insetInnerRadius);
				UpdateMesh(Highlight, true);
			}
			
			if ( Selection != null ) {
				UpdateMeshIndicator(Selection, indicator);
				UpdateMeshShape(Selection, shape, insetOuterRadius, insetInnerRadius);
				UpdateMesh(Selection, true);
			}
			
			if ( Edge != null ) {
				UpdateMeshIndicator(Edge, indicator);
				UpdateMeshShape(Edge, shape, edgeOuterRadius, edgeInnerRadius);
				UpdateMesh(Edge, ShowEdge);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshIndicator(HoverMeshArc pMesh, HoverIndicator pFillIndicator) {
			HoverIndicator meshInd = pMesh.GetComponent<HoverIndicator>();

			meshInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
			meshInd.Controllers.Set(HoverIndicator.SelectionProgressName, this);
			
			meshInd.HighlightProgress = pFillIndicator.HighlightProgress;
			meshInd.SelectionProgress = pFillIndicator.SelectionProgress;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshShape(HoverMeshArc pMesh, HoverShapeArc pFillShape, 
															float pOuterRadius, float pInnerRadius) {
			HoverShapeArc meshShape = pMesh.GetComponent<HoverShapeArc>();

			meshShape.Controllers.Set(HoverShapeArc.OuterRadiusName, this);
			meshShape.Controllers.Set(HoverShapeArc.InnerRadiusName, this);
			meshShape.Controllers.Set(HoverShapeArc.ArcDegreesName, this);
			
			meshShape.OuterRadius = pOuterRadius;
			meshShape.InnerRadius = pInnerRadius;
			meshShape.ArcDegrees = pFillShape.ArcDegrees;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMesh(HoverMeshArc pMesh, bool pShowMesh) {
			pMesh.Controllers.Set("GameObject.activeSelf", this);

			RendererUtil.SetActiveWithUpdate(pMesh, (pShowMesh /*&& pMesh.IsMeshVisible*/));
		}

	}

}
