using Hover.Items;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	public class HoverShapeRect : HoverShape {
		
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";

		[DisableWhenControlled(RangeMin=0, DisplayMessage=true)]
		public float SizeX = 0.1f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float SizeY = 0.1f;

		private Plane vWorldPlane;
		private float vPrevSizeX;
		private float vPrevSizeY;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			return RendererUtil.GetNearestWorldPositionOnRectangle(
				pFromWorldPosition, gameObject.transform, SizeX, SizeY);
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
				SizeX != vPrevSizeX ||
				SizeY != vPrevSizeY
			);

			UpdateShapeRectChildren();

			vPrevSizeX = SizeX;
			vPrevSizeY = SizeY;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateShapeRectChildren() {
			TreeUpdater tree = GetComponent<TreeUpdater>();

			for ( int i = 0 ; i < tree.TreeChildrenThisFrame.Count ; i++ ) {
				TreeUpdater child = tree.TreeChildrenThisFrame[i];
				HoverShapeRect childRect = child.GetComponent<HoverShapeRect>();

				if ( childRect == null ) {
					continue;
				}

				childRect.Controllers.Set(SizeXName, this);
				childRect.Controllers.Set(SizeYName, this);

				childRect.SizeX = SizeX;
				childRect.SizeY = SizeY;
			}
		}

	}

}
