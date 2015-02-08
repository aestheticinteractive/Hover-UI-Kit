using System.Collections.Generic;
using UnityEngine;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class HovercastNavProvider : MonoBehaviour { 

		public string Title = "Hovercast VR";

		private NavRoot vRoot;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			var rootLevel = new NavLevel(gameObject);
			vRoot = new NavRoot(rootLevel);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			vRoot.Title = Title;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavRoot GetRoot() {
			return vRoot;
		}

	}

}
