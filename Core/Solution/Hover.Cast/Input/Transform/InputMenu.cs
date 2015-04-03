using UnityEngine;

namespace Hover.Cast.Input.Transform {

	/*================================================================================================*/
	public class InputMenu : IInputMenu {

		public bool IsLeft { get; private set; }
		public bool IsAvailable { get; set; }

		public Vector3 Position { get; set; }
		public Quaternion Rotation { get; set; }
		public float Radius { get; set; }

		public float NavigateBackStrength { get; set; }
		public float DisplayStrength { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputMenu(bool pIsLeft) {
			IsLeft = pIsLeft;
			IsAvailable = true;
			Radius = 0.1f;
		}

	}

}
