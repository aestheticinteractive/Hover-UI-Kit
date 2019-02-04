using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRenderersManager : MonoBehaviour {

		public bool IsAppUsingLinearColorSpace = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			MeshBuilder.UseLinearColorSpace = IsAppUsingLinearColorSpace;
		}

	}

}
