namespace Hover.Demo.CastCubes.Navigation {

	/*================================================================================================*/
	public class DemoMotionSpeedListener : DemoBaseListener<NavItemSlider> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.ValueToLabel = (s => Component.Label+": "+s.RangeValue.ToString("0.0")+"x");
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<float> pNavItem) {
			Enviro.SetMotionSpeed(Item.RangeValue);
		}

	}

}
