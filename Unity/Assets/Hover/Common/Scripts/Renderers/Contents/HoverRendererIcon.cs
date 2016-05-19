using Hover.Common.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Hover.Common.Renderers.Contents {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(RawImage))]
	public class HoverRendererIcon : MonoBehaviour, ITreeUpdateable {
		
		public const string IconTypeName = "IconType";
		public const string CanvasScaleName = "CanvasScale";
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";

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

		public ISettingsControllerMap Controllers { get; private set; }
		
		[DisableWhenControlled(DisplayMessage=true)]
		public IconOffset IconType = IconOffset.CheckOuter;

		[DisableWhenControlled(RangeMin=0.01f, RangeMax=1)]
		public float CanvasScale = 0.02f;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeX = 10;

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeY = 10;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=0.01f)]
		public float Inset = 0.002f;

		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererIcon() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RawImage ImageComponent {
			get { return GetComponent<RawImage>(); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !_IsBuilt ) {
				BuildIcon();
				_IsBuilt = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
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
