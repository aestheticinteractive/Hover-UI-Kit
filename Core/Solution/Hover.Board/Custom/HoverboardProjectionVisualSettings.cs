using Hover.Board.Display;
using Hover.Common.Custom;
using UnityEngine;

namespace Hover.Board.Custom {

	/*================================================================================================*/
	public abstract class HoverboardProjectionVisualSettings : MonoBehaviour {

		private IProjectionVisualSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IProjectionVisualSettings GetSettings() {
			if ( vSettings != null ) {
				return vSettings;
			}

			IProjectionVisualSettings sett = GetSettingsInner();

			CustomUtil.VerifyRenderer<IUiProjectionRenderer>(
				sett.Renderer, null, "Hoverboard", "Projection");

			vSettings = sett;
			return sett;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract IProjectionVisualSettings GetSettingsInner();

	}

}
