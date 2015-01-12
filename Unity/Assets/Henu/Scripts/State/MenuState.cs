using Henu.Input;
using Henu.Navigation;

namespace Henu.State {

	/*================================================================================================*/
	public class MenuState {

		public InputProvider InputProvider { get; private set; }
		public NavigationProvider NavProv { get; private set; }
		public bool IsLeftHenu { get; private set; }
		public ArcState Arc { get; private set; }
		public CursorState Cursor { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuState(InputProvider pInputProv, NavigationProvider pNavProv, bool pIsLeftHenu) {
			InputProvider = pInputProv;
			NavProv = pNavProv;
			IsLeftHenu = pIsLeftHenu;

			Arc = new ArcState(pInputProv.GetHandProvider(IsLeftHenu), NavProv);
			Cursor = new CursorState(pInputProv.GetHandProvider(!IsLeftHenu));
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
