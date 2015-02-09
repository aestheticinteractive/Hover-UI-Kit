using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoBaseListener<T> : HovercastNavItemListener<T> where T : NavItem {

		protected DemoEnvironment Enviro { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();
			Enviro = GameObject.Find("DemoEnvironment").GetComponent<DemoEnvironment>();
		}

	}

}
