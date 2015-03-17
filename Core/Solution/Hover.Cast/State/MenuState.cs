using Hover.Cast.Custom;
using Hover.Cast.Input;
using Hover.Common.Items.Groups;

namespace Hover.Cast.State {

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
		public MenuState(IInputProvider pInputProv, IItemHierarchy pItemRoot, 
																		InteractionSettings pSettings) {
			vInputProv = pInputProv;
			vSettings = pSettings;

			Arc = new ArcState(pItemRoot, vSettings);
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
