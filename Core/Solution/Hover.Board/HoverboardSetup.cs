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
		private List<CursorType> vPrevActiveCursorTypes;
		private IDictionary<CursorType, UiProjection> vProjMap;
		private List<CursorType> vHideCursorTypes;
		private List<CursorType> vShowCursorTypes;


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

			vPrevActiveCursorTypes = new List<CursorType>();
			vProjMap = new Dictionary<CursorType, UiProjection>(EnumIntKeyComparer.CursorType);
			vHideCursorTypes = new List<CursorType>();
			vShowCursorTypes = new List<CursorType>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			gameObject.transform.localScale = Vector3.one;

			vUiPanels = new UiPanel[vState.Panels.Count];

			for ( int i = 0 ; i < vUiPanels.Length ; ++i ) {
				PanelState panelState = vState.FullPanels[i];
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

			for ( int i = 0 ; i < vState.Panels.Count ; i++ ) {
				IItemPanel itemPanel = vState.Panels[i].ItemPanel;
				((GameObject)itemPanel.DisplayContainer).SetActive(itemPanel.IsVisible);
			}

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

			IList<CursorType> activeCursorTypes = interSett.Cursors;
			IProjectionVisualSettings projSett = ProjectionVisualSettings.GetSettings();

			CursorTypeUtil.Exclude(vPrevActiveCursorTypes, activeCursorTypes, vHideCursorTypes);
			CursorTypeUtil.Exclude(activeCursorTypes, vPrevActiveCursorTypes, vShowCursorTypes);

			foreach ( CursorType cursorType in vHideCursorTypes ) {
				vProjMap[cursorType].gameObject.SetActive(false);
				vState.ActivateProjection(cursorType, false);
			}

			foreach ( CursorType cursorType in vShowCursorTypes ) {
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

			vPrevActiveCursorTypes.Clear();
			vPrevActiveCursorTypes.AddRange(activeCursorTypes);
		}
	}

}
