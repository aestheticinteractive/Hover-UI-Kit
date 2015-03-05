using System;
using System.Linq;
using Hoverboard.Core.Display;
using Hoverboard.Core.Navigation;
using UnityEngine;

namespace Hoverboard.Core.Custom {

	/*================================================================================================*/
	public abstract class HoverboardCustomButton : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Type GetRendererForNavItemType(NavItem.ItemType pNavItemType) {
			Type type = GetRendererForNavItemTypeInner(pNavItemType);

			if ( type == null ) {
				throw new Exception(GetErrorPrefix(pNavItemType)+"cannot be null.");
			}

			if ( !type.GetInterfaces().Contains(typeof(IUiSegmentRenderer)) ) {
				throw new Exception(GetErrorPrefix(pNavItemType)+"must implement the "+
					typeof(IUiSegmentRenderer).Name+" interface.");
			}

			return type;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract Type GetRendererForNavItemTypeInner(NavItem.ItemType pNavItemType);

		/*--------------------------------------------------------------------------------------------*/
		public abstract ButtonSettings GetSettings();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static string GetErrorPrefix(NavItem.ItemType pNavItemType) {
			return "Hoverboard | The 'Segment' Renderer for '"+pNavItemType+"' NavItems ";
		}

	}

}
