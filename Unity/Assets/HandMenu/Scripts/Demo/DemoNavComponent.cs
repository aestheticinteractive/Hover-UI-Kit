using HandMenu.Navigation;

namespace HandMenu.Demo {

	/*================================================================================================*/
	public class DemoNavComponent : HandMenuNavComponent {

		public readonly static DemoNavDelegate NavDelegate = new DemoNavDelegate();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override INavDelegate GetNavDelegate() {
			return NavDelegate;
		}

	}

}
