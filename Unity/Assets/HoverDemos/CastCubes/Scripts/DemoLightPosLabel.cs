using System;
using Hover.Core.Items.Types;
using UnityEngine;

namespace HoverDemos.CastCubes {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverItemDataSlider))]
	public class DemoLightPosLabel : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			GetComponent<HoverItemDataSlider>().GetFormattedLabel = GetLightPosLabel;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private string GetLightPosLabel(IItemDataSlider pSliderData) {
			int snapIndex = (int)Math.Round(pSliderData.SnappedValue*(pSliderData.Snaps-1));
			string snapName = "";

			switch ( snapIndex ) {
				case 0:
					snapName = "Lowest";
					break;

				case 1:
					snapName = "Low";
					break;

				case 2:
					snapName = "High";
					break;

				case 3:
					snapName = "Highest";
					break;

				default:
					snapName = "UNHANDLED";
					break;
			}

			return pSliderData.Label+": "+snapName;
		}

	}

}
