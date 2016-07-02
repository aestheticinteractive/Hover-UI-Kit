using Hover.Renderers.Elements.Buttons;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements.Shapes.Arc {

	/*================================================================================================*/
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverFillButton))]
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverFillButtonArcUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public enum EdgePositionType {
			Inner,
			Outer
		}

		public float EdgeThickness = 0.001f;
		public EdgePositionType EdgePosition = EdgePositionType.Inner;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			EdgeThickness = Mathf.Max(0, EdgeThickness);
			UpdateMeshes();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMeshes() {
			HoverFillButton fillButton = gameObject.GetComponent<HoverFillButton>();
			HoverShapeArc shapeArc = gameObject.GetComponent<HoverShapeArc>();
		
			bool isOuterEdge = (EdgePosition == EdgePositionType.Outer);
			float inset = EdgeThickness*Mathf.Sign(shapeArc.OuterRadius-shapeArc.InnerRadius);
			float insetOuterRadius = shapeArc.OuterRadius - (isOuterEdge ? inset : 0);
			float insetInnerRadius = shapeArc.InnerRadius + (isOuterEdge ? 0 : inset);
			float edgeOuterRadius = (isOuterEdge ? shapeArc.OuterRadius : insetInnerRadius);
			float edgeInnerRadius = (isOuterEdge ? insetOuterRadius : shapeArc.InnerRadius);
		
			if ( fillButton.Background != null ) {
				UpdateMeshShape(fillButton.Background, insetOuterRadius, insetInnerRadius);
			}

			if ( fillButton.Highlight != null ) {
				UpdateMeshShape(fillButton.Highlight, insetOuterRadius, insetInnerRadius);
			}

			if ( fillButton.Selection != null ) {
				UpdateMeshShape(fillButton.Selection, insetOuterRadius, insetInnerRadius);
			}

			if ( fillButton.Edge != null ) {
				UpdateMeshShape(fillButton.Edge, edgeOuterRadius, edgeInnerRadius);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshShape(HoverMesh pMesh, float pOuterRad, float pInnerRad) {
			HoverShapeArc meshShape = pMesh.GetComponent<HoverShapeArc>();

			meshShape.Controllers.Set(HoverShapeArc.OuterRadiusName, this);
			meshShape.Controllers.Set(HoverShapeArc.InnerRadiusName, this);

			meshShape.OuterRadius = pOuterRad;
			meshShape.InnerRadius = pInnerRad;
		}

	}

}
