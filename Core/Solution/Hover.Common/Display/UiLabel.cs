using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hover.Common.Display {

	/*================================================================================================*/
	public class UiLabel : MonoBehaviour {

		public float TextPadW { get; private set; }
		public float TextPadH { get; private set; }
		public float CanvasW { get; private set; }
		public float CanvasH { get; private set; }
		public float TextW { get; private set; }
		public float TextH { get; private set; }

		private int vFontSize;
		private string vFontName;

		private GameObject vCanvasGroupObj;
		private GameObject vCanvasObj;
		private GameObject vTextObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			TextPadH = 0;
			vFontSize = -1;

			////

			vCanvasGroupObj = new GameObject("CanvasGroup");
			vCanvasGroupObj.transform.SetParent(gameObject.transform, false);
			vCanvasGroupObj.AddComponent<CanvasGroup>();
			vCanvasGroupObj.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.back, Vector3.down);

			vCanvasObj = new GameObject("Canvas");
			vCanvasObj.transform.SetParent(vCanvasGroupObj.transform, false);

			Canvas canvas = vCanvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.WorldSpace;

			vTextObj = new GameObject("Text");
			vTextObj.transform.SetParent(vCanvasObj.transform, false);
			
			Text text = vTextObj.AddComponent<Text>();
			text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetSize(float pWidth, float pHeight, float pCanvasScale) {
			vCanvasGroupObj.transform.localScale = Vector3.one*pCanvasScale;
			CanvasW = pWidth/pCanvasScale;
			CanvasH = pHeight/pCanvasScale;
			UpdateSizing();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSizing() {
			TextPadW = vFontSize*0.6f;
			TextW = CanvasW-TextPadW*2;
			TextH = CanvasH-TextPadH*2;

			vTextObj.GetComponent<Text>().fontSize = vFontSize;

			RectTransform rect = vCanvasObj.GetComponent<RectTransform>();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CanvasW);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CanvasH);

			rect = vTextObj.GetComponent<RectTransform>();
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, TextPadW, TextW);
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
				Text textComp = vTextObj.GetComponent<Text>();

				if ( textComp.text == value ) {
					return;
				}

				textComp.text = value;
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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Font FontObject {
			get {
				return vTextObj.GetComponent<Text>().font;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Texture2D Texture {
			get {
				return (Texture2D)vTextObj.GetComponent<Text>().mainTexture;
			}
		}

	}

}
