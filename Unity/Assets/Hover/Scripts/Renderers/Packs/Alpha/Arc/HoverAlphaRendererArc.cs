using Hover.Items;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Arc {

	/*================================================================================================*/
	public abstract class HoverAlphaRendererArc : HoverAlphaRenderer {
	
		public const string OuterRadiusName = "_OuterRadius";
		public const string InnerRadiusName = "_InnerRadius";
		public const string ArcAngleName = "_ArcAngle";

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		private float _OuterRadius = 0.1f;
		
		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		private float _InnerRadius = 0.04f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		private float _ArcAngle = 60;

		private Plane vWorldPlane;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float OuterRadius {
			get { return _OuterRadius; }
			set { _OuterRadius = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float InnerRadius {
			get { return _InnerRadius; }
			set { _InnerRadius = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float ArcAngle {
			get { return _ArcAngle; }
			set { _ArcAngle = value; }
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
