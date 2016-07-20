using Hover.Items;
using Hover.Layouts.Arc;
using Hover.Layouts.Rect;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Shapes.Arc {

	/*================================================================================================*/
	public class HoverShapeArc : HoverShape, IArcLayoutable, IRectLayoutable {
		
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcDegreesName = "ArcDegrees";

		[DisableWhenControlled(RangeMin=0)]
		public float OuterRadius = 0.1f;

		[DisableWhenControlled(RangeMin=0)]
		public float InnerRadius = 0.04f;

		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		public float ArcDegrees = 60;

		private Plane vWorldPlane;
		private float vPrevOuter;
		private float vPrevInner;
		private float vPrevDegrees;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetCenterWorldPosition() {
			if ( ArcDegrees >= 360 ) {
				return gameObject.transform.position;
			}

			var midLocalPos = new Vector3((OuterRadius+InnerRadius)/2, 0, 0);
			return gameObject.transform.TransformPoint(midLocalPos);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			return RendererUtil.GetNearestWorldPositionOnArc(pFromWorldPosition, 
				gameObject.transform, OuterRadius, InnerRadius, ArcDegrees);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast) {
			pRaycast.WorldPosition = 
				RendererUtil.GetNearestWorldPositionOnPlane(pFromWorldRay, vWorldPlane);
			pRaycast.WorldRotation = transform.rotation;
			pRaycast.WorldPlane = vWorldPlane;
			return GetNearestWorldPosition(pRaycast.WorldPosition);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override float GetSliderValueViaNearestWorldPosition(Vector3 pNearestWorldPosition, 
										Transform pSliderContainerTx, HoverShape pHandleButtonShape) {
			HoverShapeArc buttonShapeArc = (pHandleButtonShape as HoverShapeArc);

			if ( buttonShapeArc == null ) {
				Debug.LogError("Expected slider handle to have a '"+typeof(HoverShapeArc).Name+
					"' component attached to it.", this);
				return 0;
			}

			Vector3 nearLocalPos = pSliderContainerTx.InverseTransformPoint(pNearestWorldPosition);
			float fromAngle;
			Vector3 fromAxis;
			Quaternion fromLocalRot = Quaternion.FromToRotation(Vector3.right, nearLocalPos.normalized);

			fromLocalRot.ToAngleAxis(out fromAngle, out fromAxis);
			fromAngle *= Mathf.Sign(nearLocalPos.y);

			float halfTrackAngle = (ArcDegrees-buttonShapeArc.ArcDegrees)/2;
			return Mathf.InverseLerp(-halfTrackAngle, halfTrackAngle, fromAngle);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetArcLayout(float pOuterRadius, float pInnerRadius, 
													float pArcAngle, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);
			Controllers.Set(InnerRadiusName, pController);
			Controllers.Set(ArcDegreesName, pController);

			OuterRadius = pOuterRadius;
			InnerRadius = pInnerRadius;
			ArcDegrees = pArcAngle;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetRectLayout(float pSizeX, float pSizeY, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);
			OuterRadius = Mathf.Min(pSizeX, pSizeY)/2;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			vWorldPlane = RendererUtil.GetWorldPlane(gameObject.transform);

			DidSettingsChange = (
				DidSettingsChange ||
				OuterRadius != vPrevOuter ||
				InnerRadius != vPrevInner || 
				ArcDegrees != vPrevDegrees
			);

			UpdateShapeArcChildren();

			vPrevOuter = OuterRadius;
			vPrevInner = InnerRadius;
			vPrevDegrees = ArcDegrees;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateShapeArcChildren() {
			if ( !ControlChildShapes ) {
				return;
			}

			TreeUpdater tree = GetComponent<TreeUpdater>();

			for ( int i = 0 ; i < tree.TreeChildrenThisFrame.Count ; i++ ) {
				TreeUpdater child = tree.TreeChildrenThisFrame[i];
				HoverShapeArc childArc = child.GetComponent<HoverShapeArc>();

				if ( childArc == null ) {
					continue;
				}

				childArc.Controllers.Set(OuterRadiusName, this);
				childArc.Controllers.Set(InnerRadiusName, this);
				childArc.Controllers.Set(ArcDegreesName, this);

				childArc.OuterRadius = OuterRadius;
				childArc.InnerRadius = InnerRadius;
				childArc.ArcDegrees = ArcDegrees;
			}
		}

	}

}
