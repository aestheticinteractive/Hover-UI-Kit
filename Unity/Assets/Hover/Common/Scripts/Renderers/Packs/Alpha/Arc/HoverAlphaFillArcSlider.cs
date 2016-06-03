using System.Collections.Generic;
using Hover.Common.Renderers.Shapes.Arc;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Packs.Alpha.Arc {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverAlphaFillArcSlider : HoverFillArcSlider {
	
		public const string AlphaName = "Alpha";
		
		public HoverAlphaMeshArcTrack[] Segments;

		public List<HoverAlphaMeshArcTrack> Ticks;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override HoverMeshArcTrack GetSegment(int pIndex) {
			return Segments[pIndex];
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override HoverMeshArcTrack GetTick(int pIndex) {
			return Ticks[pIndex];
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void BuildElements() {
			Segments = new HoverAlphaMeshArcTrack[SegmentCount];
			Ticks = new List<HoverAlphaMeshArcTrack>();

			for ( int i = 0 ; i < SegmentCount ; i++ ) {
				Segments[i] = BuildTrack("Segment"+i);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaMeshArcTrack BuildTrack(string pName) {
			var trackGo = new GameObject(pName);
			trackGo.transform.SetParent(gameObject.transform, false);

			HoverAlphaMeshArcTrack track = trackGo.AddComponent<HoverAlphaMeshArcTrack>();
			track.TrackColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
			track.FillColor = new Color(0.1f, 0.9f, 0.2f);
			return track;
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaMeshArcTrack BuildTick(string pName) {
			var tickGo = new GameObject(pName);
			tickGo.transform.SetParent(gameObject.transform, false);

			HoverAlphaMeshArcTrack track = tickGo.AddComponent<HoverAlphaMeshArcTrack>();
			track.TrackColor = new Color(1, 1, 1, 0.5f);
			track.GetComponent<MeshRenderer>().sortingOrder = 1;
			return track;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateTickCount(int pCount) {
			while ( Ticks.Count < pCount ) {
				HoverAlphaMeshArcTrack tick = BuildTick("Tick"+Ticks.Count);
				Ticks.Add(tick);
			}

			while ( Ticks.Count > pCount ) {
				int lastTickIndex = Ticks.Count-1;
				HoverMeshArcTrack tick = Ticks[lastTickIndex];

				Ticks.RemoveAt(lastTickIndex);

				if ( Application.isPlaying ) {
					Destroy(tick.gameObject);
				}
				else {
					DestroyImmediate(tick.gameObject);
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateSegmentsWithInfo() {
			base.UpdateSegmentsWithInfo();

			for ( int i = 0 ; i < Segments.Length ; i++ ) {
				HoverAlphaMeshArcTrack seg = Segments[i];
				seg.Controllers.Set(HoverAlphaMeshArcTrack.AlphaName, this);
				seg.Alpha = Alpha;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateTicksWithInfo() {
			base.UpdateTicksWithInfo();

			for ( int i = 0 ; i < Ticks.Count ; i++ ) {
				HoverAlphaMeshArcTrack tick = Ticks[i];
				tick.Controllers.Set(HoverAlphaMeshArcTrack.AlphaName, this);
				tick.Alpha = Alpha;
			}
		}

	}

}
