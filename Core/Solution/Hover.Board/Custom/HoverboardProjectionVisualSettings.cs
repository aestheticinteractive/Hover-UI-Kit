using System;
using System.Linq;
using Hover.Board.Display;
using UnityEngine;

namespace Hover.Board.Custom {

	/*================================================================================================*/
	public abstract class HoverboardProjectionVisualSettings : MonoBehaviour {

		private const string Prefix = "Hoverboard";
		private const string RendererErrorPrefix = Prefix+" | The 'Projection' Renderer ";

		private IProjectionVisualSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IProjectionVisualSettings GetSettings() {
			if ( vSettings != null ) {
				return vSettings;
			}

			IProjectionVisualSettings sett = GetSettingsInner();

			if ( sett.Renderer == null ) {
				throw new Exception(RendererErrorPrefix+"cannot be null.");
			}

			if ( !sett.Renderer.GetInterfaces().Contains(typeof(IUiProjectionRenderer)) ) {
				throw new Exception(RendererErrorPrefix+" does not implement the "+
					typeof(IUiProjectionRenderer).Name+" interface.");
			}

			vSettings = sett;
			return sett;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract IProjectionVisualSettings GetSettingsInner();

	}

}
