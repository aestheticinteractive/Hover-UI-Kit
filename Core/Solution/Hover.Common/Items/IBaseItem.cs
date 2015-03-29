using System;

namespace Hover.Common.Items {

	/*================================================================================================*/
	public interface IBaseItem {

		event ItemEvents.IsEnabledChangedHandler OnIsEnabledChanged;
		event ItemEvents.IsVisibleChangedHandler OnIsVisibleChanged;

		int AutoId { get; }
		string Id { get; set; }
		string Label { get; set; }
		float Width { get; }
		float Height { get; }
		object DisplayContainer { get; }

		bool IsEnabled { get; set; }
		bool IsVisible { get; set; }
		bool AreParentsEnabled { get; }
		bool AreParentsVisible { get; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetParentsEnabledFunc(Func<bool> pFunc);

		/*--------------------------------------------------------------------------------------------*/
		void SetParentsVisibleFunc(Func<bool> pFunc);

	}

}
