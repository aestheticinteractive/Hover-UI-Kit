using Hover.Items;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	public class HoverShapeArc : HoverShape {
		
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcDegreesName = "ArcDegrees";

		[DisableWhenControlled(RangeMin=0, DisplayMessage=true)]
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
