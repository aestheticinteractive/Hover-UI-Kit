using Hover.Core.Items;
using UnityEngine;

namespace Hover.InterfaceModules.Key {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemData))]
	public class HoverkeyItemLabels : MonoBehaviour {

		public enum KeyActionType {
			Character,
			Control,
			Navigation,
			Other
		}

		public KeyActionType ActionType;
		public KeyCode DefaultKey;
		public string DefaultLabel;
		public bool HasShiftLabel;
		public string ShiftLabel;

	}

}
