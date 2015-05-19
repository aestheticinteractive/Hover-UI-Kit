using Hover.Common.Display;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiItemStickyRenderer : UiItemBaseIconRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Materials.IconOffset GetIconOffset() {
			return Materials.IconOffset.Sticky;
		}

	}

}
