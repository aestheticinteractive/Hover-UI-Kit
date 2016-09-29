using System;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.CanvasElements {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverCanvas))]
	public class HoverCanvasDataUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public enum IconPairType {
			Unspecified,
			CheckboxOff,
			CheckboxOn,
			RadioOff,
			RadioOn,
			NavigateIn,
			NavigateOut,
			Sticky,
			Slider
		}

		public const string LabelTextName = "LabelText";
		public const string IconTypeName = "IconType";

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(DisplaySpecials=true)]
		public string LabelText;

		[DisableWhenControlled]
		public IconPairType IconType;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverCanvasDataUpdater() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HoverCanvas hoverCanvas = gameObject.GetComponent<HoverCanvas>();

			UpdateLabel(hoverCanvas);
			UpdateIcons(hoverCanvas);
			Controllers.TryExpireControllers();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateLabel(HoverCanvas pHoverCanvas) {
			pHoverCanvas.Label.Controllers.Set(SettingsControllerMap.TextText, this);
			pHoverCanvas.Label.TextComponent.text = LabelText;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateIcons(HoverCanvas pHoverCanvas) {
			var iconOuterType = HoverIcon.IconOffset.None;
			var iconInnerType = HoverIcon.IconOffset.None;

			switch ( IconType ) {
				case IconPairType.Unspecified:
					break;

				case IconPairType.CheckboxOn:
					iconOuterType = HoverIcon.IconOffset.CheckOuter;
					iconInnerType = HoverIcon.IconOffset.CheckInner;
					break;

				case IconPairType.CheckboxOff:
					iconOuterType = HoverIcon.IconOffset.CheckOuter;
					break;

				case IconPairType.RadioOn:
					iconOuterType = HoverIcon.IconOffset.RadioOuter;
					iconInnerType = HoverIcon.IconOffset.RadioInner;
					break;

				case IconPairType.RadioOff:
					iconOuterType = HoverIcon.IconOffset.RadioOuter;
					break;

				case IconPairType.NavigateIn:
					iconOuterType = HoverIcon.IconOffset.NavigateIn;
					break;

				case IconPairType.NavigateOut:
					iconOuterType = HoverIcon.IconOffset.NavigateOut;
					break;

				case IconPairType.Sticky:
					iconOuterType = HoverIcon.IconOffset.Sticky;
					break;

				case IconPairType.Slider:
					iconOuterType = HoverIcon.IconOffset.Slider;
					break;

				default:
					throw new Exception("Unhandled icon type: "+IconType);
			}

			pHoverCanvas.IconOuter.Controllers.Set(HoverIcon.IconTypeName, this);
			pHoverCanvas.IconInner.Controllers.Set(HoverIcon.IconTypeName, this);

			pHoverCanvas.IconOuter.IconType = iconOuterType;
			pHoverCanvas.IconInner.IconType = iconInnerType;
		}

	}

}
