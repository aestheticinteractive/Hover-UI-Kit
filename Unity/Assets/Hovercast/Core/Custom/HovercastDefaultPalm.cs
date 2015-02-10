using System;
using Hovercast.Core.Display.Default;
using UnityEngine;

namespace Hovercast.Core.Custom {

	/*================================================================================================*/
	public class HovercastDefaultPalm : HovercastCustomPalm {

		public bool InheritSettings = true;

		public int TextSize = 30;
		public Color TextColor = new Color(1, 1, 1);
		public string TextFont = "Tahoma";
		public Color BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);

		private SegmentSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			if ( InheritSettings ) {
				return;
			}

			vSettings = new SegmentSettings();
			vSettings.TextSize = TextSize;
			vSettings.TextColor = TextColor;
			vSettings.TextFont = TextFont;
			vSettings.BackgroundColor = BackgroundColor;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Type GetRendererInner() {
			return typeof(UiPalmRenderer);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override SegmentSettings GetSettings() {
			return vSettings; //can be null
		}

	}

}
