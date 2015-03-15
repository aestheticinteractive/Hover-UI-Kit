namespace Hover.Engines {

	/*================================================================================================*/
	public interface IVector2 {

		float X { get; set; }
		float Y { get; set; }

		IVector3 Normalized { get; }
		float Magnitude { get; }
		float SquareMagnitude { get; }

	}

}
