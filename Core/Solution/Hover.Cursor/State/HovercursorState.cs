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
		private readonly IDictionary<CursorType, CursorState> vCursorStateMap;
		private readonly IList<CursorState> vCursorStates;
		private readonly IDictionary<CursorType, Transform> vTransformMap;
		private readonly IList<IHovercursorDelegate> vDelegates;

		private readonly IList<IHovercursorDelegate> vActiveDelegates;
		private readonly List<CursorType> vActiveCursorTypes;
		private readonly HashSet<CursorType> vActiveCursorMap;
		private readonly IDictionary<CursorType, CacheList<PlaneData>> vActiveCursorPlaneMap;


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
			vActiveCursorPlaneMap = new Dictionary<CursorType, CacheList<PlaneData>>(
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

			for ( int delI = 0 ; delI < vActiveDelegates.Count ; delI++ ) {
				IHovercursorDelegate del = vActiveDelegates[delI];

				for ( int curI = 0 ; curI < del.ActiveCursorTypes.Length ; curI++ ) {
					CursorType type = del.ActiveCursorTypes[curI];

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

			for ( int curI = 0 ; curI < vCursorStates.Count ; curI++ ) {
				CursorState cursor = vCursorStates[curI];
				CursorType type = cursor.Type;
				float maxDispStren = 0;

				cursor.ClearInteractions();

				foreach ( IHovercursorDelegate del in vActiveDelegates ) {
					if ( !del.ActiveCursorTypes.Contains(type) ) {
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

			for ( int i = 0 ; i < vDelegates.Count ; i++ ) {
				IHovercursorDelegate del = vDelegates[i];

				if ( !del.IsCursorInteractionEnabled || del.CursorDisplayStrength <= 0 ) {
					continue;
				}

				if ( del.ActiveCursorTypes == null ) {
					throw new Exception(
						"The '"+del.Domain+"' "+typeof(IHovercursorDelegate).Name+
						".ActiveCursorTypes list should not be null.");
				}

				if ( del.ActiveCursorTypes.Length == 0 ) {
					continue;
				}

				vActiveDelegates.Add(del);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private ReadOnlyCollection<PlaneData> GetActiveCursorPlanes(CursorType pType) {
			UpdateActiveDelegates();

			CacheList<PlaneData> planes;

			if ( !vActiveCursorPlaneMap.TryGetValue(pType, out planes) ) {
				planes = new CacheList<PlaneData>();
				vActiveCursorPlaneMap.Add(pType, planes);
			}
			else {
				planes.Clear();
			}

			for ( int i = 0 ; i < vActiveDelegates.Count ; i++ ) {
				planes.AddRange(vActiveDelegates[i].GetActiveCursorPlanes(pType));
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
