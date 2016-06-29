using System;
using Hover.Items;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverFillArcButton : HoverFill {

		//TODO: refactor to just HoverFillButton, and handle shapes elsewhere?

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
			HoverShapeArc shape = gameObject.GetComponent<HoverShapeArc>();

			bool isOuterEdge = (EdgePosition == EdgePositionType.Outer);
			float inset = EdgeThickness*Mathf.Sign(shape.OuterRadius-shape.InnerRadius);
			float insetOuterRadius = shape.OuterRadius - (isOuterEdge ? inset : 0);
			float insetInnerRadius = shape.InnerRadius + (isOuterEdge ? 0 : inset);
			float edgeOuterRadius = (isOuterEdge ? shape.OuterRadius : insetInnerRadius);
			float edgeInnerRadius = (isOuterEdge ? insetOuterRadius : shape.InnerRadius);

			if ( Background != null ) {
				UpdateMeshShape(Background, insetOuterRadius, insetInnerRadius);
				UpdateMesh(Background, true);
			}
			
			if ( Highlight != null ) {
				UpdateMeshShape(Highlight, insetOuterRadius, insetInnerRadius);
				UpdateMesh(Highlight, true);
			}
			
			if ( Selection != null ) {
				UpdateMeshShape(Selection, insetOuterRadius, insetInnerRadius);
				UpdateMesh(Selection, true);
			}
			
			if ( Edge != null ) {
				UpdateMeshShape(Edge, edgeOuterRadius, edgeInnerRadius);
				UpdateMesh(Edge, ShowEdge);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshShape(HoverMeshArc pMesh, float pOuterRad, float pInnerRad) {
			HoverShapeArc meshShape = pMesh.GetComponent<HoverShapeArc>();

			meshShape.Controllers.Set(HoverShapeArc.OuterRadiusName, this);
			meshShape.Controllers.Set(HoverShapeArc.InnerRadiusName, this);
			
			meshShape.OuterRadius = pOuterRad;
			meshShape.InnerRadius = pInnerRad;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMesh(HoverMeshArc pMesh, bool pShowMesh) {
			pMesh.Controllers.Set("GameObject.activeSelf", this);

			//TODO: determine when mesh can be disabled (without needing a mesh update to occur first)
			RendererUtil.SetActiveWithUpdate(pMesh, (pShowMesh /*&& pMesh.IsMeshVisible*/));
		}

	}

}
