using Hover.Common.Input;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public class CursorInteractState : ICursorInteractState {

		public CursorType Type { get; private set; }
		public CursorDomain Domain { get; private set; }
		public string Id { get; private set; }
		public float DisplayStrength { get; set; }
		public float HighlightProgress { get; set; }
		public float SelectionProgress { get; set; }
		public Vector3 CursorToTarget { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorInteractState(CursorType pType, CursorDomain pDomain, string pId) {
			Type = pType;
			Domain = pDomain;
			Id = pId;
		}

	}

}
