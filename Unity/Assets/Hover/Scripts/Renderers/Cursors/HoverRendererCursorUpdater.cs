using Hover.Cursors;
using Hover.Renderers.Cursors;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverCursorFollower))]
	public class HoverRendererCursorUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public GameObject CursorRendererPrefab;
		protected HoverRendererCursor CursorRenderer;
		public bool ClickToRebuildRenderer = false;

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
			IHoverCursorData cursorData = pFollower.GetCursorData();
			HoverIndicator cursorInd = CursorRenderer.GetIndicator();

			CursorRenderer.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			cursorInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
			cursorInd.Controllers.Set(HoverIndicator.SelectionProgressName, this);

			RendererUtil.SetActiveWithUpdate(CursorRenderer, cursorData.IsActive);
			cursorInd.HighlightProgress = cursorData.MaxItemHighlightProgress;
			cursorInd.SelectionProgress = cursorData.MaxItemSelectionProgress;
		}

	}

}
