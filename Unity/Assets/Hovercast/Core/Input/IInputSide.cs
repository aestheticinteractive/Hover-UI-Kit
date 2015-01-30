namespace Hovercast.Core.Input {

	/*================================================================================================*/
	public interface IInputSide {

		bool IsLeft { get; }
		IInputCenter Center { get; }

		IInputPoint[] Points { get; }
		IInputPoint IndexPoint { get; }
		IInputPoint MiddlePoint { get; }
		IInputPoint RingPoint { get; }
		IInputPoint PinkyPoint { get; }

	}

}
