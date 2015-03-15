namespace Hover.Engines {

	/*================================================================================================*/
	public interface IQuaternion {

		float W { get; set; }
		float X { get; set; }
		float Y { get; set; }
		float Z { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IQuaternion Multiply(IQuaternion pQuaternion);

	}

}
