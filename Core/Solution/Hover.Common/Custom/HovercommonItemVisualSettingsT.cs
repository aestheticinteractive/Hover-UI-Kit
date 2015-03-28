using System;
using System.Collections.Generic;
using Hover.Common.Items;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Common.Custom {

	/*================================================================================================*/
	public abstract class HovercommonItemVisualSettings<TMain, TRend> : HovercommonItemVisualSettings
														where TMain : MonoBehaviour, IHovercommonItem {

		private readonly IDictionary<IBaseItem, IItemVisualSettings> vSettingsMap;

		private CustomItemFinder<TMain, HovercommonItemVisualSettings> vFinder;
		private IBaseItem vDefaultItem;
		private IItemVisualSettings vDefaultSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HovercommonItemVisualSettings() {
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
		public override IItemVisualSettings GetSettings(IBaseItem pItem) {
			if ( vDefaultSettings == null ) {
				vDefaultItem = new SelectorItem();
				vDefaultSettings = GetSettingsInner(vDefaultItem);
				vSettingsMap.Add(vDefaultItem, vDefaultSettings);
			}
			
			return GetSettingsWithExternalDefault(pItem, vDefaultSettings);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override IItemVisualSettings GetSettingsWithExternalDefault(IBaseItem pItem,
																		IItemVisualSettings pDefault) {
			if ( pItem == null ) {
				return vDefaultSettings;
			}

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
			CustomUtil.VerifyRenderer<TRend>(sett.Renderer, pItem.Label, GetDomain(),GetRendererUnit());
			vSettingsMap.Add(pItem, sett);
			return sett;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract IItemVisualSettings GetSettingsInner(IBaseItem pItem,
			IItemVisualSettings pDefault=null);

		/*--------------------------------------------------------------------------------------------*/
		protected abstract string GetDomain();

		/*--------------------------------------------------------------------------------------------*/
		protected abstract string GetRendererUnit();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IItemVisualSettings TryGetCustomItem(IBaseItem pItem) {
			if ( vFinder == null ) {
				vFinder = new CustomItemFinder<TMain, HovercommonItemVisualSettings>(GetDomain());
				vFinder.FindAll();
			}

			HovercommonItemVisualSettings customSett = vFinder.GetCustom(pItem.AutoId);

			if ( customSett == null ) {
				return null;
			}

			return customSett.GetSettingsWithExternalDefault(pItem, vDefaultSettings);
		}

	}

}
