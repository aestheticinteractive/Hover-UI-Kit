using System;
using Hover.Cast.Renderers.Standard.Types;
using Hover.Cast.State;
using Hover.Common.Renderers;
using Hover.Common.State;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hover.Cast.Renderers.Standard {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HovercastStandardRendererController : MonoBehaviour, IHovercastItemRenderer {
		
		public float ArcAngle { get; set; }
		public bool? AnimIsFadingIn { get; set; }
		public int AnimDirection { get; set; }
		public float AnimProgress { get; set; }
		
		private readonly RendererTypeSelector vRendererTypes;
		private IHovercastItemRenderer vActiveRenderer;

		private HovercastStandardRendererSettings vSettings;
		private IHovercastMenuState vMenuState;
		private IBaseItemState vItemState;
		private bool vShouldRebuild;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastStandardRendererController() {
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
			vShouldRebuild = true;
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastStandardRendererSettings Settings {
			get {
				return vSettings;
			}
			set {
				if ( value == vSettings ) {
					return;
				}

				vSettings = value;
				vShouldRebuild = true;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IHovercastMenuState MenuState {
			get {
				return vMenuState;
			}
			set {
				if ( value == vMenuState ) {
					return;
				}

				vMenuState = value;
				vShouldRebuild = true;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IBaseItemState ItemState {
			get {
				return vItemState;
			}
			set {
				if ( value == vItemState ) {
					return;
				}

				vItemState = value;
				vShouldRebuild = true;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			Rebuild();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			vActiveRenderer.ArcAngle = ArcAngle;
			vActiveRenderer.AnimIsFadingIn = AnimIsFadingIn;
			vActiveRenderer.AnimDirection = AnimDirection;
			vActiveRenderer.AnimProgress = AnimProgress;

			if ( !Application.isPlaying ) {
				LateUpdate();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void LateUpdate() {
			if ( vShouldRebuild ) {
				Rebuild();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetDepthHint(int pDepthHint) {
			vActiveRenderer.SetDepthHint(pDepthHint);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateHoverPoints(IBaseItemPointsState pPointsState, Vector3 pCursorWorldPos) {
			vActiveRenderer.UpdateHoverPoints(pPointsState, pCursorWorldPos);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void Rebuild() {
			if ( vActiveRenderer != null ) {
				Destroy((Object)vActiveRenderer);
			}

			Type componentType = vRendererTypes.GetItemRendererType(ItemState.Item);

			vActiveRenderer = (IHovercastItemRenderer)gameObject.AddComponent(componentType);
		}

	}

}
