using System;
using Henu.Navigation;
using Henu.Settings;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiArcSegment : MonoBehaviour {

		private ArcState vArcState;
		private ArcSegmentState vSegState;

		private Transform vCursorBaseTx;
		private float vDegreesFull;
		private Vector3 vSlideDir0;
		private Vector3 vCursorWorldPos;

		private IUiArcSegmentRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, ArcSegmentState pSegState, float pAngle0, float pAngle1, 
																				ISettings pSettings) {
			vArcState = pArcState;
			vSegState = pSegState;

			vSegState.SetCursorDistanceFunction(CalcCursorDistance);

			vCursorBaseTx = GameObject.Find("HandController").transform;
			vDegreesFull = (pAngle1-pAngle0)/(float)Math.PI*180;
			vSlideDir0 = MeshUtil.GetRingPoint(1, pAngle0);

			////

			Type rendType = pSettings.GetUiArcSegmentRendererType(vSegState.NavItem);
			ArcSegmentSettings colors = pSettings.GetArcSegmentSettings(vSegState.NavItem);

			var rendObj = new GameObject("Renderer");
			rendObj.transform.SetParent(gameObject.transform, false);

			vRenderer = (IUiArcSegmentRenderer)rendObj.AddComponent(rendType);
			vRenderer.Build(vArcState, vSegState, pAngle0, pAngle1, colors);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			if ( vSegState.NavItem.Type == NavItem.ItemType.Slider ) {
				UpdateSliderValue();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private float CalcCursorDistance(Vector3 pCursorPos) {
			vCursorWorldPos = vCursorBaseTx.TransformPoint(pCursorPos);
			float dist = vRenderer.CalculateCursorDistance(vCursorWorldPos);
			return gameObject.transform.TransformVector(Vector3.up*dist).magnitude;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		internal void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress) {
			vSegState.SetIsAnimating(pProgress < 1);
			vRenderer.HandleChangeAnimation(pFadeIn, pDirection, pProgress);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderValue() {
			if ( !vSegState.NavItem.Selected ) {
				return;
			}

			Vector3 cursorRel = gameObject.transform.InverseTransformPoint(vCursorWorldPos);
			cursorRel.y = 0;

			Vector3 cursorDir = cursorRel.normalized;
			Quaternion diff = Quaternion.FromToRotation(vSlideDir0, cursorDir);

			float cursorDeg;
			Vector3 cursorAxis;
			diff.ToAngleAxis(out cursorDeg, out cursorAxis);

			((NavItemSlider)vSegState.NavItem).CurrentValue = cursorDeg/vDegreesFull;
		}

	}

}
