using Hover.Cast.Custom;
using Hover.Cast.State;
using UnityEngine;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public class UiMenu : MonoBehaviour {

		public const float ScaleArcSize = 1.1f;

		private HovercastState vState;

		private UiPalm vUiPalm;
		private UiArc vUiArc;

		private Quaternion vLeftRot;
		private Quaternion vRightRot;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(HovercastState pState, IItemVisualSettingsProvider pItemVisualSettingsProv,
												IPalmVisualSettingsProvider pPalmVisualSettingsProv) {
			vState = pState;
			vLeftRot = Quaternion.identity;
			vRightRot = Quaternion.AngleAxis(180, Vector3.up);

			var palmObj = new GameObject("Palm");
			palmObj.transform.SetParent(gameObject.transform, false);
			vUiPalm = palmObj.AddComponent<UiPalm>();
			vUiPalm.Build(vState.FullMenu, pPalmVisualSettingsProv);

			var arcObj = new GameObject("Arc");
			arcObj.transform.SetParent(gameObject.transform, false);
			vUiArc = arcObj.AddComponent<UiArc>();
			vUiArc.Build(vState.FullMenu, pItemVisualSettingsProv);

			vState.OnSideChange += HandleSideChange;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			MenuState menu = vState.FullMenu;
			bool isLeft = menu.IsOnLeftSide;
			Vector3 scale = Vector3.one*(menu.Size*ScaleArcSize);

			if ( !isLeft ) {
				scale.z *= -1;
			}

			gameObject.transform.localPosition = menu.Center;
			gameObject.transform.localRotation = menu.Rotation;
			gameObject.transform.localScale = scale;

			vUiArc.gameObject.transform.localRotation = (isLeft ? vLeftRot : vRightRot);
			vUiPalm.gameObject.transform.localRotation = (isLeft ? vLeftRot : vRightRot);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleSideChange() {
			vUiPalm.UpdateAfterSideChange();
			vUiArc.UpdateAfterSideChange();
		}

	}

}
