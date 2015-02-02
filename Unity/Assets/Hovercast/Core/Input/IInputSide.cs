namespace Hovercast.Core.Input {

	/*================================================================================================*/
	public interface IInputSide {

		bool IsLeft { get; }
		bool IsActive { get; }

		IInputMenu Menu { get; }
		IInputCursor Cursor { get; }

	}

}
