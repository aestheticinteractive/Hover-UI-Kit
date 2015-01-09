using Henu;
using Henu.Navigation;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoNavComponent : HenuNavComponent {

		public readonly static DemoNavDelegate NavDelegate = new DemoNavDelegate();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override INavDelegate GetNavDelegate() {
			return NavDelegate;
		}

	}

}
