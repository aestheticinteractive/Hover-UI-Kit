namespace Hover.Core.Utils {

	/*================================================================================================*/
	public interface ITreeUpdateable {
	
		//GameObject gameObject { get; }
		bool isActiveAndEnabled { get; }
		string TypeName { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Start(); //forces Unity to show the "component enabled" checkbox in the inspector

		/*--------------------------------------------------------------------------------------------*/
		void TreeUpdate();

	}

}
