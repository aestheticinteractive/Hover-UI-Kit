using Hover.Items;
using UnityEngine;

namespace Hover.Interfaces.Key {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemData))]
	public class HoverkeyItemLabels : MonoBehaviour {

		public KeyCode DefaultKey;
		public string DefaultLabel;
		public bool HasShiftLabel;
		public string ShiftLabel;

	}

}
