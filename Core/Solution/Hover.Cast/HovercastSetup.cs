using Hover.cast.Custom.Standard;
using Hover.Cast.Custom;
using Hover.Cast.Display;
using Hover.Cast.Input;
using Hover.Cast.Items;
using Hover.Cast.State;
using UnityEngine;
using Hover.Cursor;
using Hover.Common.Util;

namespace Hover.Cast {

	/*================================================================================================*/
	public class HovercastSetup : MonoBehaviour {

		public HovercastItemHierarchy ItemHierarchy;
		public HovercursorSetup Hovercursor;
		public HovercastItemVisualSettings DefaultItemVisualSettings;
		public HovercastPalmVisualSettings DefaultPalmVisualSettings;
		public HovercastInteractionSettings InteractionSettings;
		public HovercastInputProvider InputProvider;
		
		private HovercastState vState;
		private UiMenu vUiMenu;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IHovercastState State {
			get {
				return vState;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			const string prefix = "Hovercast";

			ItemHierarchy = UnityUtil.FindComponentOrFail(ItemHierarchy, prefix);
			Hovercursor = UnityUtil.FindComponentOrFail(Hovercursor, prefix);

			DefaultItemVisualSettings = UnityUtil.CreateComponent<HovercastItemVisualSettings,
				HovercastItemVisualSettingsStandard>(DefaultItemVisualSettings, gameObject, prefix);
			DefaultItemVisualSettings.IsDefaultSettingsComponent = true;

			DefaultPalmVisualSettings = UnityUtil.CreateComponent<HovercastPalmVisualSettings,
				HovercastPalmVisualSettingsStandard>(DefaultPalmVisualSettings, gameObject, prefix);
			DefaultPalmVisualSettings.IsDefaultSettingsComponent = true;

			InteractionSettings = UnityUtil.FindComponentOrCreate<HovercastInteractionSettings,
				HovercastInteractionSettings>(InteractionSettings, gameObject, prefix);

			InputProvider = UnityUtil.FindComponentOrFail(InputProvider, prefix);

			vState = new HovercastState(ItemHierarchy.GetRoot(), Hovercursor, 
				InteractionSettings.GetSettings(), InputProvider, gameObject.transform);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			if ( vState == null ) {
				return;
			}

			var menuObj = new GameObject("Menu");
			menuObj.transform.SetParent(gameObject.transform, false);
			vUiMenu = menuObj.AddComponent<UiMenu>();
			vUiMenu.Build(vState, DefaultItemVisualSettings, DefaultPalmVisualSettings);

			vState.SetReferences(menuObj.transform);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vState == null ) {
				return;
			}

			InputProvider.UpdateInput();
			vState.UpdateAfterInput();

			////

			InteractionSettings interSett = InteractionSettings.GetSettings();

			if ( interSett.ApplyScaleMultiplier ) {
				Vector3 worldUp = transform.TransformVector(Vector3.up);
				interSett.ScaleMultiplier = 1/worldUp.magnitude;
			}
			else {
				interSett.ScaleMultiplier = 1;
			}

			////
			
			//TODO: handle cursor-switching
		}

	}

}
