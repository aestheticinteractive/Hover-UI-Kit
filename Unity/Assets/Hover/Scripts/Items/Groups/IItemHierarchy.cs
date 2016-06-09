using Hover.Items.Types;

namespace Hover.Items.Groups {

	/*================================================================================================*/
	public interface IItemHierarchy { 

		event ItemEvents.HierarchyLevelChangedHandler OnLevelChange;
		event ItemEvents.GroupItemSelectedHandler OnItemSelection;

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
