using Hover.Common.Items;
using UnityEngine;

namespace Hover.Common.Custom {
	
	/*================================================================================================*/
	public abstract class HovercommonItemVisualSettings : MonoBehaviour, IItemVisualSettingsProvider {

		public bool IsDefaultSettingsComponent { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract IItemVisualSettings GetSettings(IBaseItem pItem);

		/*--------------------------------------------------------------------------------------------*/
		public abstract IItemVisualSettings GetSettingsWithExternalDefault(IBaseItem pItem,
																		IItemVisualSettings pDefault);

	}

}
