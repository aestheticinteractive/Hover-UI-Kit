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

		private const string Domain = "Hoverboard";

		public HoverboardPanel[] Panels;
		public HovercursorSetup Hovercursor;
		public HoverboardItemVisualSettings DefaultItemVisualSettings;
		public HoverboardProjectionVisualSettings ProjectionVisualSettings;
		public HoverboardInteractionSettings InteractionSettings;

		private HoverboardState vState;
		private UiPanel[] vUiPanels;
		private CursorType[] vPrevActiveCursorTypes;
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
			Panels = UnityUtil.FindComponentsOrFail(Panels, Domain);
			Hovercursor = UnityUtil.FindComponentOrFail(Hovercursor, Domain);

			DefaultItemVisualSettings = UnityUtil.CreateComponent<HoverboardItemVisualSettings,
				HoverboardItemVisualSettingsStandard>(DefaultItemVisualSettings, gameObject, Domain);
			DefaultItemVisualSettings.IsDefaultSettingsComponent = true;

			ProjectionVisualSettings = UnityUtil.FindComponentOrCreate<
				HoverboardProjectionVisualSettings, HoverboardProjectionVisualSettingsStandard>(
					ProjectionVisualSettings,gameObject,Domain);
			
			InteractionSettings = UnityUtil.FindComponentOrCreate<HoverboardInteractionSettings,
				HoverboardInteractionSettings>(InteractionSettings, gameObject, Domain);

			vState = new HoverboardState(Panels.Select(x => x.GetPanel()).ToArray(), Hovercursor, 
				InteractionSettings.GetSettings(), gameObject.transform);

			vPrevActiveCursorTypes = new CursorType[0];
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
			}

			Hovercursor.State.AddDelegate(vState);
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

			CursorType[] activeCursorTypes = interSett.Cursors;
			IEnumerable<CursorType> hideCursorTypes = vPrevActiveCursorTypes.Except(activeCursorTypes);
			IEnumerable<CursorType> showCursorTypes = activeCursorTypes.Except(vPrevActiveCursorTypes);
			IProjectionVisualSettings projSett = ProjectionVisualSettings.GetSettings();

			foreach ( CursorType cursorType in hideCursorTypes ) {
				vProjMap[cursorType].gameObject.SetActive(false);
				vState.ActivateProjection(cursorType, false);
			}

			foreach ( CursorType cursorType in showCursorTypes ) {
				if ( vProjMap.ContainsKey(cursorType) ) {
					vProjMap[cursorType].gameObject.SetActive(true);
					vState.ActivateProjection(cursorType, true);
					continue;
				}

				var projObj = new GameObject("Proj-"+cursorType);
				projObj.transform.SetParent(gameObject.transform, false);
				UiProjection uiProj = projObj.AddComponent<UiProjection>();
				uiProj.Build(vState.GetProjection(cursorType), projSett);

				vProjMap.Add(cursorType, uiProj);
			}

			vPrevActiveCursorTypes = activeCursorTypes;
		}

	}

}
