using System;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Cursors {

	/*================================================================================================*/
	public class HoverRendererCursor : HoverRenderer {

		public const string IsRaycastName = "IsRaycast";
		public const string ShowRaycastLineName = "ShowRaycastLine";
		public const string RaycastWorldOriginName = "RaycastWorldOrigin";

		[DisableWhenControlled]
		public HoverFillCursor Fill;

		[DisableWhenControlled]
		public HoverRaycastLine RaycastLine;

		[DisableWhenControlled]
		public bool IsRaycast;

		[DisableWhenControlled]
		public bool ShowRaycastLine;

		[DisableWhenControlled]
		public Vector3 RaycastWorldOrigin;

		[DisableWhenControlled]
		public float RaycastOffsetZ = -0.001f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildFillCount() {
			return 1;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverFill GetChildFill(int pIndex) {
			switch ( pIndex ) {
				case 0: return Fill;
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
			return Vector3.zero;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast) {
			pRaycast = new RaycastResult();
			return Vector3.zero;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			UpdatePosition();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdatePosition() {
			if ( RaycastLine != null ) {
				RaycastLine.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
				RaycastLine.Controllers.Set(HoverRaycastLine.RaycastWorldOriginName, this);

				RaycastLine.RaycastWorldOrigin = RaycastWorldOrigin;
				RendererUtil.SetActiveWithUpdate(RaycastLine, (IsRaycast && ShowRaycastLine));
			}

			if ( !Application.isPlaying || !IsRaycast ) {
				return;
			}

			Controllers.Set(SettingsControllerMap.TransformLocalPosition+".z", this);

			Vector3 localPos = transform.localPosition;
			localPos.z = RaycastOffsetZ/transform.lossyScale.z;
			transform.localPosition = localPos;
		}

	}

}
