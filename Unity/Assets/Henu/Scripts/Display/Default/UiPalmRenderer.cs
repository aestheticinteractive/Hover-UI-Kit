using System;
using Henu.Settings;
using Henu.State;
using UnityEngine;
using UnityEngine.UI;

namespace Henu.Display.Default {

	/*================================================================================================*/
	public class UiPalmRenderer : MonoBehaviour, IUiPalmRenderer {

		protected ArcState vArcState;
		protected float vAngle0;
		protected float vAngle1;
		protected int vMeshSteps;

		protected float vInnerRadius;
		protected float vDiameter;
		protected float vTextScale;
		protected float vMainAlpha;
		private ArcSegmentSettings vSettings;

		protected GameObject vBackground;
		protected GameObject vCanvasGroupObj;
		protected GameObject vCanvasObj;
		protected GameObject vTextObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(ArcState pArcState, float pAngle0, float pAngle1) {
			vArcState = pArcState;
			vAngle0 = pAngle0;
			vAngle1 = pAngle1;
			vMeshSteps = (int)Math.Round(Math.Max(2, (vAngle1-vAngle0)/Math.PI*60));

			vInnerRadius = 0.1f;
			vDiameter = UiSelectRenderer.ArcCanvasThickness;
			vTextScale = UiSelectRenderer.ArcCanvasScale;

			bool isLeft = vArcState.IsLeft;

			////

			vBackground = new GameObject("Background");
			vBackground.transform.SetParent(gameObject.transform, false);
			vBackground.AddComponent<MeshFilter>();
			vBackground.AddComponent<MeshRenderer>();
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBackground.renderer.sharedMaterial.renderQueue -= 100;
			vBackground.renderer.sharedMaterial.color = Color.clear;

			BuildMesh(vBackground.GetComponent<MeshFilter>().mesh);

			////

			vCanvasGroupObj = new GameObject("CanvasGroup");
			vCanvasGroupObj.transform.SetParent(gameObject.transform, false);
			vCanvasGroupObj.AddComponent<CanvasGroup>();
			vCanvasGroupObj.transform.localPosition = new Vector3(0, 0, vInnerRadius);
			vCanvasGroupObj.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.back, Vector3.down)*
				Quaternion.FromToRotation(Vector3.down, Vector3.left);
			vCanvasGroupObj.transform.localScale = new Vector3((isLeft ? 1 : -1), 1, 1)*vTextScale;

			vCanvasObj = new GameObject("Canvas");
			vCanvasObj.transform.SetParent(vCanvasGroupObj.transform, false);
			
			Canvas canvas = vCanvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.WorldSpace;

			RectTransform rect = vCanvasObj.GetComponent<RectTransform>();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vDiameter);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vDiameter);
			rect.pivot = new Vector2((isLeft ? 0 : 1), 0.5f);

			////

			vTextObj = new GameObject("Text");
			vTextObj.transform.SetParent(vCanvasObj.transform, false);

			Text text = vTextObj.AddComponent<Text>();
			text.alignment = TextAnchor.MiddleCenter;

			rect = vTextObj.GetComponent<RectTransform>();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vDiameter);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vDiameter);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetSettings(ArcSegmentSettings pSettings) {
			vSettings = pSettings;

			Text text = vTextObj.GetComponent<Text>();
			text.font = Resources.Load<Font>(vSettings.TextFont);
			text.fontSize = vSettings.TextSize;
			text.color = vSettings.TextColor;
			text.text = vArcState.GetLevelTitle();
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = UiSelectRenderer.GetArcAlpha(vArcState);

			Color colBg = vSettings.BackgroundColor;
			colBg.a *= vMainAlpha;

			vCanvasGroupObj.GetComponent<CanvasGroup>().alpha = vMainAlpha;
			vBackground.renderer.sharedMaterial.color = colBg;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildMesh(Mesh pMesh) {
			MeshUtil.BuildRingMesh(pMesh, vInnerRadius, vInnerRadius+0.5f, vAngle0, vAngle1,vMeshSteps);
		}

	}

}
