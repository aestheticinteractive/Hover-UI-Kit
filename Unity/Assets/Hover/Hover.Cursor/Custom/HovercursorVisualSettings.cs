using Hover.Common.Custom;
using Hover.Cursor.Display;
using UnityEngine;

namespace Hover.Cursor.Custom {

	/*================================================================================================*/
	public abstract class HovercursorVisualSettings : MonoBehaviour {

		private ICursorSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ICursorSettings GetSettings() {
			if ( vSettings != null ) {
				return vSettings;
			}

			ICursorSettings sett = GetSettingsInner();
			CustomUtil.VerifyRenderer<IUiCursorRenderer>(sett.Renderer, null, "Hovercursor", "Cursor");
			vSettings = sett;
			return sett;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract ICursorSettings GetSettingsInner();

	}

}
