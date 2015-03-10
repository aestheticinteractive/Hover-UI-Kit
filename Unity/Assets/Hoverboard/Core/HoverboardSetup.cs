using System;
using System.Collections.Generic;
using System.Linq;
using Hoverboard.Core.Display;
using Hoverboard.Core.Input;
using Hoverboard.Core.Navigation;
using Hoverboard.Core.State;
using Hoverboard.Core.Custom;
using UnityEngine;

namespace Hoverboard.Core {

	/*================================================================================================*/
	public class HoverboardSetup : MonoBehaviour {

		public HoverboardPanelProvider[] PanelProviders;
		public HoverboardCustomizationProvider CustomizationProvider;
		public HoverboardInputProvider InputProvider;
		public Transform OptionalCameraReference;

		public IHoverboardState State { get; private set; }

		private bool vFailed;
		private OverallState vOverallState;

		private UiPanel[] vUiPanels;
		private IDictionary<CursorType, UiCursor> vUiCursorMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( PanelProviders == null || PanelProviders.Length == 0 ) {
				throw FailMissing("Panel Provider");
			}

			if ( CustomizationProvider == null ) {
				throw FailMissing("Customization Provider");
			}

			if ( InputProvider == null ) {
				throw FailMissing("Input Provider");
			}

			if ( OptionalCameraReference == null ) {
				OptionalCameraReference = gameObject.transform;
			}

			State = new HoverboardState(PanelProviders, CustomizationProvider, 
				InputProvider, OptionalCameraReference);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			if ( vFailed ) {
				return;
			}

			vOverallState = new OverallState(InputProvider, PanelProviders.Select(x => x.GetPanel()),
				CustomizationProvider.GetInteractionSettings(), gameObject.transform);

			vUiPanels = new UiPanel[vOverallState.Panels.Length];
			vUiCursorMap = new Dictionary<CursorType, UiCursor>();

			for ( int i = 0 ; i < vUiPanels.Length ; ++i ) {
				PanelState panelState = vOverallState.Panels[i];
				UiPanel uiPanel = panelState.NavPanel.Container.AddComponent<UiPanel>();
				uiPanel.Build(panelState, CustomizationProvider);
				vUiPanels[i] = uiPanel;
			}

			foreach ( CursorType cursorType in Enum.GetValues(typeof(CursorType)) ) {
				var cursorObj = new GameObject("Cursor"+cursorType);
				cursorObj.transform.SetParent(gameObject.transform, false);
				var uiCursor = cursorObj.AddComponent<UiCursor>();
				uiCursor.Build(vOverallState.GetCursor(cursorType), 
					CustomizationProvider, OptionalCameraReference);

				vUiCursorMap[cursorType] = uiCursor;
			}

			((HoverboardState)State).SetReferences(vOverallState, vUiCursorMap);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vFailed ) {
				return;
			}

			InputProvider.UpdateInput();
			vOverallState.UpdateAfterInput();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Exception FailMissing(string pName) {
			vFailed = true;
			gameObject.SetActive(false);
			return new Exception("Hoverboard | '"+pName+"' must be set.");
		}

	}

}
