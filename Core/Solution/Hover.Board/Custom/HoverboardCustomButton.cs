using System;
using System.Linq;
using Hover.Board.Display;
using Hover.Board.Navigation;
using UnityEngine;

namespace Hover.Board.Custom {

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
