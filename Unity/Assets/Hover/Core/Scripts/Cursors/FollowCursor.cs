using System;
using UnityEngine;

namespace Hover.Core.Cursors {

	/*================================================================================================*/
	[Serializable]
	public class FollowCursor {

		public CursorType Type { get; private set; }

		public Transform FollowTransform;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public FollowCursor(CursorType pType) {
			Type = pType;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateData(HoverCursorDataProvider pCursorDataProv) {
			if ( !pCursorDataProv.HasCursorData(Type) ) {
				return;
			}

			ICursorDataForInput data = pCursorDataProv.GetCursorDataForInput(Type);

			if ( data == null ) {
				return;
			}

			if ( FollowTransform == null ) {
				data.SetUsedByInput(false);
				return;
			}

			data.SetUsedByInput(FollowTransform.gameObject.activeInHierarchy);
			data.SetWorldPosition(FollowTransform.position);
			data.SetWorldRotation(FollowTransform.rotation);
		}

	}

}
