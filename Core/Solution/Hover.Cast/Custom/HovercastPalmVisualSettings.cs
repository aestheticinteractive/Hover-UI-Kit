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
	public abstract class HovercastPalmVisualSettings : MonoBehaviour, IPalmVisualSettingsProvider {

		private const string Prefix = "Hovercast";
		private const string RendererErrorPrefix = Prefix+" | The 'Palm' Renderer ";

		public bool IsDefaultSettingsComponent { get; set; }

		private readonly IDictionary<IBaseItem, IPalmVisualSettings> vSettingsMap;
		private CustomItemFinder<HovercastItem, HovercastPalmVisualSettings> vFinder;
		private IPalmVisualSettings vNullSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HovercastPalmVisualSettings() {
			vSettingsMap = new Dictionary<IBaseItem, IPalmVisualSettings>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IPalmVisualSettings GetSettings(IBaseItem pItem) {
			if ( pItem == null ) {
				if ( vNullSettings == null ) {
					vNullSettings = GetSettingsInner(null);
					ConfirmRendererType(vNullSettings, null);
				}

				return vNullSettings;
			}

			if ( vSettingsMap.ContainsKey(pItem) ) {
				return vSettingsMap[pItem];
			}

			IPalmVisualSettings sett;

			if ( IsDefaultSettingsComponent ) {
				sett = TryGetCustomItem(pItem);

				if ( sett != null ) {
					return sett;
				}
			}

			sett = GetSettingsInner(pItem);
			ConfirmRendererType(sett, pItem);
			vSettingsMap.Add(pItem, sett);
			return sett;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract IPalmVisualSettings GetSettingsInner(IBaseItem pItem);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static void ConfirmRendererType(IPalmVisualSettings pSett, IBaseItem pItem) {
			string label = (pItem == null ? "the root item" : "'"+pItem.Label+"'");

			if ( pSett.Renderer == null ) {
				throw new Exception(RendererErrorPrefix+"for "+label+" cannot be null.");
			}

			if ( !pSett.Renderer.GetInterfaces().Contains(typeof(IUiPalmRenderer)) ) {
				throw new Exception(RendererErrorPrefix+"for "+label+" does not implement the "+
					typeof(IUiPalmRenderer).Name+" interface.");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private IPalmVisualSettings TryGetCustomItem(IBaseItem pItem) {
			if ( vFinder == null ) {
				vFinder = new CustomItemFinder<HovercastItem, HovercastPalmVisualSettings>(Prefix);
				vFinder.FindAll();
			}

			HovercastPalmVisualSettings customSett = vFinder.GetCustom(pItem.AutoId);
			return (customSett == null ? null : customSett.GetSettings(pItem));
		}

	}

}
