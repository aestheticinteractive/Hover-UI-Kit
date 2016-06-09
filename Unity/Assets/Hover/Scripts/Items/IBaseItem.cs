namespace Hover.Items {

	/*================================================================================================*/
	public interface IBaseItem {

		int AutoId { get; }
		string Id { get; }
		string Label { get; }

		bool IsEnabled { get; }
		bool IsVisible { get; }
		bool IsAncestryEnabled { get; set; }
		bool IsAncestryVisible { get; set; }

	}

}
