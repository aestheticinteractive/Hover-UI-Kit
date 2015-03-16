namespace Hover.Common.Items {

	/*================================================================================================*/
	public interface IBaseItem {

		event ItemEvents.IsEnabledChangedHandler OnIsEnabledChanged;
		event ItemEvents.IsVisibleChangedHandler OnIsVisibleChanged;

		int AutoId { get; }
		string Id { get; }
		string Label { get; }

		bool IsEnabled { get; set; }
		bool IsVisible { get; set; }

	}

}
