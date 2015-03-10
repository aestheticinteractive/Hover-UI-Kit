using Hoverboard.Core.Display.Default;
using UnityEngine;

namespace Hoverboard.Demo {

	/*================================================================================================*/
	public class DemoTextField : MonoBehaviour {

		private UiLabel vTextEntry;


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
			lblBgObj.transform.localScale = new Vector3(10, 1, 1);
			lblBgObj.renderer.material = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			lblBgObj.renderer.material.color = new Color(0.1f, 0.1f, 0.1f, 0.5f);
			lblBgObj.renderer.material.renderQueue -= 300;

			vTextEntry = lblObj.AddComponent<UiLabel>();
			vTextEntry.SetSize(10, 1);
			vTextEntry.FontName = "Tahoma";
			vTextEntry.FontSize = 50;
			vTextEntry.Color = new Color(1, 1, 1, 0.5f);
			vTextEntry.Alpha = 1;
			vTextEntry.Label = "";
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddLetter(char pLetter) {
			vTextEntry.Label += pLetter;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void RemoveLatestLetter() {
			string text = vTextEntry.Label;
			vTextEntry.Label = text.Substring(0, text.Length-1);
		}

	}

}
