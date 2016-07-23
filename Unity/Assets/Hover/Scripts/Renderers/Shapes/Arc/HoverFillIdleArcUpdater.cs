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
			HoverFillIdle fill = GetComponent<HoverFillIdle>();
			HoverIndicator ind = GetComponent<HoverIndicator>();
			float timerArcDeg = Mathf.Lerp(0, 180, ind.HighlightProgress);

			if ( fill.BackgroundTop != null ) {
				UpdateMeshShape(fill.BackgroundTop, 180-timerArcDeg);
			}

			if ( fill.BackgroundBottom != null ) {
				UpdateMeshShape(fill.BackgroundBottom, 180-timerArcDeg);
			}

			if ( fill.TimerLeft != null ) {
				UpdateMeshShape(fill.TimerLeft, timerArcDeg);
			}

			if ( fill.TimerRight != null ) {
				UpdateMeshShape(fill.TimerRight, timerArcDeg);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshShape(HoverMesh pMesh, float pArcDegrees) {
			HoverShapeArc meshShape = pMesh.GetComponent<HoverShapeArc>();
			meshShape.Controllers.Set(HoverShapeArc.ArcDegreesName, this);
			meshShape.ArcDegrees = pArcDegrees;
		}

	}

}
