using UnityEditor;
using UnityEngine;

namespace Hover.Common.Edit.Utils {

	/*================================================================================================*/
	public static class EditorUtil {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static GUIStyle GetVerticalSectionStyle() {
			var style = new GUIStyle();
			style.padding = new RectOffset(16, 0, 0, 0);
			return style;
		}

	}

}
