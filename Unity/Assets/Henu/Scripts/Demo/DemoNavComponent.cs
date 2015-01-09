using Henu.Navigation;

namespace Henu.Demo {

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
