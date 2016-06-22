using Hover.Items;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Rect {

	/*================================================================================================*/
	public abstract class HoverAlphaRendererRect : HoverAlphaRenderer {
	
		public const string SizeXName = "_SizeX";
		public const string SizeYName = "_SizeY";

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		private float _SizeX = 0.1f;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		private float _SizeY = 0.1f;
		
		[DisableWhenControlled]
		public AnchorTypeWithCustom Anchor = AnchorTypeWithCustom.MiddleCenter;

		private Plane vWorldPlane;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float SizeX {
			get { return _SizeX; }
			set { _SizeX = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SizeY {
			get { return _SizeY; }
			set { _SizeY = value; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			vWorldPlane = RendererUtil.GetWorldPlane(gameObject.transform);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast) {
			pRaycast.WorldPosition = 
				RendererUtil.GetNearestWorldPositionOnPlane(pFromWorldRay, vWorldPlane);
			pRaycast.WorldRotation = transform.rotation;
			pRaycast.WorldPlane = vWorldPlane;
			return GetNearestWorldPosition(pRaycast.WorldPosition);
		}

	}

}
