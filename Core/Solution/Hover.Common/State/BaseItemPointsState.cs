using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hover.Common.State {

	/*================================================================================================*/
	public class BaseItemPointsState : IBaseItemPointsState {

		//TODO: OPTIMIZE: renderers can also provide a center point with max radius

		public Vector3[] Points { get; set; }
		public Transform RelativeToTransform { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3[] GetWorldPoints() {
			if ( RelativeToTransform == null ) {
				throw new Exception("Transform must be set.");
			}

			var worldList = new List<Vector3>();

			foreach ( Vector3 local in Points ) {
				worldList.Add(RelativeToTransform.TransformPoint(local));
			}

			return worldList.ToArray();
		}

	}

}
