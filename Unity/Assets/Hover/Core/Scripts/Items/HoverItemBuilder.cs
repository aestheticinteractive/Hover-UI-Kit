using Hover.Core.Items.Managers;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverItemBuilder : MonoBehaviour {

		public HoverItem.HoverItemType ItemType = HoverItem.HoverItemType.Selector;
		public GameObject ButtonRendererPrefab;
		public GameObject SliderRendererPrefab;

		[TriggerButton("Build Item")]
		public bool ClickToBuild;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( ButtonRendererPrefab == null ) {
				ButtonRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverAlphaButtonRectRenderer-Default");
			}

			if ( SliderRendererPrefab == null ) {
				SliderRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverAlphaSliderRectRenderer-Default");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEditorTriggerButtonSelected() {
			UnityUtil.FindOrAddHoverKitPrefab();
			PerformBuild();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( ClickToBuild ) {
				DestroyImmediate(this, false);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void PerformBuild() {
			TreeUpdater treeUp = gameObject.AddComponent<TreeUpdater>();

			HoverItem item = gameObject.AddComponent<HoverItem>();
			item.ItemType = ItemType;

			HoverItemHighlightState highState = gameObject.AddComponent<HoverItemHighlightState>();

			gameObject.AddComponent<HoverItemSelectionState>();

			HoverItemRendererUpdater rendUp = gameObject.AddComponent<HoverItemRendererUpdater>();
			rendUp.ButtonRendererPrefab = ButtonRendererPrefab;
			rendUp.SliderRendererPrefab = SliderRendererPrefab;
			highState.ProximityProvider = rendUp;

			treeUp.Update(); //forces the entire item to update
		}

	}

}
