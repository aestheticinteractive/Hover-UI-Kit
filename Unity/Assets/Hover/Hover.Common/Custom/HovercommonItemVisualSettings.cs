using Hover.Common.Items;
using UnityEngine;

namespace Hover.Common.Custom {
	
	/*================================================================================================*/
	public abstract class HovercommonItemVisualSettings : MonoBehaviour, IItemVisualSettingsProvider {

		public bool IsDefaultSettingsComponent { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract IItemVisualSettings GetSettings(IBaseItem pItem);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected internal abstract IItemVisualSettings GetVerifyAndSaveSettings(IBaseItem pItem,
			IItemVisualSettings pDefault, bool pFillWithDefault);

		/*--------------------------------------------------------------------------------------------*/
		protected abstract IItemVisualSettings GetSettingsInner(IBaseItem pItem,
			IItemVisualSettings pDefault, bool pFillWithDefault);

		/*--------------------------------------------------------------------------------------------*/
		protected abstract string GetDomain();

		/*--------------------------------------------------------------------------------------------*/
		protected abstract string GetRendererUnit();

	}

}
