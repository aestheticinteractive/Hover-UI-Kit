using Henu.Navigation;
using Henu.Settings;
using Henu.State;
using UnityEngine.UI;

namespace Henu.Display.Default {

	/*================================================================================================*/
	public class UiSliderGrabRenderer : UiSelectRenderer {

		private NavItemSlider vNavSlider;
		private Text vText;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(ArcState pArcState, ArcSegmentState pSegState,																									float pAngle0, float pAngle1, ArcSegmentSettings pSettings) {
			base.Build(pArcState, pSegState, pAngle0, pAngle1, pSettings);
			vNavSlider = (NavItemSlider)vSegState.NavItem;
			vText = vTextObj.GetComponent<Text>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();
			vText.text = vNavSlider.ValueToLabel(vNavSlider.CurrentValue);
		}

	}

}
