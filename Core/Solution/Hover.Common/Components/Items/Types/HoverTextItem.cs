using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverTextItem : HoverBaseItem {

		public new ITextItem Item { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverTextItem() {
			Item = new TextItem();
			Init((BaseItem)Item);
		}

	}

}
