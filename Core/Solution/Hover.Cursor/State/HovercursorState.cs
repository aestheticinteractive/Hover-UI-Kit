using System;
using System.Collections.Generic;
using System.Linq;
using Hover.Common.Input;
using Hover.Cursor.Custom;
using Hover.Cursor.Display;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public class HovercursorState : IHovercursorState {

		public struct CursorPair {
			public CursorState State;
			public UiCursor Display;
		}

		public HovercursorCustomizationProvider CustomizationProvider { get; private set; }
		public HovercursorInputProvider InputProvider { get; private set; }
		public CursorType[] InitializedCursorTypes { get; private set; }
		public Transform CameraTransform { get; private set; }

		private readonly Func<CursorType, CursorPair> vInitCursorType;
		private readonly IDictionary<CursorType, CursorState> vCursorStateMap;
		private readonly IDictionary<CursorType, Transform> vCursorTransformMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercursorState(Func<CursorType, CursorPair> pInitType, HovercursorInputProvider pInput,
										HovercursorCustomizationProvider pCustom, Transform pCamera) {
			vInitCursorType = pInitType;

			InitializedCursorTypes = new CursorType[0];
			CustomizationProvider = pCustom;
			InputProvider = pInput;
			CameraTransform = pCamera;

			vCursorStateMap = new Dictionary<CursorType, CursorState>();
			vCursorTransformMap = new Dictionary<CursorType, Transform>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ICursorState GetCursorState(CursorType pType) {
			TryInitCursor(pType);
			return vCursorStateMap[pType];
		}

		/*--------------------------------------------------------------------------------------------*/
		public Transform GetCursorTransform(CursorType pType) {
			TryInitCursor(pType);
			return vCursorTransformMap[pType];
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			foreach ( CursorState cursorState in vCursorStateMap.Values ) {
				cursorState.UpdateAfterInput();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TryInitCursor(CursorType pType) {
			if ( vCursorStateMap.ContainsKey(pType) ) {
				return;
			}

			CursorPair pair = vInitCursorType(pType);

			vCursorStateMap.Add(pType, pair.State);
			vCursorTransformMap.Add(pType, pair.Display.transform);

			InitializedCursorTypes = InitializedCursorTypes
				.Concat(new[] { pType })
				.ToArray();
		}

	}

}
