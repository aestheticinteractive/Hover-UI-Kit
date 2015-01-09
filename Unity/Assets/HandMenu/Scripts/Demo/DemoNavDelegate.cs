using HandMenu.Navigation;

namespace HandMenu.Demo {

	/*================================================================================================*/
	public class DemoNavDelegate : HandMenuNavDelegate {

		public static DemoData Data = new DemoData();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override INavDelegate GetNavDelegate() {
			return Data;
		}

	}

}
