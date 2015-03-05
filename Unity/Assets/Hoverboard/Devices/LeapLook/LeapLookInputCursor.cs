using Hoverboard.Core.Input;
using UnityEngine;

namespace Hoverboard.Devices.LeapLook {

	/*================================================================================================*/
	public class LeapLookInputCursor : IInputCursor {

		public CursorType Type { get; private set; }
		public bool IsAvailable { get; private set; }

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Size { get; private set; }

		private readonly LeapLookInputSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LeapLookInputCursor(LeapLookInputSettings pSettings) {
			Type = CursorType.PrimaryLeft;
			vSettings = pSettings;
		}

		//TODO: this input module requires information about the selection planes

	}

}
