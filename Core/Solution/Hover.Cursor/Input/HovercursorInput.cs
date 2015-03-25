using System.Collections.Generic;
using System.Linq;
using Hover.Common.Input;
using UnityEngine;

namespace Hover.Cursor.Input {

	/*================================================================================================*/
	public abstract class HovercursorInput : MonoBehaviour, IHovercursorInput {

		public bool IsEnabled { get; set; }
		public bool IsFailure { get; set; }

		protected readonly IDictionary<string, InputPlane> vInputPlaneMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HovercursorInput() {
			IsEnabled = true;
			vInputPlaneMap = new Dictionary<string, InputPlane>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void UpdateInput();

		/*--------------------------------------------------------------------------------------------*/
		public abstract IInputCursor GetCursor(CursorType pType);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddOrUpdatePlane(string pId, Vector3 pPointWorld, Vector3 pNormalWorld) {
			vInputPlaneMap[pId] = new InputPlane {
				Id = pId,
				PointWorld = pPointWorld,
				NormalWorld = pNormalWorld
			};
		}

		/*--------------------------------------------------------------------------------------------* /
		public Transform GetPlane(string pId) {
			InputPlane plane;
			vInputPlaneMap.TryGetValue(pId, out plane);
			return (plane == null ? null : plane.Center);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetNearestPlaneId() {
			InputPlane nearest = vInputPlaneMap.Values.FirstOrDefault(x => x.IsNearest);
			return (nearest == null ? null : nearest.Id);
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool RemovePlane(string pId) {
			if ( !vInputPlaneMap.ContainsKey(pId) ) {
				return false;
			}

			return vInputPlaneMap.Remove(pId);
		}

	}

}


