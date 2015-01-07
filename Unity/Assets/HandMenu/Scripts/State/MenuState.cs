using HandMenu.Input;
using HandMenu.Navigation;

namespace HandMenu.State {

	/*================================================================================================*/
	public class MenuState {

		public InputProvider InputProvider { get; private set; }
		public NavigationProvider NavProv { get; private set; }
		public bool IsLeftHandMenu { get; private set; }
		public MenuHandState MenuHand { get; private set; }
		public SelectHandState SelectHand { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuState(InputProvider pInputProv, NavigationProvider pNavProv, bool pIsLeftHandMenu) {
			InputProvider = pInputProv;
			NavProv = pNavProv;
			IsLeftHandMenu = pIsLeftHandMenu;

			MenuHand = new MenuHandState(pInputProv.GetHandProvider(IsLeftHandMenu), NavProv);
			SelectHand = new SelectHandState(pInputProv.GetHandProvider(!IsLeftHandMenu));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			MenuHand.UpdateAfterInput();
			SelectHand.UpdateAfterInput();

			MenuHand.UpdateWithCursor(SelectHand.CursorPosition);
		}

	}

}
