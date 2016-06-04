﻿using System.Collections.Generic;
using Hover.Common.Renderers.Shapes.Rect;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Packs.Alpha.Rect {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverAlphaFillRectSlider : HoverFillRectSlider {
	
		public const string AlphaName = "Alpha";
		
		public HoverAlphaMeshRectTrack[] Segments;

		public List<HoverAlphaMeshRectTrack> Ticks;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override HoverMeshRectTrack GetSegment(int pIndex) {
			return Segments[pIndex];
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override HoverMeshRectTrack GetTick(int pIndex) {
			return Ticks[pIndex];
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void BuildElements() {
			Segments = new HoverAlphaMeshRectTrack[SegmentCount];
			Ticks = new List<HoverAlphaMeshRectTrack>();

			for ( int i = 0 ; i < SegmentCount ; i++ ) {
				Segments[i] = BuildTrack("Segment"+i);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaMeshRectTrack BuildTrack(string pName) {
			var trackGo = new GameObject(pName);
			trackGo.transform.SetParent(gameObject.transform, false);

			HoverAlphaMeshRectTrack track = trackGo.AddComponent<HoverAlphaMeshRectTrack>();
			track.TrackColor = new Color(0.1f, 0.1f, 0.1f, 0.333f);
			track.FillColor = new Color(0.1f, 0.9f, 0.2f);
			return track;
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaMeshRectTrack BuildTick(string pName) {
			//TODO: should NOT have to do this find... why is Unity clearing "Ticks" upon play??
			// ... without this, that empty "Ticks" list causes new ones to be created

			Transform tickTx = gameObject.transform.FindChild(pName);

			if ( tickTx != null ) {
				HoverAlphaMeshRectTrack existingTick = tickTx.GetComponent<HoverAlphaMeshRectTrack>();

				if ( existingTick != null ) {
					return existingTick;
				}
			}

			////

			var tickGo = new GameObject(pName);
			tickGo.transform.SetParent(gameObject.transform, false);

			HoverAlphaMeshRectTrack tick = tickGo.AddComponent<HoverAlphaMeshRectTrack>();
			tick.TrackColor = new Color(1, 1, 1, 0.5f);
			tick.GetComponent<MeshRenderer>().sortingOrder = 1;
			return tick;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateTickCount(int pCount) {
			while ( Ticks.Count < pCount ) {
				HoverAlphaMeshRectTrack tick = BuildTick("Tick"+Ticks.Count);
				Ticks.Add(tick);
			}

			while ( Ticks.Count > pCount ) {
				int lastTickIndex = Ticks.Count-1;
				HoverMeshRectTrack tick = Ticks[lastTickIndex];

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
				HoverAlphaMeshRectTrack seg = Segments[i];
				seg.Controllers.Set(HoverAlphaMeshRectTrack.AlphaName, this);
				seg.Alpha = Alpha;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateTicksWithInfo() {
			base.UpdateTicksWithInfo();

			for ( int i = 0 ; i < Ticks.Count ; i++ ) {
				HoverAlphaMeshRectTrack tick = Ticks[i];
				tick.Controllers.Set(HoverAlphaMeshRectTrack.AlphaName, this);
				tick.Alpha = Alpha;
			}
		}

	}

}
