namespace Hover.Common.Items {

	/*================================================================================================*/
	public abstract class BaseItem : IBaseItem {

		private static int ItemCount;

		public int AutoId { get; private set; }
		public string Id { get; set; }
		public virtual string Label { get; set; }
		public float Width { get; set; } //TODO: doesn't update visually for runtime changes
		public float Height { get; set; } //TODO: doesn't update visually for runtime changes
		public object DisplayContainer { get; set; } //TODO: move setter to an "internal" interface

		public bool IsEnabled { get; set; }
		public bool IsVisible { get; set; } //TODO: doesn't update for runtime changes
		public bool IsAncestryEnabled { get; set; } //TODO: move setter to an "internal" interface
		public bool IsAncestryVisible { get; set; } //TODO: move setter to an "internal" interface


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
