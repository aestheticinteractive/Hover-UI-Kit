using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers {

	/*================================================================================================*/
	public interface IProximityProvider {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast);

	}

}
