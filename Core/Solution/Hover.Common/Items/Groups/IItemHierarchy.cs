using Hover.Common.Items.Types;

namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public interface IItemHierarchy { 

		event ItemEvents.HierarchyLevelChangedHandler OnLevelChanged;
		event ItemEvents.GroupItemSelectedHandler OnItemSelected;

		string Title { get; set; }
		IItemGroup CurrentLevel { get; }
		IItemGroup ParentLevel { get; }
		string CurrentLevelTitle { get; }
		SelectorItem NavigateBackItem { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(IItemGroup pRootLevel);

		/*--------------------------------------------------------------------------------------------*/
		void Back();

	}

}
