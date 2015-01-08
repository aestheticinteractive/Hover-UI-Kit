namespace HandMenu.Navigation {

	/*================================================================================================*/
	public class ItemProvider {

		public delegate void DataChangeHandler(ItemData pOldData, ItemData pNewData, int pDirection);
		public delegate void SelectionHandler(ItemProvider pItemProvider);

		public event DataChangeHandler OnDataChange;
		public event SelectionHandler OnSelection;

		public PointData.PointZone Zone { get; private set; }
		public ItemData Data { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemProvider(PointData.PointZone pZone) {
			Zone = pZone;
			OnSelection += (p => {});
			OnDataChange += ((o,n,d) => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Select() {
			OnSelection(this);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithData(ItemData pData, int pDirection) {
			ItemData oldData = Data;
			Data = pData;
			OnDataChange(oldData, Data, pDirection);
		}

	}

}
