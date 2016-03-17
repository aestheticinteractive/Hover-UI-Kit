using Hover.Board.Custom;
using Hover.Board.State;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Board.Display {

	/*================================================================================================*/
	public class UiProjection : MonoBehaviour {

		private ProjectionState vProjectionState;
		private IProjectionVisualSettings vSettings;

		private GameObject vRendererHold;
		private GameObject vRendererObj;
		private IUiProjectionRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ProjectionState pProjectionState, IProjectionVisualSettings pSettings) {
			vProjectionState = pProjectionState;
			vSettings = pSettings;

			////

			vRendererHold = new GameObject("ProjectionRendererHold");
			vRendererHold.transform.SetParent(gameObject.transform, false);

			vRendererObj = new GameObject("ProjectionRenderer");
			vRendererObj.transform.SetParent(vRendererHold.transform, false);

			vRenderer = (IUiProjectionRenderer)vRendererObj.AddComponent(vSettings.Renderer);
			vRenderer.Build(vProjectionState, vSettings);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			ICursorState cursorState = vProjectionState.Cursor;

			bool isActive = (cursorState.IsInputAvailable && 
				vProjectionState.ProjectedPanelPosition != null &&
				vProjectionState.ProjectedPanelDistance > 0.001f &&
				vProjectionState.ProjectedPanelProgress > 0);

			vRendererHold.SetActive(isActive);

			if ( !isActive ) {
				return;
			}

			Vector3 projPos = (Vector3)vProjectionState.ProjectedPanelPosition;
			Vector3 projPosToCursor = cursorState.Position-projPos;

			vRendererHold.transform.localPosition = projPos;
			vRendererHold.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.up, projPosToCursor);
		}

	}

}
