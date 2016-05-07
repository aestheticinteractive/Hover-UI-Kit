using UnityEngine;

namespace Hover.Common.Renderers {

	/*================================================================================================*/
	public interface IProximityProvider {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

	}

}
