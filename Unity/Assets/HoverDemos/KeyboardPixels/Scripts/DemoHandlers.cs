using Hover.Core.Cursors;
using Hover.Core.Items.Types;
using Hover.InterfaceModules.Cast;
using Hover.InterfaceModules.Key;
using UnityEngine;

namespace HoverDemos.KeyboardPixels {

	/*================================================================================================*/
	[RequireComponent(typeof(DemoEnvironment))]
	public class DemoHandlers : MonoBehaviour {

		public HoverCursorDataProvider CursorDataProvider;
		public HoverInteractionSettings InteractionSettings;
		public HoverkeyInterface Hoverkey;
		public GameObject HoverkeyMain;
		public GameObject HoverkeySplit;
		public HovercastInterface Hovercast;

		private bool vAllowThumb;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoEnvironment Enviro {
			get {
				return GetComponent<DemoEnvironment>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			HoverkeySplit.SetActive(false);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			UpdateCursorCapabilities();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void HandleKeySelected(IItemDataSelectable pItemData, HoverkeyItemLabels pLabels) {
			if ( pLabels.ActionType == HoverkeyItemLabels.KeyActionType.Character ) {
				char letter = (Hoverkey.IsInShiftMode && pLabels.HasShiftLabel ?
					pLabels.ShiftLabel[0] : pLabels.DefaultLabel[0]);
				Enviro.AddLetter(letter);
				return;
			}

			if ( pLabels.DefaultKey == KeyCode.Backspace ) {
				Enviro.RemoveLatestLetter();
				return;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetSplitMode(IItemDataSelectable<bool> pItemData) {
			bool isSplit = pItemData.Value;

			HoverkeyMain.SetActive(!isSplit);
			HoverkeySplit.SetActive(isSplit);
			Hoverkey.RefreshKeyLists();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetFastSelect(IItemDataSelectable<bool> pItemData) {
			InteractionSettings.SelectionMilliseconds = (pItemData.Value ? 200 : 400);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetAllowThumb(IItemDataSelectable<bool> pItemData) {
			vAllowThumb = pItemData.Value;
			UpdateCursorCapabilities();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorCapabilities() {
			HovercastActiveDirection castActDir = Hovercast.GetComponent<HovercastActiveDirection>();
			bool isCastActive = (Hovercast.IsOpen && castActDir.ChildForActivation.activeSelf);

			SetCursorCapability(CursorType.LeftIndex, !isCastActive);
			SetCursorCapability(CursorType.LeftThumb, (!isCastActive && vAllowThumb));
			SetCursorCapability(CursorType.RightThumb, vAllowThumb);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetCursorCapability(CursorType pType, bool pEnable) {
			CursorDataProvider
				.GetCursorDataForInput(pType)
				.SetCapability(pEnable ? CursorCapabilityType.Full : CursorCapabilityType.None);
		}

	}

}
