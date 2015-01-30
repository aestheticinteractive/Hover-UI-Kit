using System;
using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using Hovercast.Core.State;
using UnityEngine;

namespace Hovercast.Core.Display {

	/*================================================================================================*/
	public class UiArcSegment : MonoBehaviour {

		public float ArcAngle { get; private set; }

		private ArcState vArcState;
		private ArcSegmentState vSegState;

		private Transform vCursorBaseTx;
		private float vSlideDegrees;
		private Vector3 vSlideDir0;
		private Vector3 vCursorWorldPos;

		private IUiArcSegmentRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, ArcSegmentState pSegState, float pArcAngle, 
																				ISettings pSettings) {
			vArcState = pArcState;
			vSegState = pSegState;
			ArcAngle = pArcAngle;

			vSegState.SetCursorDistanceFunction(CalcCursorDistance);

			vCursorBaseTx = gameObject.transform.parent.parent.parent.parent; //HovercastSetup

			const float pi = (float)Math.PI;
			const float slideBufferAngle = pi/80f;

			vSlideDegrees = (pArcAngle-slideBufferAngle*2)/(float)Math.PI*180;
			vSlideDir0 = MeshUtil.GetRingPoint(1, -pArcAngle/2f+slideBufferAngle);

			////

			Type rendType = pSettings.GetUiArcSegmentRendererType(vSegState.NavItem);
			ArcSegmentSettings colors = pSettings.GetArcSegmentSettings(vSegState.NavItem);

			var rendObj = new GameObject("Renderer");
			rendObj.transform.SetParent(gameObject.transform, false);

			vRenderer = (IUiArcSegmentRenderer)rendObj.AddComponent(rendType);
			vRenderer.Build(vArcState, vSegState, pArcAngle, colors);
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
			if ( !vSegState.NavItem.IsStickySelected ) {
				return;
			}

			Vector3 cursorRel = gameObject.transform.InverseTransformPoint(vCursorWorldPos);
			cursorRel.y = 0;

			Vector3 cursorDir = cursorRel.normalized;
			Quaternion diff = Quaternion.FromToRotation(vSlideDir0, cursorDir);

			float cursorDeg;
			Vector3 cursorAxis;
			diff.ToAngleAxis(out cursorDeg, out cursorAxis);

			if ( cursorAxis.y < 0 ) {
				cursorDeg = 0;
			}

			((NavItemSlider)vSegState.NavItem).Value = cursorDeg/vSlideDegrees;
		}

	}

}
