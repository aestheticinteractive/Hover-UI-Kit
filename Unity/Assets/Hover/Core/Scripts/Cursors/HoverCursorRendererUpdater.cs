using Hover.Core.Renderers;
using Hover.Core.Renderers.Cursors;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverCursorFollower))]
	public class HoverCursorRendererUpdater : TreeUpdateableBehavior, ISettingsController {

		[SerializeField]
		[FormerlySerializedAs("CursorRendererPrefab")]
		private GameObject _CursorRendererPrefab;

		[SerializeField]
		[FormerlySerializedAs("CursorRenderer")]
		private HoverRendererCursor _CursorRenderer;

		[TriggerButton("Rebuild Cursor Renderer")]
		public bool ClickToRebuildRenderer;

		private GameObject vPrevCursorPrefab;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererCursor CursorRenderer {
			get => _CursorRenderer;
			set => this.UpdateValueWithTreeMessage(ref _CursorRenderer, value, "CursorRend");
		}

		/*--------------------------------------------------------------------------------------------*/
		public GameObject CursorRendererPrefab {
			get => _CursorRendererPrefab;
			set => this.UpdateValueWithTreeMessage(ref _CursorRendererPrefab, value, "CursorRendPref");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vPrevCursorPrefab = CursorRendererPrefab;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( CursorRenderer == null ) {
				return;
			}

			ICursorData cursorData = GetComponent<HoverCursorFollower>().GetCursorData();
			HoverIndicator cursorInd = CursorRenderer.GetIndicator();

			bool didChange = ( //this should match the usages in UpdateRenderer()
				CursorRenderer.gameObject.activeSelf != IsCursorActive(cursorData) ||
				CursorRenderer.IsRaycast != cursorData.IsRaycast ||
				CursorRenderer.ShowRaycastLine != ShowCursorRaycast(cursorData) ||
				CursorRenderer.RaycastWorldOrigin != cursorData.WorldPosition ||
				cursorInd.HighlightProgress != cursorData.MaxItemHighlightProgress ||
				cursorInd.SelectionProgress != cursorData.MaxItemSelectionProgress
			);

			if ( didChange ) {
				TreeUpdater.SendTreeUpdatableChanged(this, "DataChange");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			DestroyRendererIfNecessary();
			CursorRenderer = (CursorRenderer ?? FindOrBuildCursor());
			UpdateRenderer(GetComponent<HoverCursorFollower>());
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

			RendererUtil.SetActiveWithUpdate(CursorRenderer, IsCursorActive(cursorData));
			CursorRenderer.IsRaycast = cursorData.IsRaycast;
			CursorRenderer.ShowRaycastLine = ShowCursorRaycast(cursorData);
			CursorRenderer.RaycastWorldOrigin = cursorData.WorldPosition;
			cursorInd.HighlightProgress = cursorData.MaxItemHighlightProgress;
			cursorInd.SelectionProgress = cursorData.MaxItemSelectionProgress;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static bool IsCursorActive(ICursorData pData) {
			return (pData.IsActive && pData.Capability != CursorCapabilityType.None);
		}

		/*--------------------------------------------------------------------------------------------*/
		private static bool ShowCursorRaycast(ICursorData pData) {
			return (pData.CanCauseSelections && pData.Type != CursorType.Look);
		}

	}

}
