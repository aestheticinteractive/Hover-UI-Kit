using System.Collections.Generic;
using System.Linq;
using Hover.Board.Custom;
using Hover.Board.Custom.Standard;
using Hover.Board.Display;
using Hover.Board.Items;
using Hover.Board.State;
using Hover.Common.Input;
using Hover.Common.Util;
using Hover.Cursor;
using UnityEngine;

namespace Hover.Board {

	/*================================================================================================*/
	public class HoverboardSetup : MonoBehaviour {

		private const string CurosrPlaneKey = "Hoverboard.UiPanel";

		public HoverboardPanel[] Panels;
		public HovercursorSetup Hovercursor;
		public HoverboardItemVisualSettings DefaultItemVisualSettings;
		public HoverboardProjectionVisualSettings ProjectionVisualSettings;
		public HoverboardInteractionSettings InteractionSettings;

		private HoverboardState vState;
		private UiPanel[] vUiPanels;
		private IDictionary<CursorType, UiProjection> vProjMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IHoverboardState State {
			get {
				return vState;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			const string prefix = "Hoverboard";

			Panels = UnityUtil.FindComponentsOrFail(Panels, prefix);
			Hovercursor = UnityUtil.FindComponentOrFail(Hovercursor, prefix);

			DefaultItemVisualSettings = UnityUtil.CreateComponent<HoverboardItemVisualSettings,
				HoverboardItemVisualSettingsStandard>(DefaultItemVisualSettings, gameObject, prefix);
			DefaultItemVisualSettings.IsDefaultSettingsComponent = true;

			ProjectionVisualSettings = UnityUtil.FindComponentOrCreate<
				HoverboardProjectionVisualSettings, HoverboardProjectionVisualSettingsStandard>(
					ProjectionVisualSettings,gameObject,prefix);
			
			InteractionSettings = UnityUtil.FindComponentOrCreate<HoverboardInteractionSettings,
				HoverboardInteractionSettings>(InteractionSettings, gameObject, prefix);

			vState = new HoverboardState(Panels.Select(x => x.GetPanel()).ToArray(), Hovercursor, 
				InteractionSettings.GetSettings(), gameObject.transform);

			vProjMap = new Dictionary<CursorType, UiProjection>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			gameObject.transform.localScale = Vector3.one;

			vUiPanels = new UiPanel[vState.Panels.Length];

			for ( int i = 0 ; i < vUiPanels.Length ; ++i ) {
				PanelState panelState = vState.Panels[i];
				GameObject panelObj = (GameObject)panelState.ItemPanel.DisplayContainer;
				UiPanel uiPanel = panelObj.AddComponent<UiPanel>();
				uiPanel.Build(panelState, DefaultItemVisualSettings);
				vUiPanels[i] = uiPanel;

				panelState.InteractionPlane = Hovercursor.State.AddPlane(
					CurosrPlaneKey+"-"+i, panelObj.transform, Vector3.up);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			vState.UpdateAfterInput();

			////

			InteractionSettings interSett = InteractionSettings.GetSettings();

			if ( interSett.ApplyScaleMultiplier ) {
				Vector3 worldUp = transform.TransformVector(Vector3.up);
				interSett.ScaleMultiplier = 1/worldUp.magnitude;
			}
			else {
				interSett.ScaleMultiplier = 1;
			}

			////

			CursorType[] removeCursorTypes = vProjMap.Keys.Except(interSett.Cursors).ToArray();
			CursorType[] addCursorTypes = interSett.Cursors.Except(vProjMap.Keys).ToArray();
			IProjectionVisualSettings projSett = ProjectionVisualSettings.GetSettings();

			foreach ( CursorType cursorType in removeCursorTypes ) {
				UiProjection uiProj = vProjMap[cursorType];
				vProjMap.Remove(cursorType);
				Destroy(uiProj.gameObject);
				vState.RemoveProjection(cursorType);
			}

			foreach ( CursorType cursorType in addCursorTypes ) {
				var projObj = new GameObject("Proj-"+cursorType);
				projObj.transform.SetParent(gameObject.transform, false);
				UiProjection uiProj = projObj.AddComponent<UiProjection>();
				uiProj.Build(vState.GetProjection(cursorType), projSett);

				vProjMap.Add(cursorType, uiProj);
			}
		}

	}

}
