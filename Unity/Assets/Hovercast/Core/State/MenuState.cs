using Hovercast.Core.Custom;
using Hovercast.Core.Input;
using Hovercast.Core.Navigation;

namespace Hovercast.Core.State {

	/*================================================================================================*/
	public class MenuState {

		public delegate void SideChangeHandler();
		public event SideChangeHandler OnSideChange;

		public ArcState Arc { get; private set; }
		public CursorState Cursor { get; private set; }

		private readonly IInputProvider vInputProv;
		private readonly InteractionSettings vSettings;
		private bool? vCurrIsMenuOnLeftSide;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MenuState(IInputProvider pInputProv, NavRoot pNavRoot,InteractionSettings pSettings){
			vInputProv = pInputProv;
			vSettings = pSettings;

			Arc = new ArcState(pNavRoot, vSettings);
			Cursor = new CursorState(vSettings);

			OnSideChange += (() => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			bool isMenuOnLeft = vSettings.IsMenuOnLeftSide;
			IInputSide cursorSide = vInputProv.GetSide(!isMenuOnLeft);
			IInputSide menuSide = vInputProv.GetSide(isMenuOnLeft);

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
