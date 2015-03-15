namespace Hover.Engines {

	/*================================================================================================*/
	public interface IRenderer {

		Color MaterialColor { get; set; }
		int MaterialRenderQueue { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetMaterialByShaderName(string pShaderName);

	}

}
