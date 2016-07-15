using Hover.Items.Managers;
using Hover.RendererModules.Alpha;
using Hover.Renderers.Shapes.Arc;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Stationarys {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverItemStationaryState))]
	public class HoverRendererStationaryUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public GameObject StationaryRendererPrefab;
		public Component StationaryRenderer;
		public bool ClickToRebuildRenderer = false;

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

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRenderer() {
			HoverItemStationaryState stationState = GetComponent<HoverItemStationaryState>();
			HoverShapeArc rendShapeArc = StationaryRenderer.GetComponent<HoverShapeArc>();
			HoverAlphaRendererUpdater alphaUp = 
				StationaryRenderer.GetComponent<HoverAlphaRendererUpdater>();
			HoverAlphaMeshUpdater caratAlphaUp = StationaryRenderer.transform
				.FindChild("CaratHolder")
				.FindChild("Carat")
				.GetComponent<HoverAlphaMeshUpdater>();

			//TODO: this is very hacky, for a quick test

			rendShapeArc.ArcDegrees = Mathf.Lerp(30, 360, stationState.StationaryProgress);
			alphaUp.MasterAlpha = stationState.StationaryProgress;
			caratAlphaUp.Alpha = stationState.StationaryProgress;
		}

	}

}
