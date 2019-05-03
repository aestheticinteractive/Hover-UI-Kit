using System;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Items.Buttons {

	/*================================================================================================*/
	public class HoverRendererButton : HoverRenderer {

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("Fill")]
		private HoverFillButton _Fill;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("Canvas")]
		private HoverCanvas _Canvas;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverFillButton Fill {
			get => _Fill;
			set => this.UpdateValueWithTreeMessage(ref _Fill, value, "Fill");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverCanvas Canvas {
			get => _Canvas;
			set => this.UpdateValueWithTreeMessage(ref _Canvas, value, "Canvas");
		}


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
