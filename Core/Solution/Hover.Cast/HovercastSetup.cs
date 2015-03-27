using Hover.cast.Custom.Standard;
using Hover.Cast.Custom;
using Hover.Cast.Display;
using Hover.Cast.Input;
using Hover.Cast.Items;
using Hover.Cast.State;
using Hover.Common.Util;
using Hover.Cursor;
using UnityEngine;

namespace Hover.Cast {

	/*================================================================================================*/
	public class HovercastSetup : MonoBehaviour {

		private const string Domain = "Hovercast";

		public HovercastItemHierarchy ItemHierarchy;
		public HovercursorSetup Hovercursor;
		public HovercastItemVisualSettings DefaultItemVisualSettings;
		public HovercastPalmVisualSettings DefaultPalmVisualSettings;
		public HovercastInteractionSettings InteractionSettings;
		public HovercastInput Input;
		
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
			ItemHierarchy = UnityUtil.FindComponentOrFail(ItemHierarchy, Domain);
			Hovercursor = UnityUtil.FindComponentOrFail(Hovercursor, Domain);

			DefaultItemVisualSettings = UnityUtil.CreateComponent<HovercastItemVisualSettings,
				HovercastItemVisualSettingsStandard>(DefaultItemVisualSettings, gameObject, Domain);
			DefaultItemVisualSettings.IsDefaultSettingsComponent = true;

			DefaultPalmVisualSettings = UnityUtil.CreateComponent<HovercastPalmVisualSettings,
				HovercastPalmVisualSettingsStandard>(DefaultPalmVisualSettings, gameObject, Domain);
			DefaultPalmVisualSettings.IsDefaultSettingsComponent = true;

			InteractionSettings = UnityUtil.FindComponentOrCreate<HovercastInteractionSettings,
				HovercastInteractionSettings>(InteractionSettings, gameObject, Domain);

			Input = UnityUtil.FindComponentOrFail(Input, Domain);

			vState = new HovercastState(ItemHierarchy.GetRoot(), Hovercursor, 
				InteractionSettings.GetSettings(), Input, gameObject.transform);
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
			Hovercursor.State.AddDelegate(vState);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vState == null ) {
				return;
			}

			Input.UpdateInput();
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
		}

	}

}
