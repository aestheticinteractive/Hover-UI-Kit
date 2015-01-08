using HandMenu.State;

namespace HandMenu.Display {

	/*================================================================================================*/
	public interface IMenuPointRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(MenuHandState pHand, MenuPointState pPoint);

		/*--------------------------------------------------------------------------------------------*/
		void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress);

	}

}
