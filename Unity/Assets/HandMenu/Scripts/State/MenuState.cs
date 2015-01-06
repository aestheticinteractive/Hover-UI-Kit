using HandMenu.Input;

namespace HandMenu.State {

	/*================================================================================================*/
	public class MenuState {

		public InputProvider InputProvider { get; private set; }
		public bool IsLeftHandMenu { get; private set; }
		public MenuHandState MenuHand { get; private set; }
		public SelectHandState SelectHand { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuState(InputProvider pInputProv, bool pIsLeftHandMenu) {
			InputProvider = pInputProv;
			IsLeftHandMenu = pIsLeftHandMenu;

			MenuHand = new MenuHandState(pInputProv.GetHandProvider(IsLeftHandMenu));
			SelectHand = new SelectHandState(pInputProv.GetHandProvider(!IsLeftHandMenu));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			MenuHand.UpdateAfterInput();
			SelectHand.UpdateAfterInput();
		}

	}

}
