using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Items.Sliders {

	/*================================================================================================*/
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverRendererSlider))]
	public abstract class HoverRendererSliderUpdater : MonoBehaviour, 
																ITreeUpdateable, ISettingsController {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public abstract void TreeUpdate();

	}

}
