using System;
using HandMenu.State;
using UnityEngine;
using UnityEngine.UI;

namespace HandMenu.Display {

	/*================================================================================================*/
	public class MenuPointDisplay : MonoBehaviour {

		public MenuHandState Hand { get; set; }
		public MenuPointState Point { get; set; }

		private const int Width = 240;
		private const int Height = 40;
		private const float Scale = 0.0004f;

		private GameObject vHold;
		private GameObject vBackground;
		private GameObject vSelectFill;
		private GameObject vCanvasGroupObj;
		private GameObject vCanvasObj;
		private GameObject vTextObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vHold = new GameObject("Hold");
			vHold.transform.parent = gameObject.transform;

			////

			vBackground = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vBackground.transform.parent = vHold.transform;
			vBackground.name = "Background";
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBackground.renderer.sharedMaterial.renderQueue -= 2;

			vSelectFill = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vSelectFill.transform.parent = vBackground.transform;
			vSelectFill.name = "SelectFill";
			vSelectFill.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vSelectFill.renderer.sharedMaterial.renderQueue -= 1;

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
			rect.pivot = new Vector2((Hand.IsLeft ? 0 : 1), 0.5f);

			////

			vTextObj = new GameObject("Text");
			vTextObj.transform.parent = vCanvasObj.transform;

			Text text = vTextObj.AddComponent<Text>();
			text.font = Resources.Load<Font>("GothamNarrowBook");
			text.fontSize = 24;
			text.alignment = (Hand.IsLeft ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight);
			text.text = gameObject.name;

			rect = vTextObj.GetComponent<RectTransform>();
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 8, Width-16);
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 4, Height-8);

			////
			
			int mult = (Hand.IsLeft ? -1 : 1);
			Quaternion rot = Quaternion.FromToRotation(Vector3.back, Vector3.down)*
				Quaternion.FromToRotation(Vector3.down, Vector3.right);

			vHold.transform.localPosition = new Vector3(0, 0, 0.03f*mult);

			vBackground.transform.localPosition = new Vector3(0, 0, Width*Scale/2f*mult);
			vBackground.transform.localRotation = rot;
			vBackground.transform.localScale = new Vector3(Width*Scale, Height*Scale, 1);

			vCanvasObj.transform.localRotation = rot;
			vCanvasObj.transform.localScale = Vector3.one*Scale;

			Point.SetSelectionExtension(Width*Scale);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !Point.IsActive ) {
				return;
			}
			
			float handAlpha = Math.Max(0, (Hand.PalmTowardEyes-0.7f)/0.3f);
			float alpha = (float)Math.Pow(handAlpha*Point.Strength, 2);
			float select = (float)Math.Pow(Point.SelectionProgress, 5);

			Transform tx = gameObject.transform;
			tx.localPosition = Point.Position;
			tx.localRotation = Point.Rotation;
			//tx.localScale = new Vector3(1, 1, Math.Min(1, alpha*1.2f));

			if ( !Hand.IsLeft ) {
				tx.localRotation *= Quaternion.FromToRotation(Vector3.left, Vector3.right);
			}

			vCanvasGroupObj.GetComponent<CanvasGroup>().alpha = alpha;
			vBackground.renderer.sharedMaterial.color = new Color(0, 0, 0, 0.333f*alpha);

			vSelectFill.transform.localScale = new Vector3(select, 1, 1);
			vSelectFill.transform.localPosition = new Vector3(-0.5f+select/2f, 0, 0);
			vSelectFill.renderer.sharedMaterial.color = new Color(0.1f, 0.6f, 0.9f, select*alpha);
		}

	}

}
