using System;
using System.Collections.Generic;
using Henu.State;
using UnityEngine;
using UnityEngine.UI;

namespace Henu.Display.Default {

	/*================================================================================================*/
	public class UiArcSegmentRenderer : MonoBehaviour, IUiArcSegmentRenderer {

		protected const int Width = 240;
		protected const int Height = 42;
		protected const float Scale = 0.002f;

		protected ArcState vArcState;
		protected ArcSegmentState vSegState;
		protected float vAngle0;
		protected float vAngle1;
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
		public virtual void Build(ArcState pArcState, ArcSegmentState pSegState,
																		float pAngle0, float pAngle1) {
			vArcState = pArcState;
			vSegState = pSegState;
			vAngle0 = pAngle0;
			vAngle1 = pAngle1-0.003f;

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
				Quaternion.FromToRotation(Vector3.down, Vector3.left);
			vCanvasGroupObj.transform.localScale = Vector3.one*Scale;

			vCanvasObj = new GameObject("Canvas");
			vCanvasObj.transform.SetParent(vCanvasGroupObj.transform, false);
			
			Canvas canvas = vCanvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.WorldSpace;

			RectTransform rect = vCanvasObj.GetComponent<RectTransform>();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);
			rect.pivot = new Vector2((vArcState.IsLeft ? 0 : 1), 0.5f);

			////

			vTextObj = new GameObject("Text");
			vTextObj.transform.SetParent(vCanvasObj.transform, false);

			Text text = vTextObj.AddComponent<Text>();
			text.font = Resources.Load<Font>("Tahoma");
			text.fontSize = 26;
			text.alignment = (vArcState.IsLeft ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight);

			rect = vTextObj.GetComponent<RectTransform>();
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 10, Width-20);
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 4, Height-8);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = GetArcAlpha(vArcState)*vAnimAlpha;

			float high = vSegState.HighlightProgress;
			float select = 1-(float)Math.Pow(1-vSegState.SelectionProgress, 1.5f);

			vCanvasGroupObj.GetComponent<CanvasGroup>().alpha = vMainAlpha;
			vBackground.renderer.sharedMaterial.color = new Color(0.1f, 0.1f, 0.1f, 0.5f*vMainAlpha);

			BuildMesh(vHighlight.GetComponent<MeshFilter>().mesh, high);
			vHighlight.renderer.sharedMaterial.color = new Color(0.1f, 0.5f, 0.9f, high*vMainAlpha);

			BuildMesh(vSelect.GetComponent<MeshFilter>().mesh, select);
			vSelect.renderer.sharedMaterial.color = new Color(0.1f, 1.0f, 0.2f, select*vMainAlpha);

			vTextObj.GetComponent<Text>().text = vSegState.NavItem.Label;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			float a = 1-(float)Math.Pow(1-pProgress, 3);
			vAnimAlpha = (pFadeIn ? a : 1-a);
		}

		/*--------------------------------------------------------------------------------------------*/
		public float CalculateCursorDistance(Vector3 pCursorPosition) {
			float sqrMagMin = float.MaxValue;

			foreach ( Vector3 v in vSelectionPoints ) {
				float sqrMag = (v-pCursorPosition).sqrMagnitude;
				sqrMagMin = Math.Min(sqrMagMin, sqrMag);
			}

			return (float)Math.Sqrt(sqrMagMin);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildMesh(Mesh pMesh, float pThickness) {
			int steps = (int)Math.Round(Math.Max(2, (vAngle1-vAngle0)/Math.PI*60));
			MeshUtil.BuildRingMesh(pMesh, 1, 1+0.5f*pThickness, vAngle0, vAngle1, steps);
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
