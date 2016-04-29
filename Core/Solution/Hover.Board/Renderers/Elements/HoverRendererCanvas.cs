using UnityEngine;

namespace Hover.Board.Renderers.Elements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(Canvas))]
	public class HoverRendererCanvas : MonoBehaviour {

		public bool ControlledByRenderer { get; set; }
		public float CanvasSizeX { get; private set; }
		public float CanvasSizeY { get; private set; }
		
		[Range(0.01f, 1)]
		public float CanvasScale = 0.02f;
		
		[Range(0, 100)]
		public float SizeX = 10;

		[Range(0, 100)]
		public float SizeY = 10;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Canvas CanvasComponent {
			get { return GetComponent<Canvas>(); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			CanvasComponent.renderMode = RenderMode.WorldSpace;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !ControlledByRenderer ) {
				UpdateAfterRenderer();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterRenderer() {
			Canvas canvas = CanvasComponent;
			RectTransform rectTx = canvas.GetComponent<RectTransform>();

			gameObject.transform.localScale = Vector3.one*CanvasScale;

			CanvasSizeX = SizeX/CanvasScale;
			CanvasSizeY = SizeY/CanvasScale;

			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CanvasSizeX);
			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CanvasSizeY);
		}

	}

}
