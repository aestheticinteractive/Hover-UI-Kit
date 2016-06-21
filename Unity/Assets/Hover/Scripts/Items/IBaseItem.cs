using UnityEngine;

namespace Hover.Items {

	/*================================================================================================*/
	public interface IBaseItem {

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
