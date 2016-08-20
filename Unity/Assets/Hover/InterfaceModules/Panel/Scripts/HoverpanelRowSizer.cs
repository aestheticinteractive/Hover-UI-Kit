using Hover.Layouts.Rect;
using Hover.Renderers.Shapes.Rect;
using Hover.Utils;
using UnityEngine;

namespace Hover.Interfaces.Panel {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverShapeRect))]
	[RequireComponent(typeof(HoverpanelInterface))]
	public class HoverpanelRowSizer : MonoBehaviour, ITreeUpdateable, ISettingsController {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HoverpanelInterface panel = GetComponent<HoverpanelInterface>();
			HoverShapeRect shape = GetComponent<HoverShapeRect>();

			UpdateRow(panel.ActiveRow, shape);
			UpdateRow(panel.PreviousRow, shape);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRow(HoverLayoutRectRow pRow, HoverShapeRect pShape) {
			if ( pRow == null ) {
				return;
			}

			pRow.Controllers.Set(HoverLayoutRectRow.SizeXName, this);
			pRow.Controllers.Set(HoverLayoutRectRow.SizeYName, this);

			pRow.SizeX = pShape.SizeX;
			pRow.SizeY = pShape.SizeY;
		}

	}

}
