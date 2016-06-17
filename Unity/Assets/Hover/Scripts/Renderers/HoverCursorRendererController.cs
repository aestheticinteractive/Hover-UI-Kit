using Hover.Cursors;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverCursorFollower))]
	public abstract class HoverCursorRendererController : MonoBehaviour,
																ISettingsController, ITreeUpdateable {
		
		public ISettingsControllerMap Controllers { get; private set; }
		public abstract string DefaultCursorPrefabResourcePath { get; }

		[DisableWhenControlled(DisplayMessage=true)]
		public GameObject CursorRendererPrefab;

		[SerializeField]
		[DisableWhenControlled]
		protected Component _CursorRenderer;
		
		[DisableWhenControlled]
		public string SortingLayer = "Default";

		[DisableWhenControlled]
		public bool ClickToRebuildRenderer = false;

		[HideInInspector]
		[SerializeField]
		protected bool _IsBuilt;

		private GameObject vPrevCursorPrefab;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverCursorRendererController() {
			Controllers = new SettingsControllerMap();
		}

		/*--------------------------------------------------------------------------------------------*/
		public ICursorRenderer CursorRenderer {
			get { return (_CursorRenderer as ICursorRenderer); }
			set { _CursorRenderer = (Component)value; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !_IsBuilt ) {
				CursorRendererPrefab = Resources.Load<GameObject>(DefaultCursorPrefabResourcePath);
				_IsBuilt = true;
			}

			vPrevCursorPrefab = CursorRendererPrefab;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			DestroyRenderersIfNecessary();
			TryRebuildWithItemType();

			CursorRenderer.RendererController = this;
			UpdateCursorSettings(GetComponent<HoverCursorFollower>());

			Controllers.TryExpireControllers();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DestroyRenderersIfNecessary() {
			if ( ClickToRebuildRenderer || CursorRendererPrefab != vPrevCursorPrefab ) {
				vPrevCursorPrefab = CursorRendererPrefab;
				RendererUtil.DestroyRenderer(CursorRenderer);
				CursorRenderer = null;
			}

			ClickToRebuildRenderer = false;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void TryRebuildWithItemType() {
			CursorRenderer = (CursorRenderer ?? FindOrBuildCursor());
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected abstract ICursorRenderer FindOrBuildCursor();
		
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateCursorSettings(HoverCursorFollower pFollower) {
			IHoverCursorData data = pFollower.GetCursorData();

			CursorRenderer.gameObject.SetActive(data.IsActive);
			CursorRenderer.HighlightProgress = data.MaxItemHighlightProgress;
			CursorRenderer.SelectionProgress = data.MaxItemSelectionProgress;
		}
		
	}

}
