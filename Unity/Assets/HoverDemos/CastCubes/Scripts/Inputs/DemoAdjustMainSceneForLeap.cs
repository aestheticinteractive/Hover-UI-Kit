using Hover.Core.Utils;
using Hover.InterfaceModules.Cast;

namespace HoverDemos.CastCubes.Inputs {

	/*================================================================================================*/
	public class DemoAdjustMainSceneForLeap : HoverSceneAdjust {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool IsReadyToAdjust() {
			return (FindObjectOfType<HovercastInterface>() != null);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void PerformAdjustments() {
			//do nothing, for now...
		}

	}

}
