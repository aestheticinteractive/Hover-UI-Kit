using System;
using System.Collections.Generic;
using Hover.Common.Input;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cursor.Input {

	/*================================================================================================*/
	public abstract class HovercursorInput : MonoBehaviour, IHovercursorInput {

		public bool IsEnabled { get; set; }
		public bool IsFailure { get; set; }

		protected readonly IDictionary<string, InteractionPlaneState> vPlaneMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HovercursorInput() {
			IsEnabled = true;
			vPlaneMap = new Dictionary<string, InteractionPlaneState>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void UpdateInput();

		/*--------------------------------------------------------------------------------------------*/
		public abstract IInputCursor GetCursor(CursorType pType);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddPlane(string pId, InteractionPlaneState pPlane) {
			if ( vPlaneMap.ContainsKey(pId) ) {
				throw new Exception("There is already a plane with key '"+pId+"'.");
			}

			vPlaneMap[pId] = pPlane;
		}

		/*--------------------------------------------------------------------------------------------*/
		public InteractionPlaneState GetPlane(string pId) {
			InteractionPlaneState plane;
			vPlaneMap.TryGetValue(pId, out plane);
			return plane;
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool RemovePlane(string pId) {
			if ( !vPlaneMap.ContainsKey(pId) ) {
				return false;
			}

			return vPlaneMap.Remove(pId);
		}

	}

}


