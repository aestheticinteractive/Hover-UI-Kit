using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Items.Sliders {

	/*================================================================================================*/
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverRendererSlider))]
	public abstract class HoverRendererSliderUpdater : TreeUpdateableBehavior, ISettingsController {

	}

}
