using System;
using UnityEngine;
using UnityEngine.UI;

namespace HandMenu {

	/*================================================================================================*/
	public class FingerDisplay : MonoBehaviour {

		public Func<FingerData> GetCurrentData;

		private const int Width = 200;
		private const int Height = 40;
		private const float Scale = 0.0004f;

		private GameObject vBackground;
		private GameObject vCanvasObj;
		private GameObject vTextObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vBackground = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vBackground.name = "Background";
			vBackground.renderer.sharedMaterial.shader = Shader.Find("Transparent/Diffuse");
			vBackground.renderer.sharedMaterial.color = new Color(0, 0, 0, 0.333f);
			vBackground.transform.parent = gameObject.transform;

			////

			vCanvasObj = new GameObject("Canvas");
			vCanvasObj.transform.parent = gameObject.transform;
			
			Canvas canvas = vCanvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.WorldSpace;

			RectTransform rect = vCanvasObj.GetComponent<RectTransform>();
			rect.pivot = new Vector2(0, 0.5f);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);

			////

			vTextObj = new GameObject("Text");
			vTextObj.transform.parent = vCanvasObj.transform;

			var text = vTextObj.AddComponent<Text>();
			text.font = Resources.Load<Font>("GothamNarrowBook");
			text.fontSize = 24;
			text.alignment = TextAnchor.MiddleLeft;
			text.text = gameObject.name;

			rect = vTextObj.GetComponent<RectTransform>();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			Quaternion rot = Quaternion.FromToRotation(Vector3.back, Vector3.down)*
				Quaternion.FromToRotation(Vector3.up, Vector3.right);

			vBackground.transform.localPosition = new Vector3(0, 0.001f, 0.03f+Width*0.0002f);
			vBackground.transform.localRotation = rot;
			vBackground.transform.localScale = new Vector3(Width*Scale, Height*Scale, 1);

			vCanvasObj.transform.localPosition = new Vector3(0, 0, 0.03f);
			vCanvasObj.transform.localRotation = rot;
			vCanvasObj.transform.localScale = Vector3.one*Scale;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			FingerData data = GetCurrentData();

			if ( data == null ) {
				return;
			}

			gameObject.transform.localPosition = data.Position;
			gameObject.transform.localRotation = Quaternion.FromToRotation(Vector3.back,data.Direction);

			//vTextObj.GetComponent<Text>().color = new Color(1, 1, 1, 0.1f);
		}

	}

}
