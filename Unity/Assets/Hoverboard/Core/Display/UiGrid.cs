using System;
using System.Collections.Generic;
using Hoverboard.Core.Custom;
using Hoverboard.Core.State;
using UnityEngine;

namespace Hoverboard.Core.Display {

	/*================================================================================================*/
	public class UiGrid : MonoBehaviour {

		private GridState vGridState;
		private ICustomSegment vCustom;
		private IList<UiButton> vUiButtons;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(GridState pGrid, ICustomSegment pCustom) {
			vGridState = pGrid;
			vCustom = pCustom;
			vUiButtons = new List<UiButton>();

			int cols = vGridState.NavGrid.Cols;
			int gi = 0;

			for ( int i = 0 ; i < vGridState.Buttons.Length ; i++ ) {
				ButtonState button = vGridState.Buttons[i];
				int w = button.NavItem.Width;
				var pos = new Vector3(gi%cols, 0, (float)Math.Floor((float)gi/cols));

				UiButton uiButton = button.NavItem.Container.AddComponent<UiButton>();
				uiButton.Build(button, vCustom);
				uiButton.transform.localPosition = pos*UiButton.Size;

				vUiButtons.Add(uiButton);
				gi += w;
			}

			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
		}

	}

}
