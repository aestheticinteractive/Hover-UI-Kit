using Hovercast.Core.Input;
using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;

namespace Hovercast.Core.State {

	/*================================================================================================*/
	public class MenuState {

		public delegate void SideChangeHandler();
		public event SideChangeHandler OnSideChange;

		public IInputProvider InputProvider { get; private set; }
		public NavProvider NavProv { get; private set; }
		public ArcState Arc { get; private set; }
		public CursorState Cursor { get; private set; }

		private readonly InteractionSettings vSettings;
		private bool? vCurrIsMenuOnLeftSide;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuState(IInputProvider pInputProv, NavProvider pNavProv,InteractionSettings pSettings){
			InputProvider = pInputProv;
			NavProv = pNavProv;
			
			vSettings = pSettings;

			Arc = new ArcState(NavProv, vSettings);
			Cursor = new CursorState(vSettings);

			OnSideChange += (() => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			bool isMenuOnLeft = vSettings.IsMenuOnLeftSide;
			IInputSide cursorSide = InputProvider.GetSide(!isMenuOnLeft);
			IInputSide menuSide = InputProvider.GetSide(isMenuOnLeft);

			Cursor.UpdateAfterInput(cursorSide.Cursor);
			Arc.UpdateAfterInput(menuSide.Menu);
			Arc.UpdateWithCursor(Cursor);

			if ( isMenuOnLeft != vCurrIsMenuOnLeftSide ) {
				vCurrIsMenuOnLeftSide = isMenuOnLeft;
				OnSideChange();
			}
		}

	}

}
