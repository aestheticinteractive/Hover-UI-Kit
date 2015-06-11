using System.Collections.ObjectModel;
using UnityEngine;

namespace Hover.Common.State {

	/*================================================================================================*/
	public interface IBaseItemPointsState {

		ReadOnlyCollection<Vector3> Points { get; set; }
		Transform RelativeToTransform { get; set; }

	}

}
