using HandMenu.Input;

namespace HandMenu.Navigation {

	/*================================================================================================*/
	public class NavItemProvider {

		public delegate void DataChangeHandler(int pDirection);
		public delegate void SelectionHandler(NavItemProvider pNavItemProvider);

		public event DataChangeHandler OnDataChange;
		public event SelectionHandler OnSelection;

		public InputPointData.PointZone Zone { get; private set; }
		public NavItemData Data { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemProvider(InputPointData.PointZone pZone) {
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
