using System.Linq;
using Hover.Common.Display;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public class UiItemSliderTrackRenderer {

		protected readonly UiHoverMeshRectBg[] vTracks;
		protected readonly UiHoverMeshRectBg[] vFills;
		protected readonly UiHoverMeshRectBg[] vAllBgs;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public UiItemSliderTrackRenderer(GameObject pParent) {
			vTracks = new UiHoverMeshRectBg[4];
			vFills = new UiHoverMeshRectBg[2];

			for ( int i = 0 ; i < vTracks.Length ; i++ ) {
				vTracks[i] = new UiHoverMeshRectBg(pParent, "Track"+i);
			}

			for ( int i = 0 ; i < vFills.Length ; i++ ) {
				vFills[i] = new UiHoverMeshRectBg(pParent, "Fill"+i);
			}

			vAllBgs = vTracks.Concat(vFills).ToArray();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetDepthHint(int pDepthHint) {
			foreach ( UiHoverMeshRectBg bg in vAllBgs ) {
				bg.SetDepthHint(pDepthHint);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetColors(Color pTrackColor, Color pFillColor) {
			foreach ( UiHoverMeshRectBg track in vTracks ) {
				track.UpdateBackground(pTrackColor);
			}

			foreach ( UiHoverMeshRectBg fill in vFills ) {
				fill.UpdateBackground(pFillColor);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void UpdateSegments(DisplayUtil.TrackSegment[] pSegments,	
													DisplayUtil.TrackSegment[] pCuts, float pHeight) {
			DisplayUtil.TrackSegment[] slices = DisplayUtil.SplitTrackSegments(pSegments, pCuts);

			foreach ( UiHoverMeshRectBg bg in vAllBgs ) {
				bg.Show(false);
			}

			int trackI = 0;
			int fillI = 0;

			foreach ( DisplayUtil.TrackSegment slice in slices ) {
				UiHoverMeshRectBg bg;

				if ( slice.IsFill ) {
					bg = vFills[fillI++];
				}
				else {
					bg = vTracks[trackI++];
				}

				float width = slice.EndValue-slice.StartValue;
				float zeroShiftW = 0;
				float zeroShiftX = 0;

				if ( slice.IsZeroAtStart == true ) {
					zeroShiftW = UiHoverMeshRect.SizeInset*2;
					zeroShiftX = -zeroShiftW;
				}
				else if ( slice.IsZeroAtStart == false ) {
					zeroShiftW = UiHoverMeshRect.SizeInset*2;
				}

				width += zeroShiftW;

				bg.Show(true);
				bg.UpdateSize(width, pHeight);
				bg.Background.transform.localPosition = 
					Vector3.right*(slice.StartValue+width/2+zeroShiftX);
			}
		}

	}

}
