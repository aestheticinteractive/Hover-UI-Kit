using Hover.Core.Renderers.Items.Buttons;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Shapes.Arc {

	/*================================================================================================*/
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverFillButton))]
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverFillButtonArcUpdater : TreeUpdateableBehavior, ISettingsController {

		public enum EdgePositionType {
			Inner,
			Outer
		}

		[SerializeField]
		[FormerlySerializedAs("EdgeThickness")]
		private float _EdgeThickness = 0.001f;

		[SerializeField]
		[FormerlySerializedAs("EdgePosition")]
		private EdgePositionType _EdgePosition = EdgePositionType.Inner;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float EdgeThickness {
			get => _EdgeThickness;
			set => this.UpdateValueWithTreeMessage(ref _EdgeThickness, value, "EdgeThickness");
		}

		/*--------------------------------------------------------------------------------------------*/
		public EdgePositionType EdgePosition {
			get => _EdgePosition;
			set => this.UpdateValueWithTreeMessage(ref _EdgePosition, value, "EdgePosition");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
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
			float arcDegrees = shapeArc.ArcDegrees;

			if ( fillButton.Background != null ) {
				UpdateMeshShape(fillButton.Background, insetOuterRadius, insetInnerRadius, arcDegrees);
			}

			if ( fillButton.Highlight != null ) {
				UpdateMeshShape(fillButton.Highlight, insetOuterRadius, insetInnerRadius, arcDegrees);
			}

			if ( fillButton.Selection != null ) {
				UpdateMeshShape(fillButton.Selection, insetOuterRadius, insetInnerRadius, arcDegrees);
			}

			if ( fillButton.Edge != null ) {
				UpdateMeshShape(fillButton.Edge, 
					edgeOuterRadius, edgeInnerRadius, arcDegrees, fillButton.ShowEdge);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshShape(HoverMesh pMesh, float pOuterRad, float pInnerRad,
															float pArcDegrees, bool pShowMesh=true) {
			HoverShapeArc meshShape = pMesh.GetComponent<HoverShapeArc>();

			pMesh.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			meshShape.Controllers.Set(HoverShapeArc.OuterRadiusName, this);
			meshShape.Controllers.Set(HoverShapeArc.InnerRadiusName, this);
			meshShape.Controllers.Set(HoverShapeArc.ArcDegreesName, this);

			RendererUtil.SetActiveWithUpdate(pMesh, (pShowMesh && pMesh.IsMeshVisible));
			meshShape.OuterRadius = pOuterRad;
			meshShape.InnerRadius = pInnerRad;
			meshShape.ArcDegrees = pArcDegrees;
		}

	}

}
