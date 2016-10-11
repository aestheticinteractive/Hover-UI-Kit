using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public abstract class HoverInputMissing : MonoBehaviour {

		public abstract string ModuleName { get; }
		public abstract string RequiredSymbol { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			Debug.LogError("The '"+ModuleName+"' input module requires the '"+RequiredSymbol+"' "+
				"symbol to be defined.\nAdd it to the 'Scripting Define Symbols' input "+
				"in the Unity 'Player' settings.", this);
		}

	}

}
