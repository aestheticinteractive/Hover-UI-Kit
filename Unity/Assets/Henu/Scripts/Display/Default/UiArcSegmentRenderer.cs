using System;
using System.Collections.Generic;
using Henu.Settings;
using Henu.State;
using UnityEngine;
using UnityEngine.UI;

namespace Henu.Display.Default {

	/*================================================================================================*/
	public class UiArcSegmentRenderer : MonoBehaviour, IUiArcSegmentRenderer {

		public const float ArcCanvasThickness = 250;
		public const float ArcCanvasScale = 0.002f;

		protected ArcState vArcState;
		protected ArcSegmentState vSegState;
		protected float vAngle0;
		protected float vAngle1;
		protected ArcSegmentSettings vSettings;
		protected int vMeshSteps;

		protected float vTextPadW;
		protected float vTextPadH;
		protected float vCanvasW;
		protected float vCanvasH;
		protected float vTextW;
		protected float vTextH;
		protected float vTextScale;
		protected float vMainAlpha;
		protected float vAnimAlpha;

		protected GameObject vBackground;
		protected GameObject vHighlight;
		protected GameObject vSelect;
		protected GameObject vCanvasGroupObj;
		protected GameObject vCanvasObj;
		protected GameObject vTextObj;

		private List<Vector3> vSelectionPoints;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(ArcState pArcState, ArcSegmentState pSegState,																		float pAngle0, float pAngle1, ArcSegmentSettings pSettings) {
			vArcState = pArcState;
			vSegState = pSegState;
			vAngle0 = pAngle0+0.001f;
			vAngle1 = pAngle1-0.001f;
			vSettings = pSettings;
			vMeshSteps = (int)Math.Round(Math.Max(2, (vAngle1-vAngle0)/Math.PI*60));

			vTextPadW = vSettings.TextSize*0.6f;
			vTextPadH = 0;
			vCanvasW = ArcCanvasThickness;
			vCanvasH = vSettings.TextSize*1.25f+vTextPadH*2;
			vTextW = vCanvasW-vTextPadW*2;
			vTextH = vCanvasH-vTextPadH*2;
			vTextScale = ArcCanvasScale;

			////

			vBackground = new GameObject("Background");
			vBackground.transform.SetParent(gameObject.transform, false);
			vBackground.AddComponent<MeshFilter>();
			vBackground.AddComponent<MeshRenderer>();
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBackground.renderer.sharedMaterial.renderQueue -= 300;
			vBackground.renderer.sharedMaterial.color = Color.clear;

			vHighlight = new GameObject("Highlight");
			vHighlight.transform.SetParent(gameObject.transform, false);
			vHighlight.AddComponent<MeshFilter>();
			vHighlight.AddComponent<MeshRenderer>();
			vHighlight.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vHighlight.renderer.sharedMaterial.renderQueue -= 200;
			vHighlight.renderer.sharedMaterial.color = Color.clear;

			vSelect = new GameObject("Select");
			vSelect.transform.SetParent(gameObject.transform, false);
			vSelect.AddComponent<MeshFilter>();
			vSelect.AddComponent<MeshRenderer>();
			vSelect.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vSelect.renderer.sharedMaterial.renderQueue -= 10;
			vSelect.renderer.sharedMaterial.color = Color.clear;

			////

			Mesh bgMesh = vBackground.GetComponent<MeshFilter>().mesh;
			BuildMesh(bgMesh, 1);

			vSelectionPoints = new List<Vector3>();

			for ( int i = 1 ; i < bgMesh.vertices.Length ; i += 2 ) {
				vSelectionPoints.Add(bgMesh.vertices[i]);
			}

			////

			vCanvasGroupObj = new GameObject("CanvasGroup");
			vCanvasGroupObj.transform.SetParent(gameObject.transform, false);
			vCanvasGroupObj.AddComponent<CanvasGroup>();
			vCanvasGroupObj.transform.localPosition = new Vector3(0, 0, 1);
			vCanvasGroupObj.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.back, Vector3.down)*
				Quaternion.FromToRotation(Vector3.down, 
					(vArcState.IsLeft ? Vector3.left : Vector3.right));
			vCanvasGroupObj.transform.localScale = Vector3.one*vTextScale;

			vCanvasObj = new GameObject("Canvas");
			vCanvasObj.transform.SetParent(vCanvasGroupObj.transform, false);
			
			Canvas canvas = vCanvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.WorldSpace;

			RectTransform rect = vCanvasObj.GetComponent<RectTransform>();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vCanvasW);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vCanvasH);
			rect.pivot = new Vector2((vArcState.IsLeft ? 0 : 1), 0.5f);

			////

			vTextObj = new GameObject("Text");
			vTextObj.transform.SetParent(vCanvasObj.transform, false);

			Text text = vTextObj.AddComponent<Text>();
			text.font = Resources.Load<Font>(vSettings.TextFont);
			text.fontSize = vSettings.TextSize;
			text.color = vSettings.TextColor;
			text.alignment = (vArcState.IsLeft ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight);

			rect = vTextObj.GetComponent<RectTransform>();
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, vTextPadW, vTextW);
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, vTextPadH*0.75f, vTextH);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = GetArcAlpha(vArcState)*vAnimAlpha;

			float high = vSegState.HighlightProgress;
			float select = 1-(float)Math.Pow(1-vSegState.SelectionProgress, 1.5f);

			Color colBg = vSettings.BackgroundColor;
			Color colHigh = vSettings.HighlightColor;
			Color colSel = vSettings.SelectionColor;

			colBg.a *= vMainAlpha;
			colHigh.a *= high*vMainAlpha;
			colSel.a *= select*vMainAlpha;

			BuildMesh(vHighlight.GetComponent<MeshFilter>().mesh, high);
			BuildMesh(vSelect.GetComponent<MeshFilter>().mesh, select);

			vCanvasGroupObj.GetComponent<CanvasGroup>().alpha = vMainAlpha;
			vBackground.renderer.sharedMaterial.color = colBg;
			vHighlight.renderer.sharedMaterial.color = colHigh;
			vSelect.renderer.sharedMaterial.color = colSel;

			vTextObj.GetComponent<Text>().text = vSegState.NavItem.Label;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			float a = 1-(float)Math.Pow(1-pProgress, 3);
			vAnimAlpha = (pFadeIn ? a : 1-a);
		}

		/*--------------------------------------------------------------------------------------------*/
		public float CalculateCursorDistance(Vector3 pCursorWorldPosition) {
			float sqrMagMin = float.MaxValue;
			Vector3 relCursor = gameObject.transform.InverseTransformPoint(pCursorWorldPosition);

			foreach ( Vector3 v in vSelectionPoints ) {
				float sqrMag = (v-relCursor).sqrMagnitude;
				sqrMagMin = Math.Min(sqrMagMin, sqrMag);
			}

			return (float)Math.Sqrt(sqrMagMin);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildMesh(Mesh pMesh, float pThickness) {
			MeshUtil.BuildRingMesh(pMesh, 1, 1+0.5f*pThickness, vAngle0, vAngle1, vMeshSteps);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static float GetArcAlpha(ArcState pArcState) {
			float alpha = 1-(float)Math.Pow(1-pArcState.Strength, 2);
			alpha -= (float)Math.Pow(pArcState.GrabStrength, 2);
			return Math.Max(0, alpha);
		}

	}

}
