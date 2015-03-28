using Hover.Board.Display;
using Hover.Board.Items;
using Hover.Common.Custom;

namespace Hover.Board.Custom {

	/*================================================================================================*/
	public abstract class HoverboardItemVisualSettings :
										HovercommonItemVisualSettings<HoverboardItem, IUiItemRenderer> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override string GetDomain() {
			return "Hoverboard";
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override string GetRendererUnit() {
			return "Item";
		}

	}

}
