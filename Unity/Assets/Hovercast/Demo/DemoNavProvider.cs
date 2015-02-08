using Hovercast.Core;
using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hovercast.Demo {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class DemoNavProvider : HovercastNavProvider {

		public static DemoNavProvider Instance;
		public static DemoNavItems Items;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavProvider() {
			Instance = this;
			Items = new DemoNavItems();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			Items.Build(gameObject);
		}

	}

}
