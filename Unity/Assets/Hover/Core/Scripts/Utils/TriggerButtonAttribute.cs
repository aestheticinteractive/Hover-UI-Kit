using System;
using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	[AttributeUsage(AttributeTargets.Field)]
	public class TriggerButtonAttribute : PropertyAttribute {

		public string ButtonLabel { get; private set; }
		public string OnSelectedMethodName { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TriggerButtonAttribute(string pButtonLabel, 
										string pOnSelectedMethodName="OnEditorTriggerButtonSelected") {
			ButtonLabel = pButtonLabel;
			OnSelectedMethodName = pOnSelectedMethodName;
		}

	}

}
