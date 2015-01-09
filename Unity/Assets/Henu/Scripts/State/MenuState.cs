using Henu.Input;
using Henu.Navigation;

namespace Henu.State {

	/*================================================================================================*/
	public class MenuState {

		public InputProvider InputProvider { get; private set; }
		public NavigationProvider NavProv { get; private set; }
		public bool IsLeftHenu { get; private set; }
		public MenuHandState MenuHand { get; private set; }
		public SelectHandState SelectHand { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuState(InputProvider pInputProv, NavigationProvider pNavProv, bool pIsLeftHenu) {
			InputProvider = pInputProv;
			NavProv = pNavProv;
			IsLeftHenu = pIsLeftHenu;

			MenuHand = new MenuHandState(pInputProv.GetHandProvider(IsLeftHenu), NavProv);
			SelectHand = new SelectHandState(pInputProv.GetHandProvider(!IsLeftHenu));
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
