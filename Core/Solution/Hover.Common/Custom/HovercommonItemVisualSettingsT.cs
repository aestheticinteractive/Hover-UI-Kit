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
				vDefaultSettings = GetSettingsInner(vDefaultItem, null, false);
				vSettingsMap.Add(vDefaultItem, vDefaultSettings);
			}

			if ( pItem == null ) {
				return vDefaultSettings;
			}

			if ( vSettingsMap.ContainsKey(pItem) ) {
				return vSettingsMap[pItem];
			}

			if ( IsDefaultSettingsComponent ) {
				IItemVisualSettings sett = TryGetCustomItem(pItem);

				if ( sett != null ) {
					vSettingsMap.Add(pItem, sett);
					return sett;
				}
			}

			return GetVerifyAndSaveSettings(pItem, vDefaultSettings, true);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected internal override IItemVisualSettings GetVerifyAndSaveSettings(IBaseItem pItem,
												IItemVisualSettings pDefault, bool pFillWithDefault) {
			IItemVisualSettings sett = GetSettingsInner(pItem, pDefault, pFillWithDefault);
			CustomUtil.VerifyRenderer<TRend>(sett.Renderer, pItem.Label, GetDomain(),GetRendererUnit());
			vSettingsMap.Add(pItem, sett);
			return sett;
		}


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

			return customSett.GetVerifyAndSaveSettings(pItem, vDefaultSettings, false);
		}

	}

}
