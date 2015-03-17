namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public interface IItemHierarchy { 

		event ItemEvents.HierarchyLevelChangedHandler OnLevelChange;
		event ItemEvents.GroupItemSelectedHandler OnItemSelection;

		string Title { get; set; }
		IItemGroup CurrentLevel { get; }
		IItemGroup ParentLevel { get; }
		string CurrentLevelTitle { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(IItemGroup pRootLevel);

		/*--------------------------------------------------------------------------------------------*/
		void Back();

	}

}
