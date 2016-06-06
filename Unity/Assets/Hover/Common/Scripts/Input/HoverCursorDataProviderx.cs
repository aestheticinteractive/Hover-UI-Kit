using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hover.Common.Input {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverCursorDataProviderx : MonoBehaviour {

		public List<HoverCursorDatax> Cursors { get; private set; }
		public List<HoverCursorDatax> ExcludedCursors { get; private set; }
		
		private readonly Dictionary<CursorType, HoverCursorDatax> vCursorMap;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverCursorDataProviderx() {
			Cursors = new List<HoverCursorDatax>();
			ExcludedCursors = new List<HoverCursorDatax>();
			
			vCursorMap = new Dictionary<CursorType, HoverCursorDatax>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			gameObject.GetComponentsInChildren(Cursors);
			
			ExcludedCursors.Clear();
			vCursorMap.Clear();
			
			for ( int i = 0 ; i < Cursors.Count ; i++ ) {
				HoverCursorDatax cursor = Cursors[i];
				
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
		public HoverCursorDatax GetCursorData(CursorType pType) {
			if ( !vCursorMap.ContainsKey(pType) ) {
				throw new Exception("No '"+pType+"' cursor was found.");
			}
			
			return vCursorMap[pType];
		}

	}

}
