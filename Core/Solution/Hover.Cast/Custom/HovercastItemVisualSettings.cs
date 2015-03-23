using System;
using System.Collections.Generic;
using System.Linq;
using Hover.Cast.Display;
using Hover.Cast.Items;
using Hover.Common.Custom;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public abstract class HovercastItemVisualSettings : MonoBehaviour, IItemVisualSettingsProvider {

		private const string Prefix = "Hovercast";
		private const string RendererErrorPrefix = Prefix+" | The 'Item' Renderer ";

		public bool IsDefaultSettingsComponent { get; set; }

		private readonly IDictionary<IBaseItem, IItemVisualSettings> vSettingsMap;
		private CustomItemFinder<HovercastItem, HovercastItemVisualSettings> vFinder;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HovercastItemVisualSettings() {
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
				vFinder = new CustomItemFinder<HovercastItem, HovercastItemVisualSettings>(Prefix);
				vFinder.FindAll();
			}

			HovercastItemVisualSettings customSett = vFinder.GetCustom(pItem.AutoId);
			return (customSett == null ? null : customSett.GetSettings(pItem));
		}

	}

}
