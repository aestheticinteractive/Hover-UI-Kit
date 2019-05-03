using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hover.Core.Renderers.CanvasElements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(RawImage))]
	public class HoverIcon : TreeUpdateableBehavior {

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
			NavigateIn,
			NavigateOut,
			Slider,
			Sticky
		}

		public ISettingsControllerMap Controllers { get; private set; }

		[SerializeField]
		[DisableWhenControlled(DisplaySpecials=true)]
		[FormerlySerializedAs("IconType")]
		private IconOffset _IconType = IconOffset.CheckOuter;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0.0001f, RangeMax=1)]
		[FormerlySerializedAs("CanvasScale")]
		private float _CanvasScale = 0.0002f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		[FormerlySerializedAs("SizeX")]
		private float _SizeX = 0.1f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		[FormerlySerializedAs("SizeY")]
		private float _SizeY = 0.1f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=0.01f)]
		[FormerlySerializedAs("UvInset")]
		private float _UvInset = 0.002f;

		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IconOffset IconType {
			get => _IconType;
			set => this.UpdateValueWithTreeMessage(ref _IconType, value, "IconType");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float CanvasScale {
			get => _CanvasScale;
			set => this.UpdateValueWithTreeMessage(ref _CanvasScale, value, "CanvasScale");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SizeX {
			get => _SizeX;
			set => this.UpdateValueWithTreeMessage(ref _SizeX, value, "SizeX");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SizeY {
			get => _SizeY;
			set => this.UpdateValueWithTreeMessage(ref _SizeY, value, "SizeY");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float UvInset {
			get => _UvInset;
			set => this.UpdateValueWithTreeMessage(ref _UvInset, value, "UvInset");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverIcon() {
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
		public override void TreeUpdate() {
			RawImage icon = ImageComponent;
			RectTransform rectTx = icon.rectTransform;
			const float w = 1f/9;
			const float h = 1;

			icon.uvRect = new Rect((int)IconType*w+UvInset, UvInset, w-UvInset*2, h-UvInset*2);

			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SizeX/CanvasScale);
			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SizeY/CanvasScale);
			Controllers.TryExpireControllers();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildIcon() {
			RawImage icon = ImageComponent;
			icon.material = Resources.Load<Material>("Materials/HoverStandardIconsMaterial");
			icon.color = Color.white;
			icon.raycastTarget = false;
		}

	}

}
