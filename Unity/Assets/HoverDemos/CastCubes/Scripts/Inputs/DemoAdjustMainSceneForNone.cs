using Hover.Core.Cursors;
using Hover.Core.Utils;
using Hover.InterfaceModules.Cast;

namespace HoverDemos.CastCubes.Inputs {

	/*================================================================================================*/
	public class DemoAdjustMainSceneForNone : HoverSceneAdjust {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool IsReadyToAdjust() {
			return (FindObjectOfType<HovercastInterface>() != null);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void PerformAdjustments() {
			HovercastInterface cast = FindObjectOfType<HovercastInterface>();
			DemoHovercastCustomizer castCustom = cast.GetComponent<DemoHovercastCustomizer>();

			castCustom.StandardFollowCursor = CursorType.LeftPalm;
			castCustom.SwitchedFollowCursor = CursorType.RightPalm;
			castCustom.StandardInteractionCursor = CursorType.RightIndex;
			castCustom.SwitchedInteractionCursor = CursorType.LeftIndex;
		}

	}

}
