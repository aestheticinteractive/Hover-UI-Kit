using System;
using Hover.Core.Items.Types;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Hover.InterfaceModules.Key {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverkeyTextInput : MonoBehaviour {

		public HoverkeyInterface Hoverkey;
		public InputField InputField;

		public int CursorIndex = 0;

		[Multiline]
		public string TextInput = "";


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( Hoverkey == null ) {
				Hoverkey = UnityUtil.FindNearbyComponent<HoverkeyInterface>(gameObject);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			if ( Hoverkey == null ) {
				throw new Exception("The 'Hoverkey' property must not be null.");
			}

			Hoverkey.OnItemSelectedEvent.AddListener(HandleKeySelected);
			//Hoverkey.OnItemDeselectedEvent.AddListener(HandleKeyDeselected);
			//Hoverkey.OnItemToggledEvent.AddListener(HandleKeyToggled);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleKeySelected(IItemDataSelectable pItemData, HoverkeyItemLabels pLabels) {
			switch ( pLabels.ActionType ) {
				case HoverkeyItemLabels.KeyActionType.Character:
					AddCharacter(pLabels);
					break;

				case HoverkeyItemLabels.KeyActionType.Control:
					AddControl(pLabels);
					break;

				case HoverkeyItemLabels.KeyActionType.Navigation:
					AddNavigation(pLabels);
					break;
			}

			SendInputFieldEvent(pLabels);
		}

		/*--------------------------------------------------------------------------------------------* /
		private void HandleKeyToggled(IItemDataSelectable<bool> pArg0, HoverkeyItemLabels pArg1) {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------* /
		private void HandleKeyDeselected(IItemDataSelectable pArg0, HoverkeyItemLabels pArg1) {
			throw new NotImplementedException();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AddCharacter(HoverkeyItemLabels pLabels) {
			AddCharacter(Hoverkey.IsInShiftMode && pLabels.HasShiftLabel ?
				pLabels.ShiftLabel : pLabels.DefaultLabel);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void AddCharacter(string pChar) {
			TextInput = TextInput.Insert(CursorIndex, pChar);
			CursorIndex++;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void AddControl(HoverkeyItemLabels pLabels) {
			switch ( pLabels.DefaultKey ) {
				case KeyCode.Backspace:
					if ( CursorIndex > 0 ) {
						TextInput = TextInput.Substring(0, CursorIndex-1)+
							TextInput.Substring(CursorIndex, TextInput.Length-CursorIndex);
						CursorIndex--;
					}
					break;

				case KeyCode.Delete:
					if ( CursorIndex < TextInput.Length ) {
						TextInput = TextInput.Substring(0, CursorIndex)+
							TextInput.Substring(CursorIndex+1, TextInput.Length-CursorIndex-1);
					}
					break;

				case KeyCode.Return:
				case KeyCode.KeypadEnter:
					AddCharacter("\n");
					break;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void AddNavigation(HoverkeyItemLabels pLabels) {
			switch ( pLabels.DefaultKey ) {
				case KeyCode.LeftArrow:
					CursorIndex = Mathf.Max(0, CursorIndex-1);
					break;

				case KeyCode.RightArrow:
					CursorIndex = Mathf.Min(TextInput.Length, CursorIndex+1);
					break;

				case KeyCode.Home:
					CursorIndex = 0;
					break;

				case KeyCode.End:
					CursorIndex = TextInput.Length;
					break;

				case KeyCode.Tab:
					AddCharacter("\t");
					break;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void SendInputFieldEvent(HoverkeyItemLabels pLabels) {
			if ( InputField == null ) {
				return;
			}

			InputField.text = TextInput;
			InputField.caretPosition = CursorIndex;
			InputField.ForceLabelUpdate();

			/*
			//TODO: the following seems like the right way to do it, but isn't working as expected
			//http://docs.unity3d.com/540/Documentation/ScriptReference/UI.InputField.html
			//https://docs.unity3d.com/ScriptReference/Event.KeyboardEvent.html
			//https://docs.unity3d.com/Manual/ConventionalGameInput.html

			string keyName = (pLabels.DefaultKey+"").ToLower();

			switch ( pLabels.DefaultKey ) {
				case KeyCode.Keypad0:
				case KeyCode.Keypad1:
				case KeyCode.Keypad2:
				case KeyCode.Keypad3:
				case KeyCode.Keypad4:
				case KeyCode.Keypad5:
				case KeyCode.Keypad6:
				case KeyCode.Keypad7:
				case KeyCode.Keypad8:
				case KeyCode.Numlock:
				case KeyCode.KeypadDivide:
				case KeyCode.KeypadMultiply:
				case KeyCode.KeypadMinus:
				case KeyCode.KeypadPlus:
				case KeyCode.KeypadPeriod:
				case KeyCode.KeypadEnter:
					keyName = "["+keyName+"]";
					break;
			}

			if ( Hoverkey.IsInShiftMode ) {
				keyName = "#"+keyName;
				//keyName = keyName.ToUpper();
			}

			Debug.Log("KEY: "+keyName);
			InputField.ActivateInputField();
			InputField.ProcessEvent(Event.KeyboardEvent(keyName));
			InputField.ForceLabelUpdate();*/
		}

	}

}
