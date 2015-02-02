namespace Hovercast.Core.Input {

	/*================================================================================================*/
	public interface IInputSide {

		bool IsLeft { get; }

		IInputMenu Menu { get; }
		IInputCursor Cursor { get; }

	}

}
