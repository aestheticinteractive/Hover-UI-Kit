using System;
using System.Collections.Generic;
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

		private readonly IDictionary<IBaseItem, IItemVisualSettings> vSettingsMap;
		private CustomItemFinder<HoverboardItem, HoverboardItemVisualSettings> vFinder;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverboardItemVisualSettings() {
			vSettingsMap = new Dictionary<IBaseItem, IItemVisualSettings>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IItemVisualSettings GetSettings(IBaseItem pItem) {
			if ( vSettingsMap.ContainsKey(pItem) ) {
				return vSettingsMap[pItem];
			}

			IItemVisualSettings sett;

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

			vSettingsMap.Add(pItem, sett);
			return sett;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract IItemVisualSettings GetSettingsInner(IBaseItem pItem);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IItemVisualSettings TryGetCustomItem(IBaseItem pItem) {
			if ( vFinder == null ) {
				vFinder = new CustomItemFinder<HoverboardItem, HoverboardItemVisualSettings>(Prefix);
				vFinder.FindAll();
			}

			HoverboardItemVisualSettings customSett = vFinder.GetCustom(pItem.AutoId);
			return (customSett == null ? null : customSett.GetSettings(pItem));
		}

	}

}
