using System;
using System.Linq;
using Hover.Cursor.Display;
using UnityEngine;

namespace Hover.Cursor.Custom {

	/*================================================================================================*/
	public abstract class HovercursorVisualSettings : MonoBehaviour {

		private const string RendererErrorPrefix = "Hovercursor | The 'Cursor' Renderer ";

		private ICursorSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ICursorSettings GetSettings() {
			if ( vSettings != null ) {
				return vSettings;
			}

			ICursorSettings sett = GetSettingsInner();

			if ( sett.Renderer == null ) {
				throw new Exception(RendererErrorPrefix+"cannot be null.");
			}

			if ( !sett.Renderer.GetInterfaces().Contains(typeof(IUiCursorRenderer)) ) {
				throw new Exception(RendererErrorPrefix+"does not implement the "+
					typeof(IUiCursorRenderer).Name+" interface.");
			}

			vSettings = sett;
			return sett;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract ICursorSettings GetSettingsInner();

	}

}
