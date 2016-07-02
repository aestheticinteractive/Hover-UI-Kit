using System;
using Hover.Renderers.Contents;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverCanvas))]
	public class HoverCanvasSizeUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public HoverShape Shape;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			if ( Shape == null ) {
				Debug.LogWarning("No '"+typeof(HoverShape).Name+"' reference provided.");
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

			float edgeThick = 0; //TODO: Fill.EdgeThickness

			pHoverCanvas.SizeX = pShapeRect.SizeX-edgeThick*2;
			pHoverCanvas.SizeY = pShapeRect.SizeY-edgeThick*2;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateWithArc(HoverCanvas pHoverCanvas, HoverShapeArc pShapeArc) {
			pHoverCanvas.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".x", this);
			pHoverCanvas.Controllers.Set(HoverCanvas.SizeXName, this);

			float edgeThick = 0; //TODO: Fill.EdgeThickness
			
			pHoverCanvas.SizeX = pShapeArc.OuterRadius-pShapeArc.InnerRadius-edgeThick;

			Vector3 canvasLocalPos = pHoverCanvas.transform.localPosition;
			canvasLocalPos.x = pShapeArc.InnerRadius+edgeThick+pHoverCanvas.SizeX/2;
			pHoverCanvas.transform.localPosition = canvasLocalPos;
		}

	}

}
