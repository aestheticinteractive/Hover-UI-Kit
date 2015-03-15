namespace Hover.Engines {

	/*================================================================================================*/
	public interface IContainer {

		ITransform Transform { get; }
		IRenderer Renderer { get; }

		IContainer[] Children { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IContainer FindChildByName(string pName, bool pRecursive=true);

		/*--------------------------------------------------------------------------------------------*/
		void AddChild(IContainer pChild);

	}

}
