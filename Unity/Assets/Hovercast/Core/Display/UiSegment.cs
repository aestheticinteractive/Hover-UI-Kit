using System;
using Hovercast.Core.Custom;
using Hovercast.Core.Navigation;
using Hovercast.Core.State;
using UnityEngine;

namespace Hovercast.Core.Display {

	/*================================================================================================*/
	public class UiSegment : MonoBehaviour {

		public float ArcAngle { get; private set; }

		private ArcState vArcState;
		private SegmentState vSegState;

		private Transform vCursorBaseTx;
		private float vSlideDegrees;
		private Vector3 vSlideDir0;
		private Vector3 vCursorWorldPos;

		private IUiSegmentRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArcState, SegmentState pSegState, float pArcAngle, 
																			ICustomSegment pCustom) {
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

			Type rendType = pCustom.GetSegmentRenderer(vSegState.NavItem);
			SegmentSettings sett = pCustom.GetSegmentSettings(vSegState.NavItem);

			var rendObj = new GameObject("Renderer");
			rendObj.transform.SetParent(gameObject.transform, false);

			vRenderer = (IUiSegmentRenderer)rendObj.AddComponent(rendType);
			vRenderer.Build(vArcState, vSegState, pArcAngle, sett);
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
