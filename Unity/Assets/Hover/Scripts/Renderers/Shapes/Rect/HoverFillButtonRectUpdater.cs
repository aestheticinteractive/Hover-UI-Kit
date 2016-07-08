using Hover.Renderers.Buttons;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Shapes.Rect {

	/*================================================================================================*/
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverFillButton))]
	[RequireComponent(typeof(HoverShapeRect))]
	public class HoverFillButtonRectUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public float EdgeThickness = 0.001f;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			EdgeThickness = Mathf.Max(0, EdgeThickness);
			UpdateMeshes();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMeshes() {
			HoverFillButton fillButton = gameObject.GetComponent<HoverFillButton>();
			HoverShapeRect shapeRect = gameObject.GetComponent<HoverShapeRect>();
		
			float totalEdgeThick = EdgeThickness*2;
			float insetSizeX = Mathf.Max(0, shapeRect.SizeX-totalEdgeThick);
			float insetSizeY = Mathf.Max(0, shapeRect.SizeY-totalEdgeThick);
		
			if ( fillButton.Background != null ) {
				UpdateMeshShape(fillButton.Background, insetSizeX, insetSizeY);
			}

			if ( fillButton.Highlight != null ) {
				UpdateMeshShape(fillButton.Highlight, insetSizeX, insetSizeY);
			}

			if ( fillButton.Selection != null ) {
				UpdateMeshShape(fillButton.Selection, insetSizeX, insetSizeY);
			}

			if ( fillButton.Edge != null ) {
				HoverIndicator meshInd = fillButton.Edge.GetComponent<HoverIndicator>();
				float minSize = Mathf.Min(shapeRect.SizeX, shapeRect.SizeY);

				UpdateMeshShape(fillButton.Edge, shapeRect.SizeX, shapeRect.SizeY);

				meshInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
				meshInd.HighlightProgress = 1-totalEdgeThick/minSize; //TODO:  hack/workaround
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshShape(HoverMesh pMesh, float pSizeX, float pSizeY) {
			HoverShapeRect meshShape = pMesh.GetComponent<HoverShapeRect>();

			meshShape.Controllers.Set(HoverShapeRect.SizeXName, this);
			meshShape.Controllers.Set(HoverShapeRect.SizeYName, this);

			meshShape.SizeX = pSizeX;
			meshShape.SizeY = pSizeY;
		}

	}

}
