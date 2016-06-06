using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hover.Common.Input {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverCursorDataProvider : MonoBehaviour {

		public List<HoverCursorData> Cursors { get; private set; }
		public List<HoverCursorData> ExcludedCursors { get; private set; }
		
		private readonly Dictionary<CursorType, HoverCursorData> vCursorMap;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverCursorDataProvider() {
			Cursors = new List<HoverCursorData>();
			ExcludedCursors = new List<HoverCursorData>();
			
			vCursorMap = new Dictionary<CursorType, HoverCursorData>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			gameObject.GetComponentsInChildren(Cursors);
			
			ExcludedCursors.Clear();
			vCursorMap.Clear();
			
			for ( int i = 0 ; i < Cursors.Count ; i++ ) {
				HoverCursorData cursor = Cursors[i];
				
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
		public HoverCursorData GetCursorData(CursorType pType) {
			if ( !vCursorMap.ContainsKey(pType) ) {
				throw new Exception("No '"+pType+"' cursor was found.");
			}
			
			return vCursorMap[pType];
		}

	}

}
