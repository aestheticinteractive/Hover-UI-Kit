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
	public class HoverIdleRendererUpdater : TreeUpdateableBehavior, ISettingsController {

		[SerializeField]
		[FormerlySerializedAs("IdleRendererPrefab")]
		private GameObject _IdleRendererPrefab;

		[SerializeField]
		[FormerlySerializedAs("IdleRenderer")]
		private HoverRendererIdle _IdleRenderer;

		[TriggerButton("Rebuild Idle Renderer")]
		public bool ClickToRebuildRenderer;

		private GameObject vPrevIdlePrefab;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererIdle IdleRenderer {
			get => _IdleRenderer;
			set => this.UpdateValueWithTreeMessage(ref _IdleRenderer, value, "IdleRend");
		}

		/*--------------------------------------------------------------------------------------------*/
		public GameObject IdleRendererPrefab {
			get => _IdleRendererPrefab;
			set => this.UpdateValueWithTreeMessage(ref _IdleRendererPrefab, value, "IdleRendPref");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vPrevIdlePrefab = IdleRendererPrefab;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			ICursorData cursorData = GetComponent<HoverCursorFollower>().GetCursorData();

			bool didChange = ( //this should match the usages in UpdateRenderer()
				IdleRenderer.CenterOffset != GetCenterOffset(cursorData) ||
				IdleRenderer.DistanceThreshold != cursorData.Idle.DistanceThreshold ||
				IdleRenderer.TimerProgress != cursorData.Idle.Progress ||
				IdleRenderer.IsRaycast != cursorData.IsRaycast ||
				IdleRenderer.gameObject.activeSelf != cursorData.Idle.IsActive
			);

			if ( didChange ) {
				TreeUpdater.SendTreeUpdatableChanged(this, "DataChange");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			DestroyRendererIfNecessary();
			IdleRenderer = (IdleRenderer ?? FindOrBuildIdle());

			ICursorData cursorData = GetComponent<HoverCursorFollower>().GetCursorData();

			UpdateRenderer(cursorData);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEditorTriggerButtonSelected() {
			//do nothing here, check for (ClickToRebuildRenderer == true) elsewhere...
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DestroyRendererIfNecessary() {
			if ( ClickToRebuildRenderer || IdleRendererPrefab != vPrevIdlePrefab ) {
				vPrevIdlePrefab = IdleRendererPrefab;
				RendererUtil.DestroyRenderer(IdleRenderer);
				IdleRenderer = null;
			}

			ClickToRebuildRenderer = false;
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererIdle FindOrBuildIdle() {
			return RendererUtil.FindOrBuildRenderer<HoverRendererIdle>(gameObject.transform, 
				IdleRendererPrefab, "Idle", typeof(HoverRendererIdle));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRenderer(ICursorData pCursorData) {
			IdleRenderer.Controllers.Set(HoverRendererIdle.CenterPositionName, this);
			IdleRenderer.Controllers.Set(HoverRendererIdle.DistanceThresholdName, this);
			IdleRenderer.Controllers.Set(HoverRendererIdle.TimerProgressName, this);
			IdleRenderer.Controllers.Set(HoverRendererIdle.IsRaycastName, this);
			IdleRenderer.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);

			IdleRenderer.CenterOffset = GetCenterOffset(pCursorData);
			IdleRenderer.DistanceThreshold = pCursorData.Idle.DistanceThreshold;
			IdleRenderer.TimerProgress = pCursorData.Idle.Progress;
			IdleRenderer.IsRaycast = pCursorData.IsRaycast;
			RendererUtil.SetActiveWithUpdate(IdleRenderer.gameObject, pCursorData.Idle.IsActive);

			/*Transform itemPointHold = IdleRenderer.Fill.ItemPointer.transform.parent;
			Transform cursPointHold = IdleRenderer.Fill.CursorPointer.transform.parent;
			
			Vector3 itemCenter = GetComponent<HoverRendererUpdater>()
				.ActiveRenderer.GetCenterWorldPosition();
			Vector3 itemCenterLocalPos = IdleRenderer.transform
				.InverseTransformPoint(itemCenter);
			Vector3 cursorLocalPos = IdleRenderer.transform
				.InverseTransformPoint(pCursorData.WorldPosition);

			itemPointHold.localRotation = Quaternion.FromToRotation(Vector3.right, itemCenterLocalPos);
			cursPointHold.localRotation = Quaternion.FromToRotation(Vector3.right, cursorLocalPos);*/
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Vector3 GetCenterOffset(ICursorData pData) {
			return transform.InverseTransformPoint(pData.Idle.WorldPosition);
		}

	}

}
