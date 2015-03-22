using System;
using System.Linq;
using Hover.Cast.Display;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public abstract class HovercastCustomItem : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Type GetRendererForItem(IBaseItem pItem) {
			Type type = GetRendererForItemInner(pItem);

			if ( type == null ) {
				throw new Exception(GetErrorPrefix(pItem)+"cannot be null.");
			}

			if ( !type.GetInterfaces().Contains(typeof(IUiItemRenderer)) ) {
				throw new Exception(GetErrorPrefix(pItem)+"must implement the "+
					typeof(IUiItemRenderer).Name+" interface.");
			}

			return type;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract Type GetRendererForItemInner(IBaseItem pItem);

		/*--------------------------------------------------------------------------------------------*/
		public abstract ItemVisualSettings GetSettings();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static string GetErrorPrefix(IBaseItem pItem) {
			return "Hovercast | The 'Item' Renderer for the '"+pItem.Label+"' item ";
		}

	}

}
