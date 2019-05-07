using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hover.Core.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverCursorDataProvider : MonoBehaviour {

		private static HoverCursorDataProvider InstanceRef;

		public List<ICursorData> Cursors { get; private set; }
		public List<ICursorData> SelectableCursors { get; private set; }
		public List<ICursorData> ExcludedCursors { get; private set; }

		private readonly List<ICursorDataForInput> vCursorsForInput;
		private readonly Dictionary<CursorType, ICursorDataForInput> vCursorMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static HoverCursorDataProvider Instance {
			get {
				if ( InstanceRef == null ) {
					InstanceRef = FindObjectOfType<HoverCursorDataProvider>();
				}

				return InstanceRef;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverCursorDataProvider() {
			Cursors = new List<ICursorData>();
			SelectableCursors = new List<ICursorData>();
			ExcludedCursors = new List<ICursorData>();

			vCursorsForInput = new List<ICursorDataForInput>();
			vCursorMap = new Dictionary<CursorType, ICursorDataForInput>(new CursorTypeComparer());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			Update();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			gameObject.GetComponentsInChildren(true, vCursorsForInput);

			Cursors.Clear();
			SelectableCursors.Clear();
			ExcludedCursors.Clear();
			vCursorMap.Clear();
			
			for ( int i = 0 ; i < vCursorsForInput.Count ; i++ ) {
				ICursorDataForInput cursor = vCursorsForInput[i];

				if ( vCursorMap.ContainsKey(cursor.Type) ) {
					ExcludedCursors.Add(cursor);
					continue;
				}

				Cursors.Add(cursor);
				vCursorMap.Add(cursor.Type, cursor);

				if ( cursor.CanCauseSelections ) {
					SelectableCursors.Add(cursor);
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool HasCursorData(CursorType pType) {
			if ( vCursorMap.Count == 0 ) {
				Update();
				//Debug.Log("Early cursor request. Found "+vCursorMap.Count+" cursor(s).");
			}

			return vCursorMap.ContainsKey(pType);
		}

		/*--------------------------------------------------------------------------------------------*/
		public ICursorData GetCursorData(CursorType pType) {
			return GetCursorDataForInput(pType);
		}

		/*--------------------------------------------------------------------------------------------*/
		public ICursorDataForInput GetCursorDataForInput(CursorType pType) {
			if ( !HasCursorData(pType) ) {
				throw new Exception("No '"+pType+"' cursor was found.");
			}

			return vCursorMap[pType];
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void MarkAllCursorsUnused() {
			for ( int i = 0 ; i < vCursorsForInput.Count ; i++ ) {
				vCursorsForInput[i].SetUsedByInput(false);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ActivateAllCursorsBasedOnUsage() {
			for ( int i = 0 ; i < Cursors.Count ; i++ ) {
				vCursorsForInput[i].ActivateIfUsedByInput();
			}
		}

	}

}
