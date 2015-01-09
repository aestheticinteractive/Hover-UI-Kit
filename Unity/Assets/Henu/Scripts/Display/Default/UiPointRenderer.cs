using System;
using Henu.State;
using UnityEngine;
using UnityEngine.UI;

namespace Henu.Display.Default {

	/*================================================================================================*/
	public class UiPointRenderer : MonoBehaviour, IUiMenuPointRenderer {

		protected const int Width = 240;
		protected const int Height = 40;
		protected const float Scale = 0.0004f;

		protected MenuHandState vHand;
		protected MenuPointState vPoint;
		protected float vOverallAlpha;
		protected float vAnimAlpha;

		protected GameObject vHold;
		protected GameObject vBackground;
		protected GameObject vHighlight;
		protected GameObject vSelect;
		protected GameObject vCanvasGroupObj;
		protected GameObject vCanvasObj;
		protected GameObject vTextObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(MenuHandState pHand, MenuPointState pPoint) {
			vHand = pHand;
			vPoint = pPoint;

			////

			vHold = new GameObject(GetType().Name);
			vHold.transform.parent = gameObject.transform;

			////

			vBackground = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vBackground.transform.parent = vHold.transform;
			vBackground.name = "Background";
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBackground.renderer.sharedMaterial.renderQueue -= 300;
			vBackground.renderer.sharedMaterial.color = Color.clear;

			vHighlight = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vHighlight.transform.parent = vBackground.transform;
			vHighlight.name = "Highlight";
			vHighlight.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vHighlight.renderer.sharedMaterial.renderQueue -= 200;
			vHighlight.renderer.sharedMaterial.color = Color.clear;

			vSelect = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vSelect.transform.parent = vBackground.transform;
			vSelect.name = "Select";
			vSelect.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vSelect.renderer.sharedMaterial.renderQueue -= 100;
			vSelect.renderer.sharedMaterial.color = Color.clear;

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
			text.font = Resources.Load<Font>("Tahoma");
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
		public virtual void Update() {
			if ( !vPoint.IsActive ) {
				return;
			}

			vOverallAlpha = 1-(float)Math.Pow(1-vHand.Strength*vPoint.Strength*vAnimAlpha, 2);

			float high = vPoint.HighlightProgress;
			float select = 1-(float)Math.Pow(1-vPoint.SelectionProgress, 1.5f);

			vCanvasGroupObj.GetComponent<CanvasGroup>().alpha = vOverallAlpha;
			vBackground.renderer.sharedMaterial.color = new Color(0.1f, 0.1f, 0.1f, 0.5f*vOverallAlpha);

			vHighlight.transform.localScale = new Vector3(high, 1, 1);
			vHighlight.transform.localPosition = new Vector3(-0.5f+high/2f, 0, 0);
			vHighlight.renderer.sharedMaterial.color = new Color(0.1f, 0.5f, 0.9f, high*vOverallAlpha);

			vSelect.transform.localScale = new Vector3(select, 1, 1);
			vSelect.transform.localPosition = new Vector3(-0.5f+select/2f, 0, 0);
			vSelect.renderer.sharedMaterial.color = new Color(0.1f, 1.0f, 0.2f, select*vOverallAlpha);

			vTextObj.GetComponent<Text>().text = vPoint.NavItem.Label;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			float a = 1-(float)Math.Pow(1-pProgress, 3);
			vAnimAlpha = (pFadeIn ? a : 1-a);
		}

	}

}
