using Hover.Cast.Custom;
using Hover.Cast.State;
using UnityEngine;
using Hover.Common.State;

namespace Hover.Cast.Display.Default {

	/*================================================================================================*/
	public abstract class UiBaseIconRenderer : UiSelectRenderer {

		private GameObject vIcon;

		private int vPrevTextSize;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract Texture2D GetIconTexture();

		/*--------------------------------------------------------------------------------------------*/
		protected virtual Vector3 GetIconScale() {
			float s = vSettings.TextSize*0.75f*ArcCanvasScale;
			return new Vector3(s, s, 1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(ArcState pArcState, IBaseItemState pItemState,
														float pArcAngle, ItemVisualSettings pSettings) {
			base.Build(pArcState, pItemState, pArcAngle, pSettings);

			vIcon = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vIcon.name = "Icon";
			vIcon.transform.SetParent(gameObject.transform, false);
			vIcon.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vIcon.renderer.sharedMaterial.color = Color.clear;
			vIcon.renderer.sharedMaterial.mainTexture = GetIconTexture();
			vIcon.transform.localRotation = vLabel.CanvasLocalRotation;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			Color color = vSettings.ArrowIconColor;
			color.a *= (vItemState.MaxHighlightProgress*0.75f + 0.25f)*vMainAlpha;

			vIcon.renderer.sharedMaterial.color = color;

			if ( vSettings.TextSize != vPrevTextSize ) {
				vPrevTextSize = vSettings.TextSize;

				float inset = vLabel.TextH;

				vLabel.SetInset(!vArcState.IsLeft, inset);

				vIcon.transform.localPosition = 
					new Vector3(0, 0, 1+(vLabel.CanvasW-inset*0.666f)*ArcCanvasScale);
				vIcon.transform.localScale = GetIconScale();
			}
		}

	}

}
