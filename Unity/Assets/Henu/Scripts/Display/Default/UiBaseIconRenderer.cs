using Henu.Settings;
using Henu.State;
using UnityEngine;

namespace Henu.Display.Default {

	/*================================================================================================*/
	public abstract class UiBaseIconRenderer : UiSelectRenderer {

		private GameObject vArrow;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract Texture2D GetIconTexture();

		/*--------------------------------------------------------------------------------------------*/
		protected virtual Vector3 GetIconScale() {
			float scale = vSettings.TextSize*0.75f*vTextScale;
			return new Vector3(scale, scale, 1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(ArcState pArcState, ArcSegmentState pSegState,
										float pAngle0, float pAngle1, ArcSegmentSettings pSettings) {
			base.Build(pArcState, pSegState, pAngle0, pAngle1, pSettings);

			Vector3 scale = GetIconScale();
			scale.x *= (vArcState.IsLeft ? 1 : -1);

			float push = vTextH;
			float pos = 1+(vCanvasW-push/2f-vTextPadW/4f)*vTextScale;

			RectTransform.Edge edge = (vArcState.IsLeft ? 
				RectTransform.Edge.Right : RectTransform.Edge.Left);

			RectTransform rect = vTextObj.GetComponent<RectTransform>();
			rect.SetInsetAndSizeFromParentEdge(edge, push, vCanvasW-push-vTextPadW);

			vArrow = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vArrow.transform.SetParent(gameObject.transform, false);
			vArrow.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vArrow.renderer.sharedMaterial.color = Color.clear;
			vArrow.renderer.sharedMaterial.mainTexture = GetIconTexture();
			vArrow.transform.localPosition = new Vector3(0, 0, pos);
			vArrow.transform.localRotation = vCanvasGroupObj.transform.localRotation;
			vArrow.transform.localScale = scale;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			Color color = vSettings.ArrowIconColor;
			color.a *= (vSegState.HighlightProgress*0.75f + 0.25f)*vMainAlpha;

			vArrow.renderer.sharedMaterial.color = color;
		}

	}

}
