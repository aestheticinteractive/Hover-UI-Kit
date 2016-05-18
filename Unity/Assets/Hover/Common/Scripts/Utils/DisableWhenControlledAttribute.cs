using System;
using UnityEngine;

namespace Hover.Common.Utils {

	/*================================================================================================*/
	[AttributeUsage(AttributeTargets.Field)]
	public class DisableWhenControlledAttribute : PropertyAttribute {

		public string ControllerMapName { get; private set; }
		public bool DisplayMessage { get; set; }
		public float RangeMin { get; set; }
		public float RangeMax { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DisableWhenControlledAttribute(string pControllerMapName="Controllers") {
			ControllerMapName = pControllerMapName;
		}

	}

}
