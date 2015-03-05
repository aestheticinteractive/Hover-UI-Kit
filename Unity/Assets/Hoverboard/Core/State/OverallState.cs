using System;
using System.Collections.Generic;
using Hoverboard.Core.Custom;
using Hoverboard.Core.Input;
using Hoverboard.Core.Navigation;

namespace Hoverboard.Core.State {

	/*================================================================================================*/
	public class OverallState {

		public PanelState[] Panels { get; private set; }

		private readonly IInputProvider vInputProv;
		private readonly IDictionary<CursorType, CursorState> vCursorMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public OverallState(IInputProvider pInputProv, IEnumerable<NavPanel> pPanels,
																		InteractionSettings pSettings) {
			vInputProv = pInputProv;

			vCursorMap = new Dictionary<CursorType, CursorState>();

			foreach ( CursorType cursorType in Enum.GetValues(typeof(CursorType)) ) {
				vCursorMap.Add(cursorType, new CursorState(pSettings));
			}

			////

			var panels = new List<PanelState>();

			foreach ( NavPanel navPanel in pPanels ) {
				panels.Add(new PanelState(navPanel, pSettings));
			}
			
			Panels = panels.ToArray();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState GetCursor(CursorType pType) {
			return vCursorMap[pType];
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			foreach ( CursorType cursorType in vCursorMap.Keys ) {
				vCursorMap[cursorType].UpdateAfterInput(vInputProv.GetCursor(cursorType));
			}

			foreach ( PanelState panelState in Panels ) {
				panelState.UpdateWithCursors(vCursorMap);
			}
		}

	}

}
