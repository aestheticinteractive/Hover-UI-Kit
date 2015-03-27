using Hover.Board.Devices.Leap.Touch;

namespace Hover.Demo.HoverboardDemo.Navigation {

	/*================================================================================================*/
	public class DemoAllowThumbListener : DemoBaseListener<NavItemCheckbox> {

		//TODO: private HoverboardLeapInputProvider vLeapInputProv;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();

			//TODO: vLeapInputProv = (HoverboardSetup.InputProvider as HoverboardLeapInputProvider);

			//TODO: Item.IsEnabled = (vLeapInputProv != null);
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<bool> pNavItem) {
			//TODO: 
			/*if ( vLeapInputProv == null ) {
				return;
			}

			vLeapInputProv.UseSecondary = pNavItem.Value;*/
		}
	}

}
