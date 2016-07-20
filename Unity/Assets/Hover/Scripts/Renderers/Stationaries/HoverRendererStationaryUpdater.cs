using Hover.Cursors;
using Hover.Items.Managers;
using Hover.RendererModules.Alpha;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Stationaries {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverItemStationaryState))]
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

			//TODO: these methods are just a quick proof-of-concept, will need to be refactored
			UpdatePosition();
			UpdateIndicator();
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
		private void UpdatePosition() {
			HoverItemStationaryState stationState = GetComponent<HoverItemStationaryState>();
			HoverItemStationaryState.HistoryRecord? currRec = stationState.GetCurrentRecord();
			Transform itemPointHold = StationaryRenderer.Fill.ItemPointer.transform.parent;
			Transform cursPointHold = StationaryRenderer.Fill.CursorPointer.transform.parent;

			StationaryRenderer.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);

			RendererUtil.SetActiveWithUpdate(StationaryRenderer.gameObject, 
				(!Application.isPlaying || currRec != null));

			IHoverCursorData cursor;
			Vector3 itemNearestWorldPos;

			if ( currRec == null ) {
				if ( Application.isPlaying ) {
					return;
				}

				cursor = FindObjectOfType<HoverCursorDataProvider>()
					.GetCursorData(CursorType.RightIndex);
				itemNearestWorldPos = transform.position;
			}
			else {
				cursor = currRec.Value.NearestHighlight.Cursor;
				itemNearestWorldPos = currRec.Value.NearestHighlight.NearestWorldPos;
			}

			StationaryRenderer.Controllers.Set(SettingsControllerMap.TransformPosition, this);

			StationaryRenderer.transform.position = cursor.WorldPosition+
				cursor.WorldRotation*(Vector3.up*cursor.Size*1.5f);
			StationaryRenderer.transform.rotation = 
				Quaternion.Slerp(cursor.WorldRotation, transform.rotation, RotationLerp);

			Vector3 itemNearLocalPos = StationaryRenderer.transform
				.InverseTransformPoint(itemNearestWorldPos);
			Vector3 cursorLocalPos = StationaryRenderer.transform
				.InverseTransformPoint(cursor.WorldPosition);

			itemPointHold.localRotation = Quaternion.FromToRotation(Vector3.right, itemNearLocalPos);
			cursPointHold.localRotation = Quaternion.FromToRotation(Vector3.right, cursorLocalPos);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateIndicator() {
			HoverItemStationaryState stationState = GetComponent<HoverItemStationaryState>();
			HoverIndicator stationInd = StationaryRenderer.GetComponent<HoverIndicator>();
			HoverAlphaRendererUpdater alphaUp =
				StationaryRenderer.GetComponent<HoverAlphaRendererUpdater>();

			if ( Application.isPlaying ) {
				stationInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
				stationInd.HighlightProgress = stationState.StationaryProgress;
			}

			//TODO: handle alpha elsewhere
			alphaUp.Controllers.Set(HoverAlphaRendererUpdater.MasterAlphaName, this);
			alphaUp.MasterAlpha = Mathf.Pow(stationInd.HighlightProgress, 2);
		}

	}

}
