using Henu.Input;
using Henu.Navigation;

namespace Henu.State {

	/*================================================================================================*/
	public class MenuState {

		public IInputProvider InputProvider { get; private set; }
		public NavigationProvider NavProv { get; private set; }
		public bool IsLeftMenu { get; private set; }
		public ArcState Arc { get; private set; }
		public CursorState Cursor { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuState(IInputProvider pInputProv, NavigationProvider pNavProv, bool pIsLeftHenu) {
			InputProvider = pInputProv;
			NavProv = pNavProv;
			IsLeftMenu = pIsLeftHenu;

			Arc = new ArcState(pInputProv.GetHandProvider(IsLeftMenu), NavProv);
			Cursor = new CursorState(pInputProv.GetHandProvider(!IsLeftMenu));
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
