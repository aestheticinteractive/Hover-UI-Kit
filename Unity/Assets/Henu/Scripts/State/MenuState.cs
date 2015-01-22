using Henu.Input;
using Henu.Navigation;
using Henu.Settings;

namespace Henu.State {

	/*================================================================================================*/
	public class MenuState {

		public IInputProvider InputProvider { get; private set; }
		public NavigationProvider NavProv { get; private set; }
		public ArcState Arc { get; private set; }
		public CursorState Cursor { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuState(IInputProvider pInputProv, NavigationProvider pNavProv, 
																		InteractionSettings pSettings) {
			InputProvider = pInputProv;
			NavProv = pNavProv;

			Arc = new ArcState(pInputProv, NavProv, pSettings);
			Cursor = new CursorState(pInputProv, pSettings);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			Arc.UpdateAfterInput();
			Cursor.UpdateAfterInput();
			Arc.UpdateWithCursor(Cursor);
		}

	}

}
