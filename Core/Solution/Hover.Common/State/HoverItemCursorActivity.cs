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

		//TODO: make Update() run constantly in editor mode, to keep the proximity values fresh

		[Serializable]
		public struct Highlight {
			public HovercursorData Data;
			public Vector3 NearestWorldPos;
			public float Distance;
			public float Progress;
		}

		public Highlight? NearestHighlight { get; private set; }

		public HovercursorDataProvider CursorDataProvider;
		public bool AllowCursorHighlighting = true;
		public List<Highlight> Highlights; //read-only

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
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				FindObjectOfType<HovercursorDataProvider>();
			}

			if ( Highlights == null ) {
				Highlights = new List<Highlight>();
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
				var high = new Highlight();
				high.Data = data;
				high.NearestWorldPos = proxProv.GetNearestWorldPosition(data.transform.position);
				high.Distance = (data.transform.position-high.NearestWorldPos).magnitude;
				high.Progress = Mathf.InverseLerp(vSettings.HighlightDistanceMax,
					vSettings.HighlightDistanceMin, high.Distance*vSettings.ScaleMultiplier);
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

	}

}
