using Hover.Core.Layouts.Arc;
using Hover.Core.Layouts.Rect;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Shapes.Arc {

	/*================================================================================================*/
	public class HoverShapeArc : HoverShape, ILayoutableArc, ILayoutableRect {

		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcDegreesName = "ArcDegrees";
		public const string OuterOffsetName = "OuterOffset";
		public const string InnerOffsetName = "InnerOffset";

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		[FormerlySerializedAs("OuterRadius")]
		private float _OuterRadius = 0.1f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		[FormerlySerializedAs("InnerRadius")]
		private float _InnerRadius = 0.04f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		[FormerlySerializedAs("ArcDegrees")]
		private float _ArcDegrees = 60;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("OuterOffset")]
		private Vector3 _OuterOffset = Vector3.zero;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("InnerOffset")]
		private Vector3 _InnerOffset = Vector3.zero;

		private Plane vWorldPlane;
		private Vector3 vPrevWorldPos;
		private Quaternion vPrevWorldRot;
		private float vPrevOuterRad;
		private float vPrevInnerRad;
		private float vPrevDegrees;
		private Vector3 vPrevOuterOff;
		private Vector3 vPrevInnerOff;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float OuterRadius {
			get => _OuterRadius;
			set => this.UpdateValueWithTreeMessage(ref _OuterRadius, value, "OuterRadius");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float InnerRadius {
			get => _InnerRadius;
			set => this.UpdateValueWithTreeMessage(ref _InnerRadius, value, "InnerRadius");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float ArcDegrees {
			get => _ArcDegrees;
			set => this.UpdateValueWithTreeMessage(ref _ArcDegrees, value, "ArcDegrees");
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 OuterOffset {
			get => _OuterOffset;
			set => this.UpdateValueWithTreeMessage(ref _OuterOffset, value, "OuterOffset");
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 InnerOffset {
			get => _InnerOffset;
			set => this.UpdateValueWithTreeMessage(ref _InnerOffset, value, "InnerOffset");
		}


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
			pRaycast = new RaycastResult();

			Vector3? nearWorldPos =
				RendererUtil.GetNearestWorldPositionOnPlane(pFromWorldRay, vWorldPlane);

			if ( nearWorldPos == null ) {
				return pFromWorldRay.origin;
			}

			pRaycast.IsHit = true;
			pRaycast.WorldPosition = nearWorldPos.Value;
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
			float fromDeg;
			Vector3 fromAxis;
			Quaternion fromLocalRot = Quaternion.FromToRotation(Vector3.right, nearLocalPos.normalized);

			fromLocalRot.ToAngleAxis(out fromDeg, out fromAxis);
			fromDeg *= Mathf.Sign(nearLocalPos.y);

			float halfTrackDeg = (ArcDegrees-buttonShapeArc.ArcDegrees)/2;
			return Mathf.InverseLerp(-halfTrackDeg, halfTrackDeg, fromDeg);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetArcLayout(float pOuterRadius, float pInnerRadius, 
												float pArcDegrees, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);
			Controllers.Set(InnerRadiusName, pController);
			Controllers.Set(ArcDegreesName, pController);

			OuterRadius = pOuterRadius;
			InnerRadius = pInnerRadius;
			ArcDegrees = pArcDegrees;
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

			if ( transform.position != vPrevWorldPos || transform.rotation != vPrevWorldRot ) {
				vWorldPlane = RendererUtil.GetWorldPlane(gameObject.transform);
			}

			vPrevWorldPos = transform.position;
			vPrevWorldRot = transform.rotation;

			DidSettingsChange = (
				DidSettingsChange ||
				OuterRadius != vPrevOuterRad ||
				InnerRadius != vPrevInnerRad || 
				ArcDegrees != vPrevDegrees ||
				OuterOffset != vPrevOuterOff ||
				InnerOffset != vPrevInnerOff
			);

			UpdateShapeArcChildren();

			vPrevOuterRad = OuterRadius;
			vPrevInnerRad = InnerRadius;
			vPrevDegrees = ArcDegrees;
			vPrevOuterOff = OuterOffset;
			vPrevInnerOff = InnerOffset;
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

				Quaternion offsetRot = Quaternion.Inverse(childArc.transform.localRotation);

				childArc.Controllers.Set(OuterRadiusName, this);
				childArc.Controllers.Set(InnerRadiusName, this);
				childArc.Controllers.Set(ArcDegreesName, this);
				childArc.Controllers.Set(OuterOffsetName, this);
				childArc.Controllers.Set(InnerOffsetName, this);

				childArc.OuterRadius = OuterRadius;
				childArc.InnerRadius = InnerRadius;
				childArc.ArcDegrees = ArcDegrees;
				childArc.OuterOffset = offsetRot*OuterOffset;
				childArc.InnerOffset = offsetRot*InnerOffset;
			}
		}

	}

}
