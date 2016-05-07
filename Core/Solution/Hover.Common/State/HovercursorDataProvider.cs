using UnityEngine;

namespace Hover.Common.State {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HovercursorDataProvider : MonoBehaviour {

		public HovercursorData[] Cursors;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			Cursors = gameObject.GetComponentsInChildren<HovercursorData>();
		}

	}

}
