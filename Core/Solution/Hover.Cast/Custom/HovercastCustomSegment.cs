using System;
using System.Linq;
using Hover.Cast.Display;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public abstract class HovercastCustomSegment : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Type GetRendererForItem(IBaseItem pItem) {
			Type type = GetRendererForItemInner(pItem);

			if ( type == null ) {
				throw new Exception(GetErrorPrefix(pItem)+"cannot be null.");
			}

			if ( !type.GetInterfaces().Contains(typeof(IUiSegmentRenderer)) ) {
				throw new Exception(GetErrorPrefix(pItem)+"must implement the "+
					typeof(IUiSegmentRenderer).Name+" interface.");
			}

			return type;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract Type GetRendererForItemInner(IBaseItem pItem);

		/*--------------------------------------------------------------------------------------------*/
		public abstract SegmentSettings GetSettings();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static string GetErrorPrefix(IBaseItem pItem) {
			return "Hovercast | The 'Segment' Renderer for the '"+pItem.Label+"' item ";
		}

	}

}
