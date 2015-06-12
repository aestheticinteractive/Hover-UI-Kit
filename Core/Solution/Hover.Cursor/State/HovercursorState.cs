using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hover.Common.Input;
using Hover.Common.Util;
using Hover.Cursor.Custom;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public class HovercursorState : IHovercursorState {

		public HovercursorVisualSettings VisualSettings { get; private set; }
		public Transform CameraTransform { get; private set; }
		public ReadOnlyCollection<CursorType> ActiveCursorTypes { get; private set; }

		private readonly HovercursorInput vInput;
		private readonly Transform vBaseTx;
		private readonly Dictionary<CursorType, CursorState> vCursorStateMap;
		private readonly List<CursorState> vCursorStates;
		private readonly Dictionary<CursorType, Transform> vTransformMap;
		private readonly List<IHovercursorDelegate> vDelegates;

		private readonly List<IHovercursorDelegate> vActiveDelegates;
		private readonly List<CursorType> vActiveCursorTypes;
		private readonly HashSet<CursorType> vActiveCursorMap;
		private readonly Dictionary<CursorType, ReadList<PlaneData>> vActiveCursorPlaneMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercursorState(Transform pBaseTx, HovercursorInput pInput,
											HovercursorVisualSettings pVisualSett, Transform pCamera) {
			vBaseTx = pBaseTx;
			vInput = pInput;

			VisualSettings = pVisualSett;
			CameraTransform = pCamera;

			vCursorStateMap = new Dictionary<CursorType, CursorState>(EnumIntKeyComparer.CursorType);
			vCursorStates = new List<CursorState>();
			vTransformMap = new Dictionary<CursorType, Transform>(EnumIntKeyComparer.CursorType);
			vDelegates = new List<IHovercursorDelegate>();

			vActiveDelegates = new List<IHovercursorDelegate>();
			vActiveCursorTypes = new List<CursorType>();
			vActiveCursorMap = new HashSet<CursorType>(EnumIntKeyComparer.CursorType);
			vActiveCursorPlaneMap = new Dictionary<CursorType, ReadList<PlaneData>>(
				EnumIntKeyComparer.CursorType);

			ActiveCursorTypes = new ReadOnlyCollection<CursorType>(vActiveCursorTypes);

			vInput.SetPlaneProvider(GetActiveCursorPlanes);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IHovercursorInput Input {
			get {
				return vInput;
			}
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
			return vTransformMap[pType];
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetCursorTransform(CursorType pType, Transform pTransform) {
			vTransformMap[pType] = pTransform;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateBeforeInput() {
			UpdateActiveDelegates();
			vActiveCursorTypes.Clear();
			vActiveCursorMap.Clear();

			foreach ( IHovercursorDelegate del in vActiveDelegates ) {
				for ( int i = 0 ; i < del.ActiveCursorTypes.Count ; i++ ) {
					CursorType type = del.ActiveCursorTypes[i];

					if ( vActiveCursorMap.Contains(type) ) {
						continue;
					}

					vActiveCursorTypes.Add(type);
					vActiveCursorMap.Add(type);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			UpdateActiveDelegates();

			foreach ( CursorState cursor in vCursorStates ) {
				CursorType type = cursor.Type;
				float maxDispStren = 0;

				cursor.ClearInteractions();

				foreach ( IHovercursorDelegate del in vActiveDelegates ) {
					if ( !CursorTypeUtil.Contains(del.ActiveCursorTypes, type) ) {
						continue;
					}

					maxDispStren = Math.Max(maxDispStren, del.CursorDisplayStrength);
					cursor.AddInteractions(del.GetActiveCursorInteractions(type));
				}

				cursor.UpdateAfterInput(maxDispStren);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddDelegate(IHovercursorDelegate pDelegate) {
			if ( vDelegates.Contains(pDelegate) ) {
				throw new Exception("This "+typeof(IHovercursorDelegate).Name+
					" has already been added.");
			}

			vDelegates.Add(pDelegate);
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool RemoveDelegate(IHovercursorDelegate pDelegate) {
			return vDelegates.Remove(pDelegate);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateActiveDelegates() {
			vActiveDelegates.Clear();

			foreach ( IHovercursorDelegate del in vDelegates ) {
				if ( !del.IsCursorInteractionEnabled || del.CursorDisplayStrength <= 0 ) {
					continue;
				}

				if ( del.ActiveCursorTypes == null ) {
					throw new Exception(
						"The '"+del.Domain+"' "+typeof(IHovercursorDelegate).Name+
						".ActiveCursorTypes list should not be null.");
				}

				if ( del.ActiveCursorTypes.Count == 0 ) {
					continue;
				}

				vActiveDelegates.Add(del);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private ReadOnlyCollection<PlaneData> GetActiveCursorPlanes(CursorType pType) {
			UpdateActiveDelegates();

			ReadList<PlaneData> planes;

			if ( !vActiveCursorPlaneMap.TryGetValue(pType, out planes) ) {
				planes = new ReadList<PlaneData>();
				vActiveCursorPlaneMap.Add(pType, planes);
			}
			else {
				planes.Clear();
			}

			foreach ( IHovercursorDelegate del in vActiveDelegates ) {
				planes.AddRange(del.GetActiveCursorPlanes(pType));
			}

			return vActiveCursorPlaneMap[pType].ReadOnly;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void TryInitCursor(CursorType pType) {
			if ( vCursorStateMap.ContainsKey(pType) ) {
				return;
			}

			var cursor = new CursorState(Input.GetCursor(pType), VisualSettings.GetSettings(), vBaseTx);

			vCursorStateMap.Add(pType, cursor);
			vCursorStates.Add(cursor);
			vActiveCursorTypes.Add(pType);
		}

	}

}
