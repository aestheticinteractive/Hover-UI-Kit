using System;
using System.Collections.Generic;
using Hover.Common.Custom;
using Hover.Common.Input;
using Hover.Common.Items;
using Hover.Common.Renderers;
using UnityEngine;

namespace Hover.Common.State {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverItemData))]
	[RequireComponent(typeof(HoverRendererController))]
	public class HoverItemCursorActivity : MonoBehaviour {

		[Serializable]
		public struct Highlight {
			public HovercursorData Data;
			public Vector3 NearestWorldPos;
			public float Distance;
			public float Progress;
		}

		public Highlight? NearestHighlight { get; private set; }
		public List<Highlight> Highlights { get; private set; }
		
		public HovercursorDataProvider CursorDataProvider;
		public bool AllowCursorHighlighting = true;

		private readonly BaseInteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemCursorActivity() {
			vSettings = new BaseInteractionSettings();
			vSettings.HighlightDistanceMin = 3;
			vSettings.HighlightDistanceMax = 7;
			vSettings.StickyReleaseDistance = 5;
			vSettings.SelectionMilliseconds = 400;
			vSettings.ApplyScaleMultiplier = true;
			vSettings.ScaleMultiplier = 1;
			
			Highlights = new List<Highlight>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				FindObjectOfType<HovercursorDataProvider>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			Highlights.Clear();
			NearestHighlight = null;

			if ( !AllowCursorHighlighting ) {
				return;
			}

			IProximityProvider proxProv = GetComponent<HoverRendererController>();

			foreach ( HovercursorData data in CursorDataProvider.Cursors ) {
				Highlight high = CalculateHighlight(proxProv, data);
				Highlights.Add(high);

				if ( NearestHighlight == null ||
							high.Distance < ((Highlight)NearestHighlight).Distance ) {
					NearestHighlight = high;
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Highlight? GetHighlight(CursorType pType) {
			foreach ( Highlight high in Highlights ) {
				if ( high.Data.Type == pType ) {
					return high;
				}
			}

			return null;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Highlight CalculateHighlight(IProximityProvider pProxProv, HovercursorData pData) {
			var high = new Highlight();
			high.Data = pData;
			
			if ( !Application.isPlaying ) {
				return high;
			}
			
			Vector3 cursorWorldPos = pData.transform.position;
			
			high.NearestWorldPos = pProxProv.GetNearestWorldPosition(cursorWorldPos);
			high.Distance = (cursorWorldPos-high.NearestWorldPos).magnitude;
			high.Progress = Mathf.InverseLerp(vSettings.HighlightDistanceMax,
				vSettings.HighlightDistanceMin, high.Distance*vSettings.ScaleMultiplier);
			
			return high;
		}

	}

}
