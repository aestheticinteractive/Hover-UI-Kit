using System;
using System.Linq;
using Hover.Cast.Display;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public abstract class HovercastCustomSegment : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Type GetRendererForNavItemType(SelectableItemType pItemType) {
			Type type = GetRendererForNavItemTypeInner(pItemType);

			if ( type == null ) {
				throw new Exception(GetErrorPrefix(pItemType)+"cannot be null.");
			}

			if ( !type.GetInterfaces().Contains(typeof(IUiSegmentRenderer)) ) {
				throw new Exception(GetErrorPrefix(pItemType)+"must implement the "+
					typeof(IUiSegmentRenderer).Name+" interface.");
			}

			return type;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract Type GetRendererForNavItemTypeInner(SelectableItemType pItemType);

		/*--------------------------------------------------------------------------------------------*/
		public abstract SegmentSettings GetSettings();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static string GetErrorPrefix(SelectableItemType pItemType) {
			return "Hovercast | The 'Segment' Renderer for '"+pItemType+"' NavItems ";
		}

	}

}
