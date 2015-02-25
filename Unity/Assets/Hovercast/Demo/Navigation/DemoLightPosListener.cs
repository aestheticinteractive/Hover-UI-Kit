using System;
using Hovercast.Core.Navigation;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoLightPosListener : DemoBaseListener<NavItemSlider> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();

			Item.ValueToLabel = (s => {
				string lbl = "";

				switch ( (int)Math.Round(s.SnappedValue*(s.Snaps-1)) ) {
					case 0: lbl = "Lowest"; break;
					case 1: lbl = "Low"; break;
					case 2: lbl = "High"; break;
					case 3: lbl = "Highest"; break;
				}

				return Component.Label+": "+lbl;
			});

			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<float> pNavItem) {
			Enviro.SetLightPos(Item.RangeValue);
		}

	}

}
