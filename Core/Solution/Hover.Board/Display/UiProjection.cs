using Hover.Board.Display.Default;
using Hover.Board.State;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiProjection : MonoBehaviour {

		private ProjectionState vProjState;

		private GameObject vProjRendererHold;
		private GameObject vProjRendererObj;
		private UiProjectionRenderer vProjRenderer; //TODO: use interface
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ProjectionState pProjectionState) {
			vProjState = pProjectionState;

			////

			vProjRendererHold = new GameObject("ProjectionRendererHold");
			vProjRendererHold.transform.SetParent(gameObject.transform, false);

			vProjRendererObj = new GameObject("ProjectionRenderer");
			vProjRendererObj.transform.SetParent(vProjRendererHold.transform, false);

			vProjRenderer = vProjRendererObj.AddComponent<UiProjectionRenderer>();
			vProjRenderer.Build(vProjState);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			ICursorState cursorState = vProjState.CursorState;

			bool isActive = (cursorState.IsInputAvailable && 
				vProjState.ProjectedPanelPosition != null &&
				vProjState.ProjectedPanelProgress > 0);

			vProjRendererHold.SetActive(isActive);

			if ( !isActive ) {
				return;
			}

			Vector3 projPos = (Vector3)vProjState.ProjectedPanelPosition;
			Vector3 projPosToCursor = (cursorState.Position-projPos);

			vProjRendererHold.transform.localPosition = projPos;
			vProjRendererHold.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.up, projPosToCursor);
		}

	}

}
