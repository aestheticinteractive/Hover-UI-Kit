using Hover.Renderers.Cursors;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Shapes.Arc {

	/*================================================================================================*/
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverFillIdle))]
	[RequireComponent(typeof(HoverShapeArc))]
	public class HoverFillIdleArcUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			UpdateMeshes();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMeshes() {
			HoverFillIdle fillIdle = GetComponent<HoverFillIdle>();
			HoverShapeArc shapeArc = GetComponent<HoverShapeArc>();
			HoverIndicator indic = GetComponent<HoverIndicator>();
			float timerArcDeg = Mathf.Lerp(180, 0, indic.HighlightProgress);

			if ( fillIdle.BackgroundTop != null ) {
				UpdateMeshShape(fillIdle.BackgroundTop, 180-timerArcDeg, 90,
					shapeArc.OuterRadius, shapeArc.InnerRadius);
			}

			if ( fillIdle.BackgroundBottom != null ) {
				UpdateMeshShape(fillIdle.BackgroundBottom, 180-timerArcDeg, -90,
					shapeArc.OuterRadius, shapeArc.InnerRadius);
			}

			if ( fillIdle.TimerLeft != null ) {
				UpdateMeshShape(fillIdle.TimerLeft, timerArcDeg, 0,
					shapeArc.OuterRadius, shapeArc.InnerRadius);
			}

			if ( fillIdle.TimerRight != null ) {
				UpdateMeshShape(fillIdle.TimerRight, timerArcDeg, 180,
					shapeArc.OuterRadius, shapeArc.InnerRadius);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshShape(HoverMesh pMesh, float pArcDegrees, float pRotateZ,
																	float pOuterRad, float pInnerRad) {
			HoverShapeArc meshShape = pMesh.GetComponent<HoverShapeArc>();

			meshShape.Controllers.Set(HoverShapeArc.ArcDegreesName, this);
			meshShape.Controllers.Set(HoverShapeArc.OuterRadiusName, this);
			meshShape.Controllers.Set(HoverShapeArc.InnerRadiusName, this);
			meshShape.Controllers.Set(SettingsControllerMap.TransformLocalRotation, this);

			meshShape.ArcDegrees = pArcDegrees;
			meshShape.OuterRadius = pOuterRad;
			meshShape.InnerRadius = pInnerRad;
			pMesh.transform.localRotation = Quaternion.Euler(0, 0, pRotateZ);
		}

	}

}
