using System.Collections.Generic;
using UnityEngine;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class HovercastNavProvider : MonoBehaviour { 

		public string Title = "Hovercast VR";

		private NavRoot vRoot;
		private NavLevel vRootLevel;
		private bool vIsBuilt;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastNavProvider() {
			vRoot = new NavRoot();
			vRootLevel = new NavLevel();
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vRoot.Title = Title;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual NavRoot GetRoot() {
			if ( !vIsBuilt ) {
				vRootLevel.Build(gameObject);
				vRoot.Build(vRootLevel);
				vIsBuilt = true;
			}

			return vRoot;
		}

	}

}
