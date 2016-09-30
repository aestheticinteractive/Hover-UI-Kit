using Hover.Core.Renderers.Cursors;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Shapes.Arc {

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
			HoverRendererIdle rend = GetComponent<HoverRendererIdle>();
			float thickness = rend.DistanceThreshold/gameObject.transform.lossyScale.x;

			shape.Controllers.Set(HoverShapeArc.InnerRadiusName, this);
			shape.Controllers.Set(HoverShapeArc.InnerOffsetName, this);

			shape.InnerRadius = shape.OuterRadius+thickness;
			shape.InnerOffset = rend.CenterOffset;
		}

	}

}
