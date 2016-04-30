using UnityEngine;
using UnityEngine.UI;

namespace Hover.Board.Renderers.Elements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(RawImage))]
	public class HoverRendererIcon : MonoBehaviour {

		public enum IconOffset {
			None,
			CheckOuter,
			CheckInner,
			RadioOuter,
			RadioInner,
			Parent,
			Slider,
			Sticky
		}

		public bool ControlledByRenderer { get; set; }
		
		public IconOffset IconType = IconOffset.CheckOuter;

		[Range(0.01f, 1)]
		public float CanvasScale = 0.02f;
		
		[Range(0, 100)]
		public float SizeX = 10;

		[Range(0, 100)]
		public float SizeY = 10;
		
		[Range(0, 0.01f)]
		public float Inset = 0.002f;

		[HideInInspector]
		[SerializeField]
		private bool vIsBuilt;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RawImage ImageComponent {
			get { return GetComponent<RawImage>(); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !vIsBuilt ) {
				BuildIcon();
				vIsBuilt = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !ControlledByRenderer ) {
				UpdateAfterRenderer();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterRenderer() {
			RawImage icon = ImageComponent;
			RectTransform rectTx = icon.rectTransform;
			const float w = 1/8f;
			const float h = 1;

			icon.uvRect = new Rect((int)IconType*w+Inset, Inset, w-Inset*2, h-Inset*2);

			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SizeX/CanvasScale);
			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SizeY/CanvasScale);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildIcon() {
			RawImage icon = ImageComponent;
			icon.material = Resources.Load<Material>("Materials/HoverRendererStandardIconsMaterial");
			icon.color = Color.white;
			icon.raycastTarget = false;
		}

	}

}
