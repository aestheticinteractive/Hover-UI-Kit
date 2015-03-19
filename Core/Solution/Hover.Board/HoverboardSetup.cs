using System.Linq;
using Hover.Board.Custom;
using Hover.Board.Custom.Default;
using Hover.Board.Display;
using Hover.Board.Navigation;
using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.Input;
using Hover.Common.Util;
using Hover.Cursor;
using UnityEngine;

namespace Hover.Board {

	/*================================================================================================*/
	public class HoverboardSetup : MonoBehaviour {

		public HoverboardItemVisualSettings DefaultItemVisualSettings;
		public HoverboardInteractionSettings InteractionSettings;
		public HoverboardPanel[] Panels;
		public HovercursorSetup Hovercursor;

		private HoverboardState vState;
		private UiPanel[] vUiPanels;


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

			DefaultItemVisualSettings = UnityUtil.CreateComponent<HoverboardItemVisualSettings,
				HoverboardItemVisualSettingsDefault>(DefaultItemVisualSettings, gameObject, prefix);
			DefaultItemVisualSettings.IsDefaultSettingsComponent = true;

			InteractionSettings = UnityUtil.FindComponentOrCreate<HoverboardInteractionSettings,
				HoverboardInteractionSettings>(InteractionSettings, gameObject, prefix);

			Panels = UnityUtil.FindComponentsOrFail(Panels, prefix);
			Hovercursor = UnityUtil.FindComponentOrFail(Hovercursor, prefix);

			vState = new HoverboardState(InteractionSettings.GetSettings(),
				Panels.Select(x => x.GetPanel()).ToArray(), gameObject.transform);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			gameObject.transform.localScale = Vector3.one;

			vState.SetHovercursorState(Hovercursor.State);
			vUiPanels = new UiPanel[vState.Panels.Length];

			var cursorTypes = new[] {
				CursorType.LeftIndex, 
				CursorType.RightIndex
			};

			for ( int i = 0 ; i < vUiPanels.Length ; ++i ) {
				PanelState panelState = vState.Panels[i];
				GameObject panelObj = (GameObject)panelState.ItemPanel.DisplayContainer;
				UiPanel uiPanel = panelObj.AddComponent<UiPanel>();
				uiPanel.Build(panelState, DefaultItemVisualSettings);
				vUiPanels[i] = uiPanel;
			}

			foreach ( CursorType cursorType in cursorTypes ) {
				var projObj = new GameObject("Proj-"+cursorType);
				projObj.transform.SetParent(gameObject.transform, false);
				var uiProj = projObj.AddComponent<UiProjection>();
				uiProj.Build(vState.GetProjectionState(cursorType));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			vState.UpdateAfterInput();

			InteractionSettings interSett = InteractionSettings.GetSettings();

			if ( interSett.ApplyScaleMultiplier ) {
				Vector3 worldUp = transform.TransformVector(Vector3.up);
				interSett.ScaleMultiplier = 1/worldUp.magnitude;
			}
			else {
				interSett.ScaleMultiplier = 1;
			}
		}

	}

}
