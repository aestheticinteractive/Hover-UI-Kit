using Hover.Cast.Display;
using Hover.Cast.Items;
using Hover.Common.Custom;
using Hover.Common.Items;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public abstract class HovercastPalmVisualSettings : 
			HovercommonItemVisualSettings<HovercastItem, IUiPalmRenderer>, IPalmVisualSettingsProvider {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IPalmVisualSettings GetPalmSettings(IBaseItem pItem) {
			//TODO: BUG: this also finds regular "Item" settings;
			return (IPalmVisualSettings)GetSettings(pItem);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override string GetDomain() {
			return "Hovercast";
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override string GetRendererUnit() {
			return "Palm";
		}

	}

}
