using Hovercast.Core.Input;

namespace Hovercast.Devices.Test {

	/*================================================================================================*/
	public class TestInputSide : IInputSide {

		public bool IsLeft { get; private set; }
		public bool IsActive { get; private set; }
		public bool IsCursor { get; set; }

		public IInputMenu Menu { get; private set; }
		public IInputCursor Cursor { get; private set; }

		// menu   | ( 0.121, 0.118, -0.020) / ( 0.100, 0.826, -0.030,  0.554)
		// cursor | (-0.077, 0.147, -0.066) / (-0.112, 0.827, -0.254, -0.490)


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestInputSide(bool pIsLeft, TestInputMenu pMenu) {
			IsLeft = pIsLeft;
			Menu = pMenu;
		}

	}

}
