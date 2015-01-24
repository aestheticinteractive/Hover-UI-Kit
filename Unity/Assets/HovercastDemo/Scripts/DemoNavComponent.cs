using Hovercast;
using Hovercast.Navigation;

namespace HovercastDemo {

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
