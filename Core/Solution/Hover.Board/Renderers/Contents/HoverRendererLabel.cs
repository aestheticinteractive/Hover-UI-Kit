using UnityEngine;
using UnityEngine.UI;

namespace Hover.Board.Renderers.Contents {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(Text))]
	public class HoverRendererLabel : MonoBehaviour {

		public bool ControlledByRenderer { get; set; }
		
		[Range(0.01f, 1)]
		public float CanvasScale = 0.02f;
		
		[Range(0, 100)]
		public float SizeX = 10;

		[Range(0, 100)]
		public float SizeY = 10;

		[HideInInspector]
		[SerializeField]
		private bool vIsBuilt;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Text TextComponent {
			get { return GetComponent<Text>(); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !vIsBuilt ) {
				BuildText();
				vIsBuilt = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			RectTransform rectTx = TextComponent.rectTransform;
			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SizeX/CanvasScale);
			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SizeY/CanvasScale);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildText() {
			Text text = TextComponent;
			text.text = "Label";
			text.font = Resources.Load<Font>("Fonts/Tahoma");
			text.fontSize = 40;
			text.lineSpacing = 0.75f;
			text.color = Color.white;
			text.alignment = TextAnchor.MiddleCenter;
			text.raycastTarget = false;
			text.horizontalOverflow = HorizontalWrapMode.Wrap;
			text.verticalOverflow = VerticalWrapMode.Overflow;
		}

	}

}
