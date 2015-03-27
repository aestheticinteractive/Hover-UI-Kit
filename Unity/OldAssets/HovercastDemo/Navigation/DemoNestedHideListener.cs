namespace Hover.Demo.HovercastDemo.Navigation {

	/*================================================================================================*/
	public class DemoNestedHideListener : DemoBaseListener<NavItemCheckbox> {

		public HovercastNavItem ItemToHide;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<bool> pNavItem) {
			ItemToHide.GetItem().IsVisible = !pNavItem.Value;
		}

	}

}
