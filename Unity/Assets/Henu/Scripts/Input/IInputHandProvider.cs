namespace Henu.Input {

	/*================================================================================================*/
	public interface IInputHandProvider {

		bool IsLeft { get; }
		IInputHand Hand { get; }

		IInputPoint IndexPoint { get; }
		IInputPoint MiddlePoint { get; }
		IInputPoint RingPoint { get; }
		IInputPoint PinkyPoint { get; }

	}

}
