namespace Hover.Common.Items {

	/*================================================================================================*/
	public abstract class BaseItem : IBaseItem {

		private static int ItemCount;

		public int AutoId { get; private set; }
		public string Id { get; set; }
		public virtual string Label { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public object DisplayContainer { get; set; }

		public bool IsEnabled { get; set; }
		public bool IsVisible { get; set; }
		public bool IsAncestryEnabled { get; set; }
		public bool IsAncestryVisible { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected BaseItem() {
			AutoId = (++ItemCount);
			Id = GetType().Name+AutoId;

			IsEnabled = true;
			IsVisible = true;
			IsAncestryEnabled = true;
			IsAncestryVisible = true;
		}

	}

}
