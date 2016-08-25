using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Layouts.Arc {

	/*================================================================================================*/
	public class HoverLayoutArcRelativeSizer : MonoBehaviour, ISettingsController {

		[Range(0, 10)]
		public float RelativeThickness = 1;

		[Range(0, 10)]
		public float RelativeArcDegrees = 1;

		[Range(-2, 2)]
		public float RelativeRadiusOffset = 0;

		[Range(-2, 2)]
		public float RelativeStartDegreeOffset = 0;

	}

}
