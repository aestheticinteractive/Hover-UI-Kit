using Henu.State;
using UnityEngine;

namespace Henu.Display.Default {

	/*================================================================================================*/
	public abstract class UiPointToggleIconRenderer : UiPointRenderer {

		public static float IconSize = 16;

		private Texture2D ToggleOuterTexture;
		private Texture2D ToggleInnerTexture;
		private GameObject vOuter;
		private GameObject vInner;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract string GetIconName();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(MenuHandState pHand, MenuPointState pPoint) {
			base.Build(pHand, pPoint);

			RectTransform rect = vTextObj.GetComponent<RectTransform>();
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, Height, Width-Height-8);

			////
			
			string iconName = GetIconName();
			
			vOuter = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vOuter.transform.parent = vHold.transform;
			vOuter.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vOuter.renderer.sharedMaterial.color = Color.clear;
			vOuter.renderer.sharedMaterial.mainTexture = Resources.Load<Texture2D>(iconName+"Outer");

			vInner = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vInner.transform.parent = vHold.transform;
			vInner.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vInner.renderer.sharedMaterial.color = Color.clear;
			vInner.renderer.sharedMaterial.mainTexture = Resources.Load<Texture2D>(iconName+"Inner");

			////
			
			int mult = (vHand.IsLeft ? -1 : 1);

			vOuter.transform.localPosition = new Vector3(0, 0, (Height/2f)*Scale*mult);
			vOuter.transform.localRotation = vBackground.transform.localRotation;
			vOuter.transform.localScale = new Vector3(-IconSize*Scale*mult, IconSize*Scale, 1);

			vInner.transform.localPosition = vOuter.transform.localPosition;
			vInner.transform.localRotation = vOuter.transform.localRotation;
			vInner.transform.localScale = vOuter.transform.localScale;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			if ( !vPoint.IsActive ) {
				return;
			}

			float alpha = vPoint.HighlightProgress*0.5f + 0.5f;

			vOuter.renderer.sharedMaterial.color = new Color(1, 1, 1, alpha*vOverallAlpha);
			vInner.renderer.sharedMaterial.color = new Color(1, 1, 1, alpha*vOverallAlpha);
			vInner.renderer.enabled = vPoint.Data.Selected;
		}

	}

}
