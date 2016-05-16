using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hover.Common.Input {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HovercursorDataProvider : MonoBehaviour {

		public List<HovercursorData> Cursors { get; private set; }
		public List<HovercursorData> ExcludedCursors { get; private set; }
		
		private readonly Dictionary<CursorType, HovercursorData> vCursorMap;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercursorDataProvider() {
			Cursors = new List<HovercursorData>();
			ExcludedCursors = new List<HovercursorData>();
			
			vCursorMap = new Dictionary<CursorType, HovercursorData>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			gameObject.GetComponentsInChildren<HovercursorData>(Cursors);
			
			ExcludedCursors.Clear();
			vCursorMap.Clear();
			
			for ( int i = 0 ; i < Cursors.Count ; i++ ) {
				HovercursorData cursor = Cursors[i];
				
				if ( vCursorMap.ContainsKey(cursor.Type) ) {
					ExcludedCursors.Add(cursor);
					Cursors.RemoveAt(i);
					i--;
					continue;
				}
				
				vCursorMap.Add(cursor.Type, cursor);
			}
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercursorData GetCursorData(CursorType pType) {
			if ( !vCursorMap.ContainsKey(pType) ) {
				throw new Exception("No '"+pType+"' cursor was found.");
			}
			
			return vCursorMap[pType];
		}

	}

}
