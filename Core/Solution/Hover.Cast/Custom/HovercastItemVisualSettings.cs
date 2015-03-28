using Hover.Cast.Display;
using Hover.Cast.Items;
using Hover.Common.Custom;

namespace Hover.Cast.Custom {
	
	/*================================================================================================*/
	public abstract class HovercastItemVisualSettings : 
										HovercommonItemVisualSettings<HovercastItem, IUiItemRenderer> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override string GetDomain() {
			return "Hovercast";
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override string GetRendererUnit() {
			return "Item";
		}

	}

}
