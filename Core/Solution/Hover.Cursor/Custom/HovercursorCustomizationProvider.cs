using System;
using System.Linq;
using Hover.Cursor.Display;
using UnityEngine;

namespace Hover.Cursor.Custom {

	/*================================================================================================*/
	public abstract class HovercursorCustomizationProvider : MonoBehaviour {

		private const string RendererErrorPrefix = "Hovercursor | The 'Cursor' Renderer ";


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorSettings GetSettings() {
			CursorSettings sett = GetSettingsInner();

			if ( sett.Renderer == null ) {
				throw new Exception(RendererErrorPrefix+"cannot be null.");
			}

			if ( !sett.Renderer.GetInterfaces().Contains(typeof(IUiCursorRenderer)) ) {
				throw new Exception(RendererErrorPrefix+"must implement the "+
					typeof(IUiCursorRenderer).Name+" interface.");
			}

			return sett;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract CursorSettings GetSettingsInner();

	}

}
