using Hoverboard.Core.Custom;
using Hovercast.Core.Navigation;

namespace Hoverboard.Demo.Navigation {

	/*================================================================================================*/
	public class DemoFastSelectListener : DemoBaseListener<NavItemCheckbox> {

		private InteractionSettings vInteractSett;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			vInteractSett = HoverboardSetup.CustomizationProvider.GetInteractionSettings();
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<bool> pNavItem) {
			vInteractSett.SelectionMilliseconds = (pNavItem.Value ? 200 : 400);
		}

	}

}
