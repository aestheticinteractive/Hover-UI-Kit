namespace Hover.Cast.Input.Transforms {

	/*================================================================================================*/
	public class HovercastTransformInput : HovercastInput {

		public bool IsAvailable = true;
		public float DisplayStrength = 1;
		public float NavigateBackStrength = 0;

		private readonly InputMenu vMenuL;
		private readonly InputMenu vMenuR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastTransformInput() {
			vMenuL = new InputMenu(true);
			vMenuR = new InputMenu(false);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
			UpdateMenu(vMenuL);
			UpdateMenu(vMenuR);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override IInputMenu GetMenu(bool pIsLeft) {
			return (pIsLeft ? vMenuL : vMenuR);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMenu(InputMenu pMenu) {
			UnityEngine.Transform tx = gameObject.transform;

			pMenu.IsAvailable = IsAvailable;
			pMenu.Position = tx.localPosition;
			pMenu.Rotation = tx.localRotation;
			pMenu.DisplayStrength = DisplayStrength;
			pMenu.NavigateBackStrength = NavigateBackStrength;
		}

	}

}
