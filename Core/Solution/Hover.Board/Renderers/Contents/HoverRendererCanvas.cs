using System;
using Hover.Common;
using Hover.Common.Renderers;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Renderers.Contents {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasGroup))]
	public class HoverRendererCanvas : MonoBehaviour, ISettingsController {
		
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";
		public const string AlphaName = "Alpha";
		public const string RenderQueueName = "RenderQueue";

		public enum CanvasAlignmentType {
			Left,
			Center,
			Right,
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
		
		[DisableWhenControlled(DisplayMessage=true)]
		public HoverRendererLabel Label;
		public HoverRendererIcon IconOuter;
		public HoverRendererIcon IconInner;
		
		[Range(0.01f, 1)]
		[DisableWhenControlled]
		public float Scale = 0.02f;
		
		[Range(0, 100)]
		[DisableWhenControlled]
		public float SizeX = 10;

		[Range(0, 100)]
		[DisableWhenControlled]
		public float SizeY = 10;
		
		[Range(0, 50)]
		[DisableWhenControlled]
		public float PaddingX = 0.5f;
		
		[Range(0, 50)]
		[DisableWhenControlled]
		public float PaddingY = 0.5f;

		[Range(0, 1)]
		[DisableWhenControlled]
		public float Alpha = 1;
		
		[DisableWhenControlled]
		public CanvasAlignmentType Alignment = CanvasAlignmentType.Left;

		[DisableWhenControlled]
		public IconSizeType IconSize = IconSizeType.FontSize;

		[DisableWhenControlled]
		public int RenderQueue = 3001;
		
		[HideInInspector]
		[SerializeField]
		private bool vIsBuilt;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererCanvas() {
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
			if ( !vIsBuilt ) {
				BuildElements();
				vIsBuilt = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			UpdateCanvasComponent();
			UpdateGeneralSettings();
			UpdateActiveStates();
			UpdateIconSizeSettings();
			UpdateCanvasAlignmentSettings();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildElements() {
			CanvasComponent.renderMode = RenderMode.WorldSpace;
			
			Label = BuildLabel();
			IconOuter = BuildIcon("IconOuter");
			IconInner = BuildIcon("IconInner");
			
			IconOuter.IconType = HoverRendererIcon.IconOffset.RadioOuter;
			IconInner.IconType = HoverRendererIcon.IconOffset.RadioInner;

			IconInner.ImageComponent.color = new Color(1, 1, 1, 0.7f);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererLabel BuildLabel() {
			var labelGo = new GameObject("Label");
			labelGo.transform.SetParent(gameObject.transform, false);
			return labelGo.AddComponent<HoverRendererLabel>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererIcon BuildIcon(string pName) {
			var iconGo = new GameObject(pName);
			iconGo.transform.SetParent(gameObject.transform, false);
			return iconGo.AddComponent<HoverRendererIcon>();
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
		private void UpdateGeneralSettings() {
			Label.Controllers.Set(HoverRendererLabel.CanvasScaleName, this);
			Label.Controllers.Set(HoverRendererLabel.SizeXName, this);
			Label.Controllers.Set(HoverRendererLabel.SizeYName, this);
			Label.Controllers.Set("Text.alignment", this);
			Label.Controllers.Set("Text.material.renderQueue", this);
			
			IconOuter.Controllers.Set(HoverRendererIcon.CanvasScaleName, this);
			IconOuter.Controllers.Set(HoverRendererIcon.SizeXName, this);
			IconOuter.Controllers.Set(HoverRendererIcon.SizeYName, this);
			
			IconInner.Controllers.Set(HoverRendererIcon.CanvasScaleName, this);
			IconInner.Controllers.Set(HoverRendererIcon.SizeXName, this);
			IconInner.Controllers.Set(HoverRendererIcon.SizeYName, this);
			
			Label.CanvasScale = Scale;
			IconOuter.CanvasScale = Scale;
			IconInner.CanvasScale = Scale;
			
			CanvasGroupComponent.alpha = Alpha;

			Label.TextComponent.material.renderQueue = RenderQueue;
			IconOuter.ImageComponent.material.renderQueue = RenderQueue;
			IconInner.ImageComponent.material.renderQueue = RenderQueue;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateActiveStates() {
			bool isLabelActive = (!string.IsNullOrEmpty(Label.TextComponent.text));
			bool isIconOuterActive = (IconOuter.IconType != HoverRendererIcon.IconOffset.None);
			bool isIconInnerActive = (IconInner.IconType != HoverRendererIcon.IconOffset.None);

			RendererHelper.SetActiveWithUpdate(Label, isLabelActive);
			RendererHelper.SetActiveWithUpdate(IconOuter, isIconOuterActive);
			RendererHelper.SetActiveWithUpdate(IconInner, isIconInnerActive);
		}
				
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateIconSizeSettings() {
			if ( IconSize == IconSizeType.Custom ) {
				return;
			}
			
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
			
			const float iconVertShiftMult = -0.35f;
			
			float fontSize = Label.TextComponent.fontSize*Label.CanvasScale/2;
			float iconAvailW = UnscaledPaddedSizeX-IconOuter.SizeX;
			float iconPad = IconOuter.SizeX*0.2f;
			float iconShiftX = 0;
			float iconShiftY = 0;
			float labelInsetL = 0;
			float labelInsetR = 0;
			float labelInsetT = 0;
			TextAnchor labelAlign;
			
			switch ( Alignment ) {
				case CanvasAlignmentType.Left:
					iconShiftX = -0.5f*iconAvailW;
					iconShiftY = iconVertShiftMult*fontSize;
					labelInsetL = IconOuter.SizeX+iconPad;
					labelAlign = TextAnchor.MiddleLeft;
					break;
					
				case CanvasAlignmentType.Center:
					iconShiftY = (fontSize+iconPad)/2;
					labelInsetT = (IconOuter.SizeY+iconPad)/2;
					labelAlign = TextAnchor.MiddleCenter;
					break;
					
				case CanvasAlignmentType.Right:
					iconShiftX = 0.5f*iconAvailW;
					iconShiftY = iconVertShiftMult*fontSize;
					labelInsetR = IconOuter.SizeX+iconPad;
					labelAlign = TextAnchor.MiddleRight;
					break;
					
				default:
					throw new Exception("Unhandled alignment: "+Alignment);
			}
			
			if ( !IconOuter.gameObject.activeSelf && !IconInner.gameObject.activeSelf ) {
				iconShiftX = 0;
				iconShiftY = 0;
				labelInsetL = 0;
				labelInsetR = 0;
				labelInsetT = 0;
			}
			
			var labelLocalPos = new Vector3((labelInsetL-labelInsetR)/2, -labelInsetT, 0);
			var iconLocalPos = new Vector3(iconShiftX, iconShiftY, 0);
			
			Label.SizeX = UnscaledPaddedSizeX-labelInsetL-labelInsetR;
			Label.SizeY = UnscaledPaddedSizeY-labelInsetT;
			
			Label.TextComponent.alignment = labelAlign;
			
			Label.transform.localPosition = labelLocalPos/Scale;
			IconOuter.transform.localPosition = iconLocalPos/Scale;
			IconInner.transform.localPosition = IconOuter.transform.localPosition;
		}

	}

}
