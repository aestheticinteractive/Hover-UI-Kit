using System;
using Hover.Cast.State;
using Hover.Common.Display;
using Hover.Common.Items;
using Hover.Common.Renderers;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Cast.Renderers.Standard.Types {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HovercastStandardItemRenderer : MonoBehaviour, IHovercastItemRenderer {
		
		public const float Thickness = 0.5f;
		public const float InnerRadius = 1;
		public const float OuterRadius = InnerRadius+Thickness;
		public const float ArcCanvasThickness = 250;
		public const float ArcCanvasScale = 0.002f;
		
		public IHovercastMenuState MenuState { get; set; }
		public IBaseItemState ItemState { get; set; }

		public HovercastStandardRendererSettings Settings {
			get { return vSettings; }
			set { vSettings = value; }
		}

		public float ArcAngle { get; set; }
		public bool? AnimIsFadingIn { get; set; }
		public int AnimDirection { get; set; }
		public float AnimProgress { get; set; }

		protected float vMainAlpha;
		protected float vAnimAlpha;
		protected UiHoverMeshSlice vHoverSlice;
		protected UiLabel vLabel;

		private readonly RendererTypeSelector vRendererTypes;
		private IHovercastItemRenderer vActiveRenderer;

		[SerializeField]
		private HovercastStandardRendererSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastStandardItemRenderer() {
			vRendererTypes = new RendererTypeSelector(
				typeof(UiItemCheckboxRenderer),
				typeof(UiItemParentRenderer),
				typeof(UiItemRadioRenderer),
				typeof(UiItemSelectRenderer),
				typeof(UiItemSliderRenderer),
				typeof(UiItemStickyRenderer),
				typeof(UiItemSliderRenderer)
			);

			vSettings = new HovercastStandardRendererSettings();
		}

			
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			vHoverSlice = new UiHoverMeshSlice(gameObject);
			
			var labelObj = new GameObject("Label");
			labelObj.transform.SetParent(gameObject.transform, false);
			labelObj.transform.localPosition = new Vector3(0, 0, 1);
			labelObj.transform.localRotation = Quaternion.FromToRotation(Vector3.back, Vector3.right);
			
			vLabel = labelObj.AddComponent<UiLabel>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void OnEnable() {
			if ( vLabel != null ) {
				vLabel.Alpha = 0;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vHoverSlice.UpdateSize(InnerRadius, OuterRadius, ArcAngle);
			
			vLabel.AlignLeft = MenuState.IsOnLeftSide;
			vLabel.gameObject.transform.localScale = 
				new Vector3((MenuState.IsOnLeftSide ? 1 : -1), 1, 1);
			
			////
			
			if ( AnimIsFadingIn != null ) {
				vAnimAlpha = (float)Math.Pow(1-AnimProgress, 3);
				vAnimAlpha = (AnimIsFadingIn == true ? 1-vAnimAlpha : vAnimAlpha);
			}
			
			vMainAlpha = GetArcAlpha(MenuState)*vAnimAlpha;

			if ( !ItemState.Item.IsEnabled || !ItemState.Item.IsAncestryEnabled ) {
				vMainAlpha *= 0.333f;
			}
			
			////

			ISelectableItem selItem = (ISelectableItem)ItemState.Item;
			float high = ItemState.MaxHighlightProgress;
			bool showEdge = DisplayUtil.IsEdgeVisible(ItemState);
			float edge = (showEdge ? high : 0);
			float select = 1-(float)Math.Pow(1-ItemState.SelectionProgress, 1.5f);
			float selectAlpha = select;

			if ( selItem != null && selItem.IsStickySelected ) {
				selectAlpha = 1;
			}

			Color colBg = vSettings.BackgroundColor;
			Color colEdge = vSettings.EdgeColor;
			Color colHigh = vSettings.HighlightColor;
			Color colSel = vSettings.SelectionColor;

			colBg.a *= vMainAlpha;
			colEdge.a *= edge*vMainAlpha;
			colHigh.a *= high*vMainAlpha;
			colSel.a *= selectAlpha*vMainAlpha;

			vHoverSlice.UpdateBackground(colBg);
			vHoverSlice.UpdateEdge(colEdge);
			vHoverSlice.UpdateHighlight(colHigh, high);
			vHoverSlice.UpdateSelect(colSel, select);

			if ( vSettings.TextSize != vLabel.FontSize ) {
				vLabel.SetSize(ArcCanvasThickness*ArcCanvasScale, 
					vSettings.TextSize*1.5f*ArcCanvasScale, vSettings.TextSize*0.6f, ArcCanvasScale);
			}

			vLabel.Alpha = vMainAlpha;
			vLabel.Font = vSettings.TextFont;
			vLabel.FontSize = vSettings.TextSize;
			vLabel.Color = vSettings.TextColor;
			vLabel.Label = ItemState.Item.Label;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetDepthHint(int pDepthHint) {
			vHoverSlice.SetDepthHint(pDepthHint);
			vLabel.SetDepthHint(pDepthHint);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateHoverPoints(IBaseItemPointsState pPointsState, 
																			Vector3 pCursorWorldPos) {
			vHoverSlice.UpdateHoverPoints(pPointsState);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static float GetArcAlpha(IHovercastMenuState pMenuState) {
			float alpha = 1-(float)Math.Pow(1-pMenuState.DisplayStrength, 2);
			alpha -= (float)Math.Pow(pMenuState.NavBackStrength, 2)*0.5f;
			return Math.Max(0, alpha);
		}

	}

}
