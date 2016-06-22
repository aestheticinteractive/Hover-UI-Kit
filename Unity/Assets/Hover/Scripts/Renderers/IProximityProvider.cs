using Hover.Cursors;
using Hover.Items;
using UnityEngine;

namespace Hover.Renderers {

	/*================================================================================================*/
	public interface IProximityProvider {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast);

	}

}
