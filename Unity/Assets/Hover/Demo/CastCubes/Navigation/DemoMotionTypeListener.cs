namespace Hover.Demo.CastCubes.Navigation {

	/*================================================================================================*/
	public class DemoMotionTypeListener : DemoBaseListener<NavItemCheckbox> {

		public DemoEnvironment.MotionType Type;


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
			Enviro.ToggleMotion(Type, pNavItem.Value);
		}

	}

}
