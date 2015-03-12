using Hoverboard.Core.Custom;
using Hoverboard.Core.Display.Default;
using UnityEngine;

namespace Hoverboard.Demo {

	/*================================================================================================*/
	public class DemoTextField : MonoBehaviour {

		private UiLabel vTextEntry;
		private string vText;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			var lblObj = new GameObject("TextEntry");
			lblObj.transform.SetParent(gameObject.transform, false);
			lblObj.transform.localRotation = Quaternion.FromToRotation(Vector3.down, Vector3.back);
			lblObj.transform.localScale = Vector3.one*0.05f;

			var lblBgObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
			lblBgObj.transform.SetParent(lblObj.transform, false);
			lblBgObj.transform.localPosition = new Vector3(0, 0, 0.01f);
			lblBgObj.transform.localRotation = Quaternion.FromToRotation(Vector3.up, Vector3.back);
			lblBgObj.transform.localScale = new Vector3(6, 1, 1);
			lblBgObj.renderer.material = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			lblBgObj.renderer.material.color = new Color(0.1f, 0.1f, 0.1f, 0.666f);
			lblBgObj.renderer.material.renderQueue -= 300;

			Color green = HoverboardDefaultButton.Green;
			green.a *= 0.5f;

			vTextEntry = lblObj.AddComponent<UiLabel>();
			vTextEntry.SetSize(6, 1);
			vTextEntry.FontName = "Tahoma";
			vTextEntry.FontSize = 60;
			vTextEntry.Color = green;
			vTextEntry.Alpha = 1;
			vTextEntry.Label = "";

			ClearLetters();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddLetter(char pLetter) {
			vText += pLetter;
			UpdateLabel();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void RemoveLatestLetter() {
			vText = vText.Substring(0, vText.Length-1);
			UpdateLabel();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ClearLetters() {
			vText = "";
			UpdateLabel();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateLabel() {
			vTextEntry.Label = vText+"|";
		}

	}

}
