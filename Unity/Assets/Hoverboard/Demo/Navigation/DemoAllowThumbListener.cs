using Hoverboard.Devices.Leap;
using Hovercast.Core.Navigation;

namespace Hoverboard.Demo.Navigation {

	/*================================================================================================*/
	public class DemoAllowThumbListener : DemoBaseListener<NavItemCheckbox> {

		private HoverboardLeapInputProvider vLeapInputProv;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();

			vLeapInputProv = (HoverboardSetup.InputProvider as HoverboardLeapInputProvider);

			Item.IsEnabled = (vLeapInputProv != null);
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<bool> pNavItem) {
			if ( vLeapInputProv == null ) {
				return;
			}

			vLeapInputProv.UseSecondary = pNavItem.Value;
		}
	}

}
