using HandMenu.Navigation;

namespace HandMenu.Demo {

	/*================================================================================================*/
	public class DemoNavDelegate : HandMenuNavDelegate {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override INavDelegate GetNavDelegate() {
			return new DemoData();
		}

	}

}
