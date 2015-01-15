using System;
using Henu.Settings;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiArcSegment : MonoBehaviour {

		private ArcState vArcState;
		private ArcSegmentState vSegState;
		private Transform vCursorBaseTx;
		private IUiArcSegmentRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, ArcSegmentState pSegState, float pAngle0, float pAngle1, 
																				ISettings pSettings) {
			vArcState = pArcState;
			vSegState = pSegState;
			vCursorBaseTx = GameObject.Find("HandController").transform;
			vSegState.SetCursorDistanceFunction(CalcCursorDistance);

			////

			Type rendType = pSettings.GetUiArcSegmentRendererType(vSegState.NavItem);
			ArcSegmentSettings colors = pSettings.GetArcSegmentSettings(vSegState.NavItem);

			var rendObj = new GameObject("Renderer");
			rendObj.transform.SetParent(gameObject.transform, false);

			vRenderer = (IUiArcSegmentRenderer)rendObj.AddComponent(rendType);
			vRenderer.Build(vArcState, vSegState, pAngle0, pAngle1, colors);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private float CalcCursorDistance(Vector3 pCursorPos) {
			Vector3 worldCursor = vCursorBaseTx.TransformPoint(pCursorPos);
			float dist = vRenderer.CalculateCursorDistance(worldCursor);
			return gameObject.transform.TransformVector(Vector3.up*dist).magnitude;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		internal void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			vSegState.SetIsAnimating(pProgress < 1);
			vRenderer.HandleChangeAnimation(pFadeIn, pDirection, pProgress);
		}

	}

}
