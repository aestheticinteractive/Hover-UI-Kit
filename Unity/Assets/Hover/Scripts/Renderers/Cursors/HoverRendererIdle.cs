using System;
using Hover.Items;
using Hover.Renderers.Contents;
using Hover.Renderers.Shapes;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Cursors {

	/*================================================================================================*/
	public class HoverRendererIdle : HoverRenderer {

		public const string CenterPositionName = "CenterPosition";
		public const string DistanceThresholdName = "DistanceThreshold";
		public const string TimerProgressName = "TimerProgress";

		[DisableWhenControlled]
		public HoverFillIdle Fill;

		[DisableWhenControlled]
		public Vector3 CenterPosition;

		[DisableWhenControlled]
		public float DistanceThreshold;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float TimerProgress;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float TimerVisibleAfterProgress = 0.333f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildFillCount() {
			return 1;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverFill GetChildFill(int pIndex) {
			switch ( pIndex ) {
				case 0:
					return Fill;
			}

			throw new ArgumentOutOfRangeException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildRendererCount() {
			return 0;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverRenderer GetChildRenderer(int pIndex) {
			throw new ArgumentOutOfRangeException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverCanvas GetCanvas() {
			return null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverCanvasDataUpdater GetCanvasDataUpdater() {
			return null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetCenterWorldPosition() {
			return transform.position;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			return GetComponent<HoverShape>().GetNearestWorldPosition(pFromWorldPosition);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast) {
			return GetComponent<HoverShape>().GetNearestWorldPosition(pFromWorldRay, out pRaycast);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			UpdateIndicator();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateIndicator() {
			if ( !Application.isPlaying ) {
				return;
			}

			HoverIndicator idleInd = GetComponent<HoverIndicator>();
			float prog = Mathf.InverseLerp(TimerVisibleAfterProgress, 1, TimerProgress);

			idleInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
			idleInd.HighlightProgress = prog;
		}

	}

}
