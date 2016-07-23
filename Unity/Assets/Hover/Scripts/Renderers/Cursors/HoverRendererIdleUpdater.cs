using Hover.Cursors;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverCursorFollower))]
	public class HoverRendererIdleUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public GameObject IdleRendererPrefab;
		public HoverRendererIdle IdleRenderer;
		public bool ClickToRebuildRenderer = false;

		private GameObject vPrevIdlePrefab;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vPrevIdlePrefab = IdleRendererPrefab;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			DestroyRendererIfNecessary();
			IdleRenderer = (IdleRenderer ?? FindOrBuildIdle());

			IHoverCursorData cursorData = GetComponent<HoverCursorFollower>().GetCursorData();

			UpdatePosition(cursorData);
			UpdateIndicator(cursorData);
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
		private void UpdatePosition(IHoverCursorData pCursorData) {
			IdleRenderer.Controllers.Set(HoverRendererIdle.CenterPositionName, this);
			IdleRenderer.Controllers.Set(HoverRendererIdle.DistanceThresholdName, this);

			IdleRenderer.CenterPosition =
				transform.InverseTransformPoint(pCursorData.Idle.WorldPosition);
			IdleRenderer.DistanceThreshold = pCursorData.Idle.DistanceThreshold;

			/*Transform itemPointHold = IdleRenderer.Fill.ItemPointer.transform.parent;
			Transform cursPointHold = IdleRenderer.Fill.CursorPointer.transform.parent;

			IdleRenderer.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);

			RendererUtil.SetActiveWithUpdate(IdleRenderer.gameObject, 
				(!Application.isPlaying || currRec != null));

			if ( currRec == null ) {
				if ( Application.isPlaying ) {
					return;
				}

				cursor = FindObjectOfType<HoverCursorDataProvider>()
					.GetCursorData(CursorType.RightIndex);
			}
			else {
				cursor = currRec.Value.NearestHighlight.Cursor;
			}*/

			/*IdleRenderer.Controllers.Set(SettingsControllerMap.TransformPosition, this);

			IdleRenderer.transform.position = pCursorData.Idle.WorldPosition;
				//+pCursorData.WorldRotation*(Vector3.up*pCursorData.Size*1.5f);
			//IdleRenderer.transform.rotation = 
			//	Quaternion.Slerp(pCursorData.WorldRotation, transform.rotation, RotationLerp);

			/*Vector3 itemCenter = GetComponent<HoverRendererUpdater>()
				.ActiveRenderer.GetCenterWorldPosition();
			Vector3 itemCenterLocalPos = IdleRenderer.transform
				.InverseTransformPoint(itemCenter);
			Vector3 cursorLocalPos = IdleRenderer.transform
				.InverseTransformPoint(pCursorData.WorldPosition);

			itemPointHold.localRotation = Quaternion.FromToRotation(Vector3.right, itemCenterLocalPos);
			cursPointHold.localRotation = Quaternion.FromToRotation(Vector3.right, cursorLocalPos);*/
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateIndicator(IHoverCursorData pCursorData) {
			if ( !Application.isPlaying ) {
				return;
			}

			HoverIndicator idleInd = IdleRenderer.GetComponent<HoverIndicator>();

			idleInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
			idleInd.HighlightProgress = pCursorData.Idle.Progress;
		}

	}

}
