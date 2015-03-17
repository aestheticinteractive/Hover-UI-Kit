using System;
using System.Linq;
using Hover.Board.Display;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Board.Custom {

	/*================================================================================================*/
	public abstract class HoverboardCustomButton : MonoBehaviour {


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
		public abstract ButtonSettings GetSettings();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static string GetErrorPrefix(IBaseItem pItem) {
			return "Hoverboard | The 'Segment' Renderer for the '"+pItem.Label+"' item ";
		}

	}

}
