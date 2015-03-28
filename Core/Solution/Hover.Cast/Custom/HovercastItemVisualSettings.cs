using System;
using System.Collections.Generic;
using System.Linq;
using Hover.Cast.Display;
using Hover.Cast.Items;
using Hover.Common.Custom;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public abstract class HovercastItemVisualSettings : MonoBehaviour, IItemVisualSettingsProvider {

		//TODO: standardize the "vDefaultSettings" changes across all visual settings

		private const string Prefix = "Hovercast";
		private const string RendererErrorPrefix = Prefix+" | The 'Item' Renderer ";

		public bool IsDefaultSettingsComponent { get; set; }

		private readonly IDictionary<IBaseItem, IItemVisualSettings> vSettingsMap;
		private CustomItemFinder<HovercastItem, HovercastItemVisualSettings> vFinder;

		private IItemVisualSettings vDefaultSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HovercastItemVisualSettings() {
			vSettingsMap = new Dictionary<IBaseItem, IItemVisualSettings>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAllSettings(Action<IItemVisualSettings> pUpdateAction) {
			foreach ( IItemVisualSettings sett in vSettingsMap.Values ) {
				pUpdateAction(sett);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IItemVisualSettings GetSettings(IBaseItem pItem) {
			if ( vDefaultSettings == null ) {
				var item = new SelectorItem();
				vDefaultSettings = GetSettingsInner(item);
				vSettingsMap.Add(item, vDefaultSettings);
			}
			
			return GetSettings(pItem, vDefaultSettings);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IItemVisualSettings GetSettings(IBaseItem pItem, IItemVisualSettings pDefault) {
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

			sett = GetSettingsInner(pItem, pDefault);

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
		protected abstract IItemVisualSettings GetSettingsInner(IBaseItem pItem,
			IItemVisualSettings pDefault=null);

		/*--------------------------------------------------------------------------------------------*/
		private IItemVisualSettings TryGetCustomItem(IBaseItem pItem) {
			if ( vFinder == null ) {
				vFinder = new CustomItemFinder<HovercastItem, HovercastItemVisualSettings>(Prefix);
				vFinder.FindAll();
			}

			HovercastItemVisualSettings customSett = vFinder.GetCustom(pItem.AutoId);
			return (customSett == null ? null : customSett.GetSettings(pItem, vDefaultSettings));
		}

	}

}
