using Hover.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Layouts.Arc {

	/*================================================================================================*/
	public class HoverLayoutArcRelativeSizer : MonoBehaviour, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(RangeMin=0, RangeMax=10)]
		public float RelativeThickness = 1;

		[DisableWhenControlled(RangeMin=0, RangeMax=10)]
		[FormerlySerializedAs("RelativeArcAngle")]
		public float RelativeArcDegrees = 1;
		
		[DisableWhenControlled(RangeMin=-2, RangeMax=2)]
		public float RelativeRadiusOffset = 0;

		[DisableWhenControlled(RangeMin=-2, RangeMax=2)]
		[FormerlySerializedAs("RelativeStartAngleOffset")]
		public float RelativeStartDegreeOffset = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverLayoutArcRelativeSizer() {
			Controllers = new SettingsControllerMap();
		}

	}

}
