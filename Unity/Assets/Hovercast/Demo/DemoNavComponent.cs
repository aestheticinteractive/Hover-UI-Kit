using Hovercast.Core;
using Hovercast.Core.Navigation;

namespace Hovercast.Demo {

	/*================================================================================================*/
	public class DemoNavComponent : HovercastNavComponent {

		public readonly static DemoNavDelegate NavDelegate = new DemoNavDelegate();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override INavDelegate GetNavDelegate() {
			return NavDelegate;
		}

	}

}
