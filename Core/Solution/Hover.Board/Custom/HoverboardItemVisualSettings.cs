using System;
using System.Linq;
using Hover.Board.Display;
using Hover.Board.Items;
using Hover.Common.Custom;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Board.Custom {

	/*================================================================================================*/
	public abstract class HoverboardItemVisualSettings : MonoBehaviour, IItemVisualSettingsProvider {

		private const string Prefix = "Hoverboard";
		private const string RendererErrorPrefix = Prefix+" | The 'Item' Renderer ";

		public bool IsDefaultSettingsComponent { get; set; }

		private CustomItemFinder<HoverboardItem, HoverboardItemVisualSettings> vFinder;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemVisualSettings GetSettings(IBaseItem pItem) {
			ItemVisualSettings sett;

			if ( IsDefaultSettingsComponent ) {
				sett = TryGetCustomItem(pItem);

				if ( sett != null ) {
					return sett;
				}
			}

			sett = GetSettingsInner(pItem);

			if ( sett.Renderer == null ) {
				throw new Exception(RendererErrorPrefix+"for '"+pItem.Label+"' cannot be null.");
			}

			if ( !sett.Renderer.GetInterfaces().Contains(typeof(IUiItemRenderer)) ) {
				throw new Exception(RendererErrorPrefix+"for '"+pItem.Label+"' does not implement the "+
					typeof(IUiItemRenderer).Name+" interface.");
			}

			return sett;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract ItemVisualSettings GetSettingsInner(IBaseItem pItem);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private ItemVisualSettings TryGetCustomItem(IBaseItem pItem) {
			if ( vFinder == null ) {
				vFinder = new CustomItemFinder<HoverboardItem, HoverboardItemVisualSettings>(Prefix);
				vFinder.FindAll();
			}

			HoverboardItemVisualSettings customSett = vFinder.GetCustom(pItem.AutoId);
			return (customSett == null ? null : customSett.GetSettings(pItem));
		}

	}

}
