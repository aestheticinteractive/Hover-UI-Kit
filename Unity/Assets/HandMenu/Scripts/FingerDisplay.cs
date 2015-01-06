using System;
using UnityEngine;
using UnityEngine.UI;

namespace HandMenu {

	/*================================================================================================*/
	public class FingerDisplay : MonoBehaviour {

		public bool IsLeft;
		public Func<FingerData> GetCurrentData;
		public float HandAlpha;

		private const int Width = 200;
		private const int Height = 40;
		private const float Scale = 0.0004f;

		private GameObject vHold;
		private GameObject vBackground;
		private GameObject vCanvasGroupObj;
		private GameObject vCanvasObj;
		private GameObject vTextObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vHold = new GameObject("Hold");
			vHold.transform.parent = gameObject.transform;

			////

			vBackground = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vBackground.transform.parent = vHold.transform;
			vBackground.name = "Background";
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Transparent/Diffuse"));
			vBackground.renderer.sharedMaterial.renderQueue -= 1;

			////

			vCanvasGroupObj = new GameObject("CanvasGroup");
			vCanvasGroupObj.transform.parent = vHold.transform;
			vCanvasGroupObj.AddComponent<CanvasGroup>();

			////

			vCanvasObj = new GameObject("Canvas");
			vCanvasObj.transform.parent = vCanvasGroupObj.transform;
			
			Canvas canvas = vCanvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.WorldSpace;

			RectTransform rect = vCanvasObj.GetComponent<RectTransform>();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);

			////

			vTextObj = new GameObject("Text");
			vTextObj.transform.parent = vCanvasObj.transform;

			Text text = vTextObj.AddComponent<Text>();
			text.font = Resources.Load<Font>("GothamNarrowBook");
			text.fontSize = 24;
			text.text = gameObject.name;

			rect = vTextObj.GetComponent<RectTransform>();
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 8, Width-16);
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 4, Height-8);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			RectTransform rect = vCanvasObj.GetComponent<RectTransform>();
			rect.pivot = new Vector2((IsLeft ? 0 : 1), 0.5f);

			Text text = vTextObj.GetComponent<Text>();
			text.alignment = (IsLeft ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight);
			
			////

			Quaternion rot = Quaternion.FromToRotation(Vector3.back, Vector3.down)*
				Quaternion.FromToRotation(Vector3.down, Vector3.right);
			int mult = (IsLeft ? -1 : 1);

			vHold.transform.localPosition = new Vector3(0, 0, 0.03f*mult);

			vBackground.transform.localPosition = new Vector3(0, 0.001f, Width*Scale/2f*mult);
			vBackground.transform.localRotation = rot;
			vBackground.transform.localScale = new Vector3(Width*Scale, Height*Scale, 1);

			vCanvasObj.transform.localRotation = rot;
			vCanvasObj.transform.localScale = Vector3.one*Scale;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			FingerData data = GetCurrentData();

			if ( data == null ) {
				return;
			}

			float alpha = HandAlpha*(float)Math.Pow(data.Extension, 2);

			gameObject.transform.localPosition = data.Position;
			gameObject.transform.localRotation = data.Rotation;
			//gameObject.transform.localScale = new Vector3(1, 1, Math.Min(1, alpha*1.2f));

			if ( !IsLeft ) {
				gameObject.transform.localRotation *= 
					Quaternion.FromToRotation(Vector3.left, Vector3.right);
			}

			vCanvasGroupObj.GetComponent<CanvasGroup>().alpha = alpha;
			vBackground.renderer.sharedMaterial.color = new Color(0, 0, 0, 0.333f*alpha);
		}

	}

}
