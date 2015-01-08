using System;
using HandMenu.Display;
using HandMenu.State;
using UnityEngine;
using UnityEngine.UI;

namespace HandMenu.Demo {

	/*================================================================================================*/
	public class DemoPointRenderer : MonoBehaviour, IMenuPointRenderer {

		private const int Width = 240;
		private const int Height = 40;
		private const float Scale = 0.0004f;

		private MenuHandState vHand;
		private MenuPointState vPoint;
		private float vAnimAlpha;

		private GameObject vHold;
		private GameObject vBackground;
		private GameObject vHighlight;
		private GameObject vSelect;
		private GameObject vCanvasGroupObj;
		private GameObject vCanvasObj;
		private GameObject vTextObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(MenuHandState pHand, MenuPointState pPoint) {
			vHand = pHand;
			vPoint = pPoint;

			////

			vHold = new GameObject("Hold");
			vHold.transform.parent = gameObject.transform;

			////

			vBackground = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vBackground.transform.parent = vHold.transform;
			vBackground.name = "Background";
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBackground.renderer.sharedMaterial.renderQueue -= 3;

			vHighlight = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vHighlight.transform.parent = vBackground.transform;
			vHighlight.name = "Highlight";
			vHighlight.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vHighlight.renderer.sharedMaterial.renderQueue -= 2;

			vSelect = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vSelect.transform.parent = vBackground.transform;
			vSelect.name = "Select";
			vSelect.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vSelect.renderer.sharedMaterial.renderQueue -= 1;

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
			rect.pivot = new Vector2((vHand.IsLeft ? 0 : 1), 0.5f);

			////

			vTextObj = new GameObject("Text");
			vTextObj.transform.parent = vCanvasObj.transform;

			Text text = vTextObj.AddComponent<Text>();
			text.font = Resources.Load<Font>("GothamNarrowBook");
			text.fontSize = 24;
			text.alignment = (vHand.IsLeft ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight);

			rect = vTextObj.GetComponent<RectTransform>();
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 8, Width-16);
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 4, Height-8);

			////
			
			int mult = (vHand.IsLeft ? -1 : 1);
			Quaternion rot = Quaternion.FromToRotation(Vector3.back, Vector3.down)*
				Quaternion.FromToRotation(Vector3.down, Vector3.right);

			vHold.transform.localPosition = new Vector3(0, 0, 0.03f*mult);

			vBackground.transform.localPosition = new Vector3(0, 0, Width*Scale/2f*mult);
			vBackground.transform.localRotation = rot;
			vBackground.transform.localScale = new Vector3(Width*Scale, Height*Scale, 1);

			vCanvasObj.transform.localRotation = rot;
			vCanvasObj.transform.localScale = Vector3.one*Scale;

			vPoint.SetSelectionExtension(Width*Scale);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !vPoint.IsActive ) {
				return;
			}

			float alpha = vHand.Strength*vPoint.Strength*vAnimAlpha;
			float high = (float)Math.Pow(vPoint.HighlightProgress, 5);
			float select = 1-(float)Math.Pow(1-vPoint.SelectionProgress, 2);

			vCanvasGroupObj.GetComponent<CanvasGroup>().alpha = alpha;
			vBackground.renderer.sharedMaterial.color = new Color(0, 0, 0, 0.333f*alpha);

			vHighlight.transform.localScale = new Vector3(high, 1, 1);
			vHighlight.transform.localPosition = new Vector3(-0.5f+high/2f, 0, 0);
			vHighlight.renderer.sharedMaterial.color = new Color(0.1f, 0.5f, 0.9f, high*alpha);

			vSelect.transform.localScale = new Vector3(select, 1, 1);
			vSelect.transform.localPosition = new Vector3(-0.5f+select/2f, 0, 0);
			vSelect.renderer.sharedMaterial.color = new Color(0.1f, 1.0f, 0.2f, select*alpha);

			vTextObj.GetComponent<Text>().text = vPoint.Data.Label;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			vAnimAlpha = (pFadeIn ? pProgress : 1-pProgress);
		}

	}

}
