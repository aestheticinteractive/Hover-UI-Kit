using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverRendererSlider))]
	public abstract class HoverRendererSliderUpdater : MonoBehaviour, 
																ITreeUpdateable, ISettingsController {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract SliderUtil.SliderInfo FillShapeRelatedSliderInfo();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public abstract void TreeUpdate();

	}

}
