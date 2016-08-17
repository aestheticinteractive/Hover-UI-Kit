using Hover.Items.Managers;
using Hover.Renderers;
using Hover.Utils;
using UnityEngine;

namespace Hover.Items {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverItemBuilder : MonoBehaviour {

		public HoverItem.HoverItemType ItemType;
		public GameObject ButtonRendererPrefab;
		public GameObject SliderRendererPrefab;
		public bool ClickToBuild = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( ClickToBuild ) {
				ClickToBuild = false;
				BuilderUtil.FindOrAddHoverManagerPrefab();
				PerformBuild();
				DestroyImmediate(this, false);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void PerformBuild() {
			gameObject.AddComponent<TreeUpdater>();

			HoverItem item = gameObject.AddComponent<HoverItem>();
			item.ItemType = ItemType;

			HoverItemHighlightState highState = gameObject.AddComponent<HoverItemHighlightState>();

			gameObject.AddComponent<HoverItemSelectionState>();

			HoverRendererUpdater rendUp = gameObject.AddComponent<HoverRendererUpdater>();
			rendUp.ButtonRendererPrefab = ButtonRendererPrefab;
			rendUp.SliderRendererPrefab = SliderRendererPrefab;
			highState.ProximityProvider = rendUp;

			rendUp.TreeUpdate(); //forces the renderer prefab to load
		}

	}

}
