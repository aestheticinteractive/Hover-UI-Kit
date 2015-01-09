using Henu.Input;

namespace Henu.Navigation {

	/*================================================================================================*/
	public class NavItemProvider {

		public delegate void DataChangeHandler(int pDirection);
		public delegate void SelectionHandler(NavItemProvider pNavItemProvider);

		public event DataChangeHandler OnDataChange;
		public event SelectionHandler OnSelection;

		public InputPointZone Zone { get; private set; }
		public NavItemData Data { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemProvider(InputPointZone pZone) {
			Zone = pZone;
			OnSelection += (p => {});
			OnDataChange += (d => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Select() {
			OnSelection(this);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithData(NavItemData pData, int pDirection) {
			Data = pData;
			OnDataChange(pDirection);
		}

	}

}
