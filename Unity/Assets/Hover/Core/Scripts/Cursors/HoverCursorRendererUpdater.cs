using Hover.Core.Renderers;
using Hover.Core.Renderers.Cursors;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverCursorFollower))]
	public class HoverCursorRendererUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public GameObject CursorRendererPrefab;
		protected HoverRendererCursor CursorRenderer;

		[TriggerButton("Rebuild Cursor Renderer")]
		public bool ClickToRebuildRenderer;

		private GameObject vPrevCursorPrefab;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vPrevCursorPrefab = CursorRendererPrefab;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			DestroyRendererIfNecessary();
			CursorRenderer = (CursorRenderer ?? FindOrBuildCursor());
			UpdateRenderer(gameObject.GetComponent<HoverCursorFollower>());
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEditorTriggerButtonSelected() {
			//do nothing here, check for (ClickToRebuildRenderer == true) elsewhere...
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DestroyRendererIfNecessary() {
			if ( ClickToRebuildRenderer || CursorRendererPrefab != vPrevCursorPrefab ) {
				vPrevCursorPrefab = CursorRendererPrefab;
				RendererUtil.DestroyRenderer(CursorRenderer);
				CursorRenderer = null;
			}

			ClickToRebuildRenderer = false;
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererCursor FindOrBuildCursor() {
			return RendererUtil.FindOrBuildRenderer<HoverRendererCursor>(gameObject.transform, 
				CursorRendererPrefab, "Cursor", typeof(HoverRendererCursor));
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRenderer(HoverCursorFollower pFollower) {
			ICursorData cursorData = pFollower.GetCursorData();
			HoverIndicator cursorInd = CursorRenderer.GetIndicator();

			CursorRenderer.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			CursorRenderer.Controllers.Set(HoverRendererCursor.IsRaycastName, this);
			CursorRenderer.Controllers.Set(HoverRendererCursor.ShowRaycastLineName, this);
			CursorRenderer.Controllers.Set(HoverRendererCursor.RaycastWorldOriginName, this);
			cursorInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
			cursorInd.Controllers.Set(HoverIndicator.SelectionProgressName, this);

			RendererUtil.SetActiveWithUpdate(CursorRenderer, 
				(cursorData.IsActive && cursorData.Capability != CursorCapabilityType.None));
			CursorRenderer.IsRaycast = cursorData.IsRaycast;
			CursorRenderer.ShowRaycastLine = (cursorData.CanCauseSelections &&
				cursorData.Type != CursorType.Look);
			CursorRenderer.RaycastWorldOrigin = cursorData.WorldPosition;
			cursorInd.HighlightProgress = cursorData.MaxItemHighlightProgress;
			cursorInd.SelectionProgress = cursorData.MaxItemSelectionProgress;
		}

	}

}
