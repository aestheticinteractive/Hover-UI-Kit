using UnityEngine;

namespace Hover.Common.Renderers.Shared.Bases {

	/*================================================================================================*/
	public interface IProximityProvider {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

	}

}
