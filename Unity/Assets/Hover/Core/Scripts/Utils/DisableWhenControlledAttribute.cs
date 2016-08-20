using System;
using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	[AttributeUsage(AttributeTargets.Field)]
	public class DisableWhenControlledAttribute : PropertyAttribute {

		public const float NullRangeMin = float.MaxValue;
		public const float NullRangeMax = float.MinValue;

		public string ControllerMapName { get; private set; }
		public bool DisplaySpecials { get; set; }
		public float RangeMin { get; set; }
		public float RangeMax { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DisableWhenControlledAttribute(string pControllerMapName="Controllers") {
			ControllerMapName = pControllerMapName;
			RangeMin = NullRangeMin;
			RangeMax = NullRangeMax;
		}

	}

}
