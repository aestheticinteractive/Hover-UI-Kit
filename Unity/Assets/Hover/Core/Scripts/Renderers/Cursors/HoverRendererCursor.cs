using System;
using Hover.Core.Items;
using Hover.Core.Renderers.Contents;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Cursors {

	/*================================================================================================*/
	public class HoverRendererCursor : HoverRenderer {

		public const string IsRaycastName = "IsRaycast";

		[DisableWhenControlled]
		public HoverFillCursor Fill;

		[DisableWhenControlled]
		public bool IsRaycast;

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
