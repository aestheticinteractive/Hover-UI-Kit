using System;
using System.Linq;
using Hovercast.Core.Display;
using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hovercast.Core.Custom {

	/*================================================================================================*/
	public abstract class HovercastCustomSegment : MonoBehaviour {


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
		public abstract SegmentSettings GetSettings();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static string GetErrorPrefix(NavItem.ItemType pNavItemType) {
			return "Hovercast | The 'ArcSegment' Renderer for '"+pNavItemType+"' NavItems ";
		}

	}

}
