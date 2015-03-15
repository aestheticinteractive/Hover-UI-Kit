namespace Hover.Engines {

	/*================================================================================================*/
	public interface ITransform {

		IVector3 LocalPosition { get; set; }
		IVector3 WorldPosition { get; set; }
		IQuaternion LocalRotation { get; set; }
		IQuaternion WorldRotation { get; set; }

		ITransform Parent { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IVector3 GetLocalPoint(ITransform pOtherTransform, IVector3 pOtherLocalPoint);

	}

}
