using System;

namespace Hover.Board.Items {

	/*================================================================================================*/
	public class ItemPanel : IItemPanel { 

		public object DisplayContainer { get; internal set; }

		private readonly Func<IItemGrid[]> vGetGrids;
		private IItemGrid[] vActiveGrids;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemPanel(Func<IItemGrid[]> pGetGrids) {
			vGetGrids = pGetGrids;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IItemGrid[] Grids {
			get {
				if ( vActiveGrids == null ) {
					vActiveGrids = vGetGrids();
				}

				return vActiveGrids;
			}
		}

	}

}
