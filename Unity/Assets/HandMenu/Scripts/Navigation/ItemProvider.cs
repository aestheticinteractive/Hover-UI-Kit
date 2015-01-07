namespace HandMenu.Navigation {

	/*================================================================================================*/
	public class ItemProvider {

		public delegate void DataChangeHandler(ItemData pOldData, ItemData pNewData);
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
			OnDataChange += ((o,n) => {});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Select() {
			OnSelection(this);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithData(ItemData pData) {
			ItemData oldData = Data;
			Data = pData;
			OnDataChange(oldData, Data);
		}

	}

}
