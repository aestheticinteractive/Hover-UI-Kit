using System;
using System.Collections.ObjectModel;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Common.State {

	/*================================================================================================*/
	public class BaseItemPointsState : IBaseItemPointsState {

		//TODO: OPTIMIZE: renderers can also provide a center point with max radius

		public ReadOnlyCollection<Vector3> Points { get; set; }
		public Transform RelativeToTransform { get; set; }

		private readonly ReadList<Vector3> vWorldPoints;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public BaseItemPointsState() {
			vWorldPoints = new ReadList<Vector3>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ReadOnlyCollection<Vector3> GetWorldPoints() {
			if ( RelativeToTransform == null ) {
				throw new Exception("Transform must be set.");
			}

			vWorldPoints.Clear();

			for ( int i = 0 ; i < Points.Count ; i++ ) {
				vWorldPoints.Add(RelativeToTransform.TransformPoint(Points[i]));
			}

			return vWorldPoints.ReadOnly;
		}

	}

}
