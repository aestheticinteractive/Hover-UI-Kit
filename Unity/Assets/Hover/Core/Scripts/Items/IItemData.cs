using UnityEngine;

namespace Hover.Core.Items {

	/*================================================================================================*/
	public interface IItemData {

		event ItemEvents.EnabledChangedHandler OnEnabledChanged;

		GameObject gameObject { get; }

		int AutoId { get; }
		string Id { get; }
		string Label { get; }

		bool IsEnabled { get; }
		bool IsVisible { get; }
		bool IsAncestryEnabled { get; set; }
		bool IsAncestryVisible { get; set; }

	}

}
