using System;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Renderers.Shapes.Arc;
using Hover.Core.Renderers.Shapes.Rect;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.CanvasElements {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverCanvas))]
	public class HoverCanvasSizeUpdater : TreeUpdateableBehavior, ISettingsController {

		[SerializeField]
		[FormerlySerializedAs("Shape")]
		private HoverShape _Shape;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverShape Shape {
			get => _Shape;
			set => this.UpdateValueWithTreeMessage(ref _Shape, value, "Shape");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			if ( Shape == null ) {
				Debug.LogWarning("No '"+typeof(HoverShape).Name+"' reference provided.", this);
				return;
			}

			HoverCanvas canvas = GetComponent<HoverCanvas>();
			HoverShapeRect shapeRect = (Shape as HoverShapeRect);
			HoverShapeArc shapeArc = (Shape as HoverShapeArc);

			if ( shapeRect ) {
				UpdateWithRect(canvas, shapeRect);
			}
			else if ( shapeArc != null ) {
				UpdateWithArc(canvas, shapeArc);
			}
			else {
				throw new Exception("Shape not supported: "+Shape);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithRect(HoverCanvas pHoverCanvas, HoverShapeRect pShapeRect) {
			pHoverCanvas.Controllers.Set(HoverCanvas.SizeXName, this);
			pHoverCanvas.Controllers.Set(HoverCanvas.SizeYName, this);

			pHoverCanvas.SizeX = pShapeRect.SizeX;
			pHoverCanvas.SizeY = pShapeRect.SizeY;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithArc(HoverCanvas pHoverCanvas, HoverShapeArc pShapeArc) {
			pHoverCanvas.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".x", this);
			pHoverCanvas.Controllers.Set(HoverCanvas.SizeXName, this);

			pHoverCanvas.SizeX = pShapeArc.OuterRadius-pShapeArc.InnerRadius;

			Vector3 canvasLocalPos = pHoverCanvas.transform.localPosition;
			canvasLocalPos.x = pShapeArc.InnerRadius+pHoverCanvas.SizeX/2;
			pHoverCanvas.transform.localPosition = canvasLocalPos;
		}

	}

}
