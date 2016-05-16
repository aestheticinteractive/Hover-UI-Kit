using UnityEngine;

namespace Hover.Common.Renderers.Helpers {

	/*================================================================================================*/
	public interface IProximityProvider {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

	}

}
