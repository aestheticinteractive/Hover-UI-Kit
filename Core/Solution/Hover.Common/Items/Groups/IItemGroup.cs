using System;

namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public interface IItemGroup {

		event ItemEvents.GroupItemSelectedHandler OnItemSelected;

		string Title { get; }
		object DisplayContainer { get; }
		
		bool IsEnabled { get; set; }
		bool IsVisible { get; set; }
		bool AreParentsEnabled { get; }
		bool AreParentsVisible { get; }

		IBaseItem[] Items { get; }
		ISelectableItem LastSelectedItem { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T[] GetTypedItems<T>() where T : class, IBaseItem;

		/*--------------------------------------------------------------------------------------------*/
		void SetParentsEnabledFunc(Func<bool> pFunc);

		/*--------------------------------------------------------------------------------------------*/
		void SetParentsVisibleFunc(Func<bool> pFunc);

	}

}
