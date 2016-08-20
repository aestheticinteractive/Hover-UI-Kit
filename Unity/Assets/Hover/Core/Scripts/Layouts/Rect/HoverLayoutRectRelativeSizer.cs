using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Layouts.Rect {

	/*================================================================================================*/
	public class HoverLayoutRectRelativeSizer : MonoBehaviour, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(RangeMin=0, RangeMax=10)]
		public float RelativeSizeX = 1;

		[DisableWhenControlled(RangeMin=0, RangeMax=10)]
		public float RelativeSizeY = 1;
		
		[DisableWhenControlled(RangeMin=-2, RangeMax=2)]
		public float RelativePositionOffsetX = 0;

		[DisableWhenControlled(RangeMin=-2, RangeMax=2)]
		public float RelativePositionOffsetY = 0;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverLayoutRectRelativeSizer() {
			Controllers = new SettingsControllerMap();
		}

	}

}
