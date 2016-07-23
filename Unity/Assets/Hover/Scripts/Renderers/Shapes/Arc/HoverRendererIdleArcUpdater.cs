using Hover.Renderers.Cursors;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Shapes.Arc {

	/*================================================================================================*/
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverShapeArc))]
	[RequireComponent(typeof(HoverRendererIdle))]
	public class HoverRendererIdleArcUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HoverShapeArc shape = GetComponent<HoverShapeArc>();
			shape.Controllers.Set(HoverShapeArc.OuterOffsetName, this);
			shape.OuterOffset = GetComponent<HoverRendererIdle>().CenterPosition;
		}

	}

}
