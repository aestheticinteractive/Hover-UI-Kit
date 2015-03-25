using Hover.Cast.Custom;
using Hover.Cast.State;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public class UiPalm : MonoBehaviour {

		private MenuState vMenuState;
		private IPalmVisualSettingsProvider vVisualSettingsProv;
		private bool vRebuildOnUpdate;

		private GameObject vRendererHold;
		private GameObject vPrevRendererObj;
		private GameObject vRendererObj;
		private IUiPalmRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(MenuState pMenu, IPalmVisualSettingsProvider pVisualSettingsProv) {
			vMenuState = pMenu;
			vVisualSettingsProv = pVisualSettingsProv;

			vRendererHold = new GameObject("RendererHold");
			vRendererHold.transform.SetParent(gameObject.transform, false);
			vRendererHold.transform.localRotation = Quaternion.AngleAxis(170, Vector3.up);

			vMenuState.OnLevelChange += HandleLevelChange;
			Rebuild();
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterSideChange() {
			vRebuildOnUpdate = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vPrevRendererObj != null ) {
				vPrevRendererObj.SetActive(false);
				Destroy(vPrevRendererObj);
				vPrevRendererObj = null;
			}

			if ( vRebuildOnUpdate ) {
				vRebuildOnUpdate = false;
				Rebuild();
			}

			vRendererHold.SetActive(vMenuState.DisplayStrength > 0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			vRebuildOnUpdate = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void Rebuild() {
			vPrevRendererObj = vRendererObj;

			const float halfAngle = UiLevel.AngleFull/2f;
			IBaseItem item = vMenuState.GetLevelParentItem();
			IPalmVisualSettings visualSett = vVisualSettingsProv.GetSettings(item);

			vRendererHold.SetActive(true); //ensures that Awake() is called in the renderers

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(vRendererHold.transform, false);

			vRenderer = (IUiPalmRenderer)vRendererObj.AddComponent(visualSett.Renderer);
			vRenderer.Build(vMenuState, visualSett, -halfAngle, halfAngle);
		}

	}

}
