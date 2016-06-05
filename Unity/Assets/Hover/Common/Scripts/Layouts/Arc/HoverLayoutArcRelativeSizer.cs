using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Layouts.Arc {

	/*================================================================================================*/
	public class HoverLayoutArcRelativeSizer : MonoBehaviour, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float RelativeThickness = 1;

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float RelativeArcAngle = 1;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverLayoutArcRelativeSizer() {
			Controllers = new SettingsControllerMap();
		}

	}

}
