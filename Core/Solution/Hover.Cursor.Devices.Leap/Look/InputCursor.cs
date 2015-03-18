using Hover.Common.Input;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.Devices.Leap.Look {

	/*================================================================================================*/
	public class LeapLookInputCursor : IInputCursor {

		public CursorType Type { get; private set; }
		public bool IsAvailable { get; private set; }

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Size { get; private set; }

		private readonly InputSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LeapLookInputCursor(InputSettings pSettings) {
			Type = CursorType.LeftIndex;
			vSettings = pSettings;
		}

		//TODO: this input module requires information about the selection planes

	}

}
