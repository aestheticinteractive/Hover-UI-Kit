using Hover.Board.Custom;
using Hover.Board.Display;
using Hover.Board.Navigation;
using Hover.Board.State;
using Hover.Common.Input;
using Hover.Common.Util;
using Hover.Cursor;
using UnityEngine;

namespace Hover.Board {

	/*================================================================================================*/
	public class HoverboardSetup : MonoBehaviour {

		public HoverboardCustomizationProvider CustomizationProvider;
		public HoverboardPanelProvider[] PanelProviders;
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

			CustomizationProvider = UnityUtil.FindComponentOrCreate<HoverboardCustomizationProvider,
				HoverboardCustomizationProvider>(CustomizationProvider, gameObject, prefix);

			PanelProviders = UnityUtil.FindComponentsOrFail(PanelProviders, prefix);
			Hovercursor = UnityUtil.FindComponentOrFail(Hovercursor, prefix);

			vState = new HoverboardState(CustomizationProvider, PanelProviders, gameObject.transform);
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
				uiPanel.Build(panelState, CustomizationProvider);
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

			var interSett = CustomizationProvider.GetInteractionSettings();

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
