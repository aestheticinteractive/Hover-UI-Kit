using Hovercast.Core.Navigation;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoNestedHideListener : DemoBaseListener<NavItemCheckbox> {

		public HovercastNavItem ItemToHide;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<bool> pNavItem) {
			ItemToHide.GetItem().IsVisible = !pNavItem.Value;
		}

	}

}
