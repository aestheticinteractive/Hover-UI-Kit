using Hover.Cursors;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Stationaries {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverCursorFollower))]
	public class HoverRendererStationaryUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public GameObject StationaryRendererPrefab;
		public HoverRendererStationary StationaryRenderer;
		public bool ClickToRebuildRenderer = false;

		[Range(0, 1)]
		public float RotationLerp = 0;

		private GameObject vPrevStationaryPrefab;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vPrevStationaryPrefab = StationaryRendererPrefab;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			DestroyRendererIfNecessary();
			StationaryRenderer = (StationaryRenderer ?? FindOrBuildStationary());

			IHoverCursorData cursorData = GetComponent<HoverCursorFollower>().GetCursorData();

			UpdatePosition(cursorData);
			UpdateIndicator(cursorData);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DestroyRendererIfNecessary() {
			if ( ClickToRebuildRenderer || StationaryRendererPrefab != vPrevStationaryPrefab ) {
				vPrevStationaryPrefab = StationaryRendererPrefab;
				RendererUtil.DestroyRenderer(StationaryRenderer);
				StationaryRenderer = null;
			}

			ClickToRebuildRenderer = false;
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererStationary FindOrBuildStationary() {
			return RendererUtil.FindOrBuildRenderer<HoverRendererStationary>(gameObject.transform, 
				StationaryRendererPrefab, "Stationary", typeof(HoverRendererStationary));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdatePosition(IHoverCursorData pCursorData) {
			/*Transform itemPointHold = StationaryRenderer.Fill.ItemPointer.transform.parent;
			Transform cursPointHold = StationaryRenderer.Fill.CursorPointer.transform.parent;

			StationaryRenderer.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);

			RendererUtil.SetActiveWithUpdate(StationaryRenderer.gameObject, 
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

			StationaryRenderer.Controllers.Set(SettingsControllerMap.TransformPosition, this);

			StationaryRenderer.transform.position = pCursorData.Idle.WorldPosition;
				//+pCursorData.WorldRotation*(Vector3.up*pCursorData.Size*1.5f);
			//StationaryRenderer.transform.rotation = 
			//	Quaternion.Slerp(pCursorData.WorldRotation, transform.rotation, RotationLerp);

			/*Vector3 itemCenter = GetComponent<HoverRendererUpdater>()
				.ActiveRenderer.GetCenterWorldPosition();
			Vector3 itemCenterLocalPos = StationaryRenderer.transform
				.InverseTransformPoint(itemCenter);
			Vector3 cursorLocalPos = StationaryRenderer.transform
				.InverseTransformPoint(pCursorData.WorldPosition);

			itemPointHold.localRotation = Quaternion.FromToRotation(Vector3.right, itemCenterLocalPos);
			cursPointHold.localRotation = Quaternion.FromToRotation(Vector3.right, cursorLocalPos);*/
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateIndicator(IHoverCursorData pCursorData) {
			if ( !Application.isPlaying ) {
				return;
			}

			HoverIndicator stationInd = StationaryRenderer.GetComponent<HoverIndicator>();

			stationInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
			stationInd.HighlightProgress = pCursorData.Idle.Progress;
		}

	}

}
