using System;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Items.Buttons {

	/*================================================================================================*/
	public class HoverRendererButton : HoverRenderer {

		[DisableWhenControlled]
		public HoverFillButton Fill;

		[DisableWhenControlled]
		public HoverCanvas Canvas;


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
			return Canvas;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverCanvasDataUpdater GetCanvasDataUpdater() {
			return Canvas.GetComponent<HoverCanvasDataUpdater>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetCenterWorldPosition() {
			return GetComponent<HoverShape>().GetCenterWorldPosition();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			return GetComponent<HoverShape>().GetNearestWorldPosition(pFromWorldPosition);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast) {
			return GetComponent<HoverShape>().GetNearestWorldPosition(pFromWorldRay, out pRaycast);
		}

	}

}
