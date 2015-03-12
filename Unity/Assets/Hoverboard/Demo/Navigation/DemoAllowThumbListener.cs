using Hovercast.Core.Navigation;

namespace Hoverboard.Demo.Navigation {

	/*================================================================================================*/
	public class DemoAllowThumbListener : DemoBaseListener<NavItemCheckbox> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.IsEnabled = (LeapInputProv != null);
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<bool> pNavItem) {
			if ( LeapInputProv != null ) {
				LeapInputProv.UseSecondary = pNavItem.Value;
			}
		}

	}

}
