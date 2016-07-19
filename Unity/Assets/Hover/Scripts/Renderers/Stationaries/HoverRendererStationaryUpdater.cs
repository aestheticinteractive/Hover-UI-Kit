using Hover.Cursors;
using Hover.Items.Managers;
using Hover.RendererModules.Alpha;
using Hover.Renderers.Shapes.Arc;
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
		public Component StationaryRenderer;
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
			//StationaryRenderer = (StationaryRenderer ?? FindOrBuildStationary());

			//TODO: these methods are just a quick proof-of-concept, will need to be refactored
			UpdatePosition();
			UpdateRenderer();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DestroyRendererIfNecessary() {
			/*if ( ClickToRebuildRenderer || StationaryRendererPrefab != vPrevStationaryPrefab ) {
				vPrevStationaryPrefab = StationaryRendererPrefab;
				RendererUtil.DestroyRenderer(StationaryRenderer);
				StationaryRenderer = null;
			}

			ClickToRebuildRenderer = false;*/
		}

		/*--------------------------------------------------------------------------------------------* /
		private HoverRendererStationary FindOrBuildStationary() {
			return RendererUtil.FindOrBuildRenderer<HoverRendererStationary>(gameObject.transform, 
				StationaryRendererPrefab, "Stationary", typeof(HoverRendererStationary));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdatePosition() {
			HoverItemStationaryState stationState = GetComponent<HoverItemStationaryState>();
			HoverItemStationaryState.HistoryRecord? currRec = stationState.GetCurrentRecord();
			HoverRenderer rend = StationaryRenderer.GetComponent<HoverRenderer>();
			Transform caratHold = StationaryRenderer.transform.FindChild("CaratHolder");
			Transform caratHold2 = StationaryRenderer.transform.FindChild("CaratHolder2");

			rend.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);

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

			rend.Controllers.Set(SettingsControllerMap.TransformPosition, this);

			StationaryRenderer.transform.position = cursor.WorldPosition+
				cursor.WorldRotation*(Vector3.up*cursor.Size*1.5f);
			StationaryRenderer.transform.rotation = 
				Quaternion.Slerp(cursor.WorldRotation, transform.rotation, RotationLerp);

			Vector3 itemNearLocalPos = StationaryRenderer.transform
				.InverseTransformPoint(itemNearestWorldPos);
			Vector3 cursorLocalPos = StationaryRenderer.transform
				.InverseTransformPoint(cursor.WorldPosition);

			caratHold.localRotation = Quaternion.FromToRotation(Vector3.left, itemNearLocalPos);
			caratHold2.localRotation = Quaternion.FromToRotation(Vector3.left, cursorLocalPos);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRenderer() {
			HoverItemStationaryState stationState = GetComponent<HoverItemStationaryState>();
			HoverRenderer rend = StationaryRenderer.GetComponent<HoverRenderer>();
			HoverShapeArc bgShapeArc = rend
				.GetChildFill(0)
				.GetChildMesh(0)
				.GetComponent<HoverShapeArc>();
			HoverShapeArc highShapeArc = rend
				.GetChildFill(0)
				.GetChildMesh(1)
				.GetComponent<HoverShapeArc>();
			HoverAlphaRendererUpdater alphaUp = 
				StationaryRenderer.GetComponent<HoverAlphaRendererUpdater>();
			HoverAlphaMeshUpdater caratAlphaUp = StationaryRenderer.transform
				.FindChild("CaratHolder")
				.FindChild("Carat")
				.GetComponent<HoverAlphaMeshUpdater>();
			HoverAlphaMeshUpdater caratAlphaUp2 = StationaryRenderer.transform
				.FindChild("CaratHolder2")
				.FindChild("Carat2")
				.GetComponent<HoverAlphaMeshUpdater>();
			float prog = (Application.isPlaying ? stationState.StationaryProgress : 0.75f);

			bgShapeArc.Controllers.Set(HoverShapeArc.ArcDegreesName, this);
			highShapeArc.Controllers.Set(HoverShapeArc.ArcDegreesName, this);
			alphaUp.Controllers.Set(HoverAlphaRendererUpdater.MasterAlphaName, this);
			caratAlphaUp.Controllers.Set(HoverAlphaMeshUpdater.AlphaName, this);
			caratAlphaUp2.Controllers.Set(HoverAlphaMeshUpdater.AlphaName, this);

			highShapeArc.ArcDegrees = Mathf.Lerp(30, 360, prog);
			bgShapeArc.ArcDegrees = 360-highShapeArc.ArcDegrees;
			alphaUp.MasterAlpha = prog;
			caratAlphaUp.Alpha = prog;
			caratAlphaUp2.Alpha = prog;
		}

	}

}
