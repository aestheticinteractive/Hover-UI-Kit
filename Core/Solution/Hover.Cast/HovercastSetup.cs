using Hover.cast.Custom.Standard;
using Hover.Cast.Custom;
using Hover.Cast.Display;
using Hover.Cast.Input;
using Hover.Cast.Items;
using Hover.Cast.State;
using Hover.Common.Util;
using Hover.Cursor;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cast {

	/*================================================================================================*/
	public class HovercastSetup : MonoBehaviour {

		private const string CursorPlaneKey = "Hovercast.UiMenu";

		public HovercastItemHierarchy ItemHierarchy;
		public HovercursorSetup Hovercursor;
		public HovercastItemVisualSettings DefaultItemVisualSettings;
		public HovercastPalmVisualSettings DefaultPalmVisualSettings;
		public HovercastInteractionSettings InteractionSettings;
		public HovercastInput Input;
		
		private HovercastState vState;
		private UiMenu vUiMenu;
		private IInteractionPlaneState vCursorPlaneState;


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

			Input = UnityUtil.FindComponentOrFail(Input, prefix);

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

			vCursorPlaneState = Hovercursor.State.AddPlane(
				CursorPlaneKey, menuObj.transform, Vector3.up);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vState == null ) {
				return;
			}

			vCursorPlaneState.IsEnabled = (vState.Menu.DisplayStrength > 0);

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
