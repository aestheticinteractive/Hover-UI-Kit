using Hover.Common.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Common.Layouts.Rect {

	/*================================================================================================*/
	public class HoverLayoutRectRelativeSizer : MonoBehaviour, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }

		[FormerlySerializedAs("_RelativeSizeX")]
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float RelativeSizeX = 1;

		[FormerlySerializedAs("_RelativeSizeY")]
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float RelativeSizeY = 1;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverLayoutRectRelativeSizer() {
			Controllers = new SettingsControllerMap();
		}

	}

}
