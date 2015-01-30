using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hovercast.Core.Display.Default {

	/*================================================================================================*/
	public class UiLabel : MonoBehaviour {

		public float TextPadW { get; private set; }
		public float TextPadH { get; private set; }
		public float CanvasW { get; private set; }
		public float CanvasH { get; private set; }
		public float TextW { get; private set; }
		public float TextH { get; private set; }

		private int vFontSize;
		private bool? vIsLeft;
		private string vFontName;
		private float vLeftInset;
		private float vRightInset;

		protected float vTextScale;

		protected GameObject vCanvasGroupObj;
		protected GameObject vCanvasObj;
		protected GameObject vTextObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			TextPadH = 0;
			CanvasW = UiSelectRenderer.ArcCanvasThickness;
			vTextScale = UiSelectRenderer.ArcCanvasScale;
			vFontSize = -1;

			////

			vCanvasGroupObj = new GameObject("CanvasGroup");
			vCanvasGroupObj.transform.SetParent(gameObject.transform, false);
			vCanvasGroupObj.AddComponent<CanvasGroup>();
			vCanvasGroupObj.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.back, Vector3.down)*
				Quaternion.FromToRotation(Vector3.down, Vector3.left);
			vCanvasGroupObj.transform.localScale = Vector3.one*vTextScale;

			vCanvasObj = new GameObject("Canvas");
			vCanvasObj.transform.SetParent(vCanvasGroupObj.transform, false);

			Canvas canvas = vCanvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.WorldSpace;

			vTextObj = new GameObject("Text");
			vTextObj.transform.SetParent(vCanvasObj.transform, false);
			vTextObj.AddComponent<Text>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetInset(bool pLeftSide, float pInset) {
			if ( pLeftSide && Math.Abs(pInset-vLeftInset) > 0.001f ) {
				vLeftInset = pInset;
				UpdateSizing();
			}
			else if ( !pLeftSide && Math.Abs(pInset-vRightInset) > 0.001f ) {
				vRightInset = pInset;
				UpdateSizing();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected void UpdateSizing() {
			TextPadW = vFontSize*0.6f;
			CanvasH = vFontSize*1.25f+TextPadH*2;
			TextW = CanvasW-TextPadW*2;
			TextH = CanvasH-TextPadH*2;

			vTextObj.GetComponent<Text>().fontSize = vFontSize;

			RectTransform rect = vCanvasObj.GetComponent<RectTransform>();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CanvasW);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CanvasH);

			rect = vTextObj.GetComponent<RectTransform>();
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
				TextPadW+vLeftInset, TextW-vLeftInset-vRightInset);
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, TextPadH, TextH);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public int FontSize {
			get {
				return vFontSize;
			}
			set {
				if ( value == vFontSize ) {
					return;
				}

				vFontSize = value;
				UpdateSizing();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public string Label {
			get {
				return vTextObj.GetComponent<Text>().text;
			}
			set {
				vTextObj.GetComponent<Text>().text = value;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsLeft {
			get {
				return (vIsLeft ?? false);
			}
			set {
				if ( value == vIsLeft ) {
					return;
				}

				vIsLeft = value;
				vCanvasObj.GetComponent<RectTransform>().pivot = new Vector2((value ? 0 : 1), 0.5f);
				vTextObj.GetComponent<Text>().alignment = 
					(value ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float Alpha {
			get {
				return vCanvasGroupObj.GetComponent<CanvasGroup>().alpha;
			}
			set {
				CanvasGroup group = vCanvasGroupObj.GetComponent<CanvasGroup>();

				if ( Math.Abs(value-group.alpha) < 0.001f ) {
					return;
				}

				group.alpha = value;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public string FontName {
			get {
				return vFontName;
			}
			set {
				if ( value == vFontName ) {
					return;
				}

				vFontName = value;
				vTextObj.GetComponent<Text>().font = Resources.Load<Font>(vFontName);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public Color Color {
			get {
				return vTextObj.GetComponent<Text>().color;
			}
			set {
				Text text = vTextObj.GetComponent<Text>();

				if ( value == text.color ) {
					return;
				}

				text.color = value;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public Quaternion CanvasLocalRotation {
			get {
				return vCanvasGroupObj.transform.localRotation;
			}
		}

	}

}
