using UnityEngine;

namespace Hover.Common.State {

	/*================================================================================================*/
	public interface IBaseItemPointsState {

		Vector3[] Points { get; set; }
		Transform RelativeToTransform { get; set; }

	}

}
