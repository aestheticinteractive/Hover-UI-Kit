using System;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.CanvasElements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasGroup))]
	public class HoverCanvas : MonoBehaviour, ISettingsController, ITreeUpdateable {
		
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";

		public enum CanvasAlignmentType {
			Left,
			Center,
			Right,
			TextLeftAndIconRight,
			TextRightAndIconLeft,
			Custom
		}
		
		public enum IconSizeType {
			FontSize,
			ThreeQuartersFontSize,
			OneAndHalfFontSize,
			DoubleFontSize,
			Custom
		}
		
		public ISettingsControllerMap Controllers { get; private set; }
		
		[DisableWhenControlled(DisplaySpecials=true)]
		public HoverLabel Label;
		public HoverIcon IconOuter;
		public HoverIcon IconInner;
		
		[DisableWhenControlled(RangeMin=0.0001f)]
		public float Scale = 0.0002f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float SizeX = 0.1f;

		[DisableWhenControlled(RangeMin=0)]
		public float SizeY = 0.1f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float PaddingX = 0.005f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float PaddingY = 0.005f;
		
		[DisableWhenControlled]
		public CanvasAlignmentType Alignment = CanvasAlignmentType.Left;

		[DisableWhenControlled]
		public IconSizeType IconSize = IconSizeType.FontSize;

		[DisableWhenControlled]
		public bool UseMirrorLayout = false;

		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;

		private Canvas vPrevCanvas;
		private CanvasGroup vPrevCanvasGroup;
		private HoverLabel vPrevLabel;
		private HoverIcon vPrevIconOuter;
		private HoverIcon vPrevIconInner;
		private float vPrevScale;
		private float vPrevSizeX;
		private float vPrevSizeY;
		private float vPrevPaddingX;
		private float vPrevPaddingY;
		private CanvasAlignmentType vPrevAlignment;
		private IconSizeType vPrevIconSize;
		private bool vPrevUseMirrorLayout;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverCanvas() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Canvas CanvasComponent {
			get { return GetComponent<Canvas>(); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public CanvasGroup CanvasGroupComponent {
			get { return GetComponent<CanvasGroup>(); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float UnscaledPaddedSizeX {
			get { return SizeX-PaddingX*2; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float UnscaledPaddedSizeY {
			get { return SizeY-PaddingY*2; }
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !_IsBuilt ) {
				BuildElements();
				_IsBuilt = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			bool didChange = (
				vPrevCanvas != CanvasComponent ||
				vPrevCanvasGroup != CanvasGroupComponent ||
				vPrevLabel != Label ||
				vPrevIconOuter != IconOuter ||
				vPrevIconInner != IconInner ||
				vPrevScale != Scale ||
				vPrevSizeX != SizeX ||
				vPrevSizeY != SizeY ||
				vPrevPaddingX != PaddingX ||
				vPrevPaddingY != PaddingY ||
				vPrevAlignment != Alignment ||
				vPrevIconSize != IconSize ||
				vPrevUseMirrorLayout != UseMirrorLayout
			);

			if ( !didChange ) {
				return;
			}

			UpdateCanvasComponent();
			UpdateScale();
			UpdateActiveStates();
			UpdateIconSizeSettings();
			UpdateCanvasAlignmentSettings();
			Controllers.TryExpireControllers();

			vPrevCanvas = CanvasComponent;
			vPrevCanvasGroup = CanvasGroupComponent;
			vPrevLabel = Label;
			vPrevIconOuter = IconOuter;
			vPrevIconInner = IconInner;
			vPrevScale = Scale;
			vPrevSizeX = SizeX;
			vPrevSizeY = SizeY;
			vPrevPaddingX = PaddingX;
			vPrevPaddingY = PaddingY;
			vPrevAlignment = Alignment;
			vPrevIconSize = IconSize;
			vPrevUseMirrorLayout = UseMirrorLayout;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildElements() {
			CanvasComponent.renderMode = RenderMode.WorldSpace;
			CanvasComponent.sortingOrder = 1;
			
			Label = BuildLabel();
			IconOuter = BuildIcon("IconOuter");
			IconInner = BuildIcon("IconInner");
			
			IconOuter.IconType = HoverIcon.IconOffset.RadioOuter;
			IconInner.IconType = HoverIcon.IconOffset.RadioInner;

			IconInner.ImageComponent.color = new Color(1, 1, 1, 0.7f);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverLabel BuildLabel() {
			var labelGo = new GameObject("Label");
			labelGo.transform.SetParent(gameObject.transform, false);
			return labelGo.AddComponent<HoverLabel>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverIcon BuildIcon(string pName) {
			var iconGo = new GameObject(pName);
			iconGo.transform.SetParent(gameObject.transform, false);
			return iconGo.AddComponent<HoverIcon>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCanvasComponent() {
			Canvas canvas = CanvasComponent;
			RectTransform rectTx = canvas.GetComponent<RectTransform>();

			gameObject.transform.localScale = Vector3.one*Scale;
			
			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, UnscaledPaddedSizeX/Scale);
			rectTx.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, UnscaledPaddedSizeY/Scale);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateScale() {
			Label.Controllers.Set(HoverLabel.CanvasScaleName, this);
			IconOuter.Controllers.Set(HoverIcon.CanvasScaleName, this);
			IconInner.Controllers.Set(HoverIcon.CanvasScaleName, this);
			
			Label.CanvasScale = Scale;
			IconOuter.CanvasScale = Scale;
			IconInner.CanvasScale = Scale;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateActiveStates() {
			bool isLabelActive = (!string.IsNullOrEmpty(Label.TextComponent.text));
			bool isIconOuterActive = (IconOuter.IconType != HoverIcon.IconOffset.None);
			bool isIconInnerActive = (IconInner.IconType != HoverIcon.IconOffset.None);

			Label.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			IconOuter.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			IconInner.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);

			RendererUtil.SetActiveWithUpdate(Label, isLabelActive);
			RendererUtil.SetActiveWithUpdate(IconOuter, isIconOuterActive);
			RendererUtil.SetActiveWithUpdate(IconInner, isIconInnerActive);
		}
				
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateIconSizeSettings() {
			if ( IconSize == IconSizeType.Custom ) {
				return;
			}

			IconOuter.Controllers.Set(HoverIcon.SizeXName, this);
			IconOuter.Controllers.Set(HoverIcon.SizeYName, this);
			IconInner.Controllers.Set(HoverIcon.SizeXName, this);
			IconInner.Controllers.Set(HoverIcon.SizeYName, this);

			float fontSize = Label.TextComponent.fontSize*Label.CanvasScale;
			
			switch ( IconSize ) {
				case IconSizeType.FontSize:
					IconOuter.SizeX = fontSize;
					break;
					
				case IconSizeType.ThreeQuartersFontSize:
					IconOuter.SizeX = fontSize*0.75f;
					break;
					
				case IconSizeType.OneAndHalfFontSize:
					IconOuter.SizeX = fontSize*1.5f;
					break;
					
				case IconSizeType.DoubleFontSize:
					IconOuter.SizeX = fontSize*2;
					break;
			}
			
			IconOuter.SizeY = IconOuter.SizeX;
			IconInner.SizeX = IconOuter.SizeX;
			IconInner.SizeY = IconOuter.SizeY;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCanvasAlignmentSettings() {
			if ( Alignment == CanvasAlignmentType.Custom ) {
				return;
			}

			Vector3 labelLocalScale = Label.transform.localScale;
			float fontSize = Label.TextComponent.fontSize*Label.CanvasScale/2;
			float iconAvailW = UnscaledPaddedSizeX-IconOuter.SizeX;
			float iconPad = IconOuter.SizeX*0.2f;
			float iconShiftX = 0;
			float iconShiftY = 0;
			float labelInsetL = 0;
			float labelInsetR = 0;
			float labelInsetT = 0;
			TextAnchor labelAlign;
			
			switch ( Alignment ) { //icon
				case CanvasAlignmentType.Left:
				case CanvasAlignmentType.TextRightAndIconLeft:
					iconShiftX = -0.5f*iconAvailW;
					labelInsetL = IconOuter.SizeX+iconPad;
					break;
					
				case CanvasAlignmentType.Center:
					iconShiftY = (Label.TextComponent.text.Length == 0 ? 0 : fontSize/2+iconPad);
					labelInsetT = IconOuter.SizeY/2+iconPad;
					break;
					
				case CanvasAlignmentType.Right:
				case CanvasAlignmentType.TextLeftAndIconRight:
					iconShiftX = 0.5f*iconAvailW;
					labelInsetR = IconOuter.SizeX+iconPad;
					break;
					
				default:
					throw new Exception("Unhandled alignment: "+Alignment);
			}
			
			switch ( Alignment ) { //label
				case CanvasAlignmentType.Left:
				case CanvasAlignmentType.TextLeftAndIconRight:
					labelAlign = (UseMirrorLayout ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft);
					break;
					
				case CanvasAlignmentType.Center:
					labelAlign = TextAnchor.MiddleCenter;
					break;
					
				case CanvasAlignmentType.Right:
				case CanvasAlignmentType.TextRightAndIconLeft:
					labelAlign = (UseMirrorLayout ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight);
					break;
					
				default:
					throw new Exception("Unhandled alignment: "+Alignment);
			}

			Label.Controllers.Set(SettingsControllerMap.TransformLocalPosition, this);
			Label.Controllers.Set(SettingsControllerMap.TransformLocalScale+".x", this);
			Label.Controllers.Set(HoverLabel.SizeXName, this);
			Label.Controllers.Set(HoverLabel.SizeYName, this);
			Label.Controllers.Set(SettingsControllerMap.TextAlignment, this);
			IconOuter.Controllers.Set(SettingsControllerMap.TransformLocalPosition, this);
			IconInner.Controllers.Set(SettingsControllerMap.TransformLocalPosition, this);

			labelLocalScale.x = (UseMirrorLayout ? -1 : 1);
			Label.transform.localScale = labelLocalScale;

			if ( !IconOuter.gameObject.activeSelf && !IconInner.gameObject.activeSelf ) {
				iconShiftX = 0;
				iconShiftY = 0;
				labelInsetL = 0;
				labelInsetR = 0;
				labelInsetT = 0;
			}
			
			var labelLocalPos = new Vector3((labelInsetL-labelInsetR)/2, -labelInsetT, 0);
			var iconLocalPos = new Vector3(iconShiftX, iconShiftY, 0);
			TextAnchor prevAlign = Label.TextComponent.alignment;
			int prevVertAlign = (int)prevAlign/3;
			int newHorizAlign = (int)labelAlign%3;

			Label.SizeX = UnscaledPaddedSizeX-labelInsetL-labelInsetR;
			Label.SizeY = UnscaledPaddedSizeY-labelInsetT;
			
			Label.TextComponent.alignment = (TextAnchor)(newHorizAlign+prevVertAlign*3);
			
			Label.transform.localPosition = labelLocalPos/Scale;
			IconOuter.transform.localPosition = iconLocalPos/Scale;
			IconInner.transform.localPosition = IconOuter.transform.localPosition;
		}

	}

}
