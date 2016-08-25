using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Layouts.Rect {

	/*================================================================================================*/
	public class HoverLayoutRectRelativeSizer : MonoBehaviour, ISettingsController {

		[Range(0, 10)]
		public float RelativeSizeX = 1;

		[Range(0, 10)]
		public float RelativeSizeY = 1;
		
		[Range(-2, 2)]
		public float RelativePositionOffsetX = 0;

		[Range(-2, 2)]
		public float RelativePositionOffsetY = 0;

	}

}
