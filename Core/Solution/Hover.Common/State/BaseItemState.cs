using System;
using System.Collections.Generic;
using System.Linq;
using Hover.Common.Custom;
using Hover.Common.Input;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Common.State {

	/*================================================================================================*/
	public class BaseItemState : IBaseItemState {

		public IBaseItem Item { get; private set; }
		public Action<IBaseItemPointsState, Vector3> HoverPointUpdater { get; set; }

		public bool IsSelectionPrevented { get; private set; }

		private readonly BaseInteractionSettings vSettings;
		private readonly BaseItemPointsState vPoints;
		private readonly IDictionary<CursorType, Vector3?> vCursorWorldPosMap;
		private readonly IDictionary<CursorType, float> vHighlightDistanceMap;
		private readonly IDictionary<CursorType, float> vHighlightProgressMap;
		private readonly IDictionary<CursorType, bool> vIsNearestHighlightMap;
		private readonly IDictionary<string, bool> vPreventSelectionViaDisplayMap;

		private DateTime? vSelectionStart;
		private float vDistanceUponSelection;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public BaseItemState(IBaseItem pItem, BaseInteractionSettings pSettings) {
			Item = pItem;
			vSettings = pSettings;
			vPoints = new BaseItemPointsState();

			vCursorWorldPosMap = new Dictionary<CursorType, Vector3?>();
			vHighlightDistanceMap = new Dictionary<CursorType, float>();
			vHighlightProgressMap = new Dictionary<CursorType, float>();
			vIsNearestHighlightMap = new Dictionary<CursorType, bool>();
			vPreventSelectionViaDisplayMap = new Dictionary<string, bool>();

			ResetAllCursorInteractions();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float GetHighlightDistance(CursorType pCursorType) {
			return vHighlightDistanceMap[pCursorType];
		}

		/*--------------------------------------------------------------------------------------------*/
		public float GetHighlightProgress(CursorType pCursorType) {
			return vHighlightProgressMap[pCursorType];
		}

		/*--------------------------------------------------------------------------------------------*/
		public int ItemAutoId {
			get {
				return Item.AutoId;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float MinHighlightDistance {
			get {
				return vHighlightDistanceMap.Min(x => x.Value);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float MaxHighlightProgress {
			get {
				ISelectableItem selItem = (Item as ISelectableItem);

				if ( selItem != null && selItem.IsStickySelected ) {
					return 1;
				}

				return vHighlightProgressMap.Max(x => x.Value);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsNearestHighlight {
			get {
				return vIsNearestHighlightMap.Any(x => x.Value);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SelectionProgress {
			get {
				if ( vSelectionStart == null ) {
					ISelectableItem selItem = (Item as ISelectableItem);

					if ( selItem == null || !selItem.IsStickySelected ) {
						return 0;
					}

					return Mathf.InverseLerp(vSettings.StickyReleaseDistance/vSettings.ScaleMultiplier,
						vDistanceUponSelection, MinHighlightDistance);
				}

				float ms = (float)(DateTime.UtcNow-(DateTime)vSelectionStart).TotalMilliseconds;
				return Math.Min(1, ms/vSettings.SelectionMilliseconds);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3? NearestCursorWorldPos {
			get {
				float minDist = float.MaxValue;
				CursorType? nearestType = null;

				foreach ( KeyValuePair<CursorType, float> pair in vHighlightDistanceMap ) {
					if ( pair.Value >= minDist ) {
						continue;
					}

					minDist = pair.Value;
					nearestType = pair.Key;
				}

				return (nearestType == null ? null : vCursorWorldPosMap[(CursorType)nearestType]);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void PreventSelectionViaDisplay(string pName, bool pPrevent) {
			vPreventSelectionViaDisplayMap[pName] = pPrevent;
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsSelectionPreventedViaDisplay() {
			return vPreventSelectionViaDisplayMap.Values.Any(x => x);
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsSelectionPreventedViaDisplay(string pName) {
			return (vPreventSelectionViaDisplayMap.ContainsKey(pName) && 
				vPreventSelectionViaDisplayMap[pName]);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithCursor(CursorType pType, Vector3? pCursorWorldPos) {
			vCursorWorldPosMap[pType] = pCursorWorldPos;

			bool prevent = (
				pCursorWorldPos == null ||
				!Item.IsEnabled ||
				!Item.IsVisible ||
				vPreventSelectionViaDisplayMap.Any(x => x.Value) ||
				!Item.IsAncestryEnabled ||
				!Item.IsAncestryVisible
			);

			if ( prevent ) {
				vHighlightDistanceMap[pType] = float.MaxValue;
				vHighlightProgressMap[pType] = 0;
				return;
			}

			Vector3 cursorWorldPos = (Vector3)pCursorWorldPos;
			HoverPointUpdater(vPoints, cursorWorldPos);

			Vector3[] pointWorldPosList = vPoints.GetWorldPoints();

			if ( pointWorldPosList.Length == 0 ) {
				throw new Exception("No hover points provided.");
			}

			float sqrMagMin = float.MaxValue;
			//Vector3 nearest = Vector3.zero;

			foreach ( Vector3 pointWorldPos in pointWorldPosList ) {
				float sqrMag = (pointWorldPos-cursorWorldPos).sqrMagnitude;

				if ( sqrMag < sqrMagMin ) {
					sqrMagMin = sqrMag;
					//nearest = pointWorldPos;
				}
			}

			/*foreach ( Vector3 pointWorldPos in pointWorldPosList ) {
				Debug.DrawLine(pointWorldPos, cursorWorldPos, 
					(pointWorldPos == nearest ? Color.red : Color.gray));
			}*/
			
			float dist = (float)Math.Sqrt(sqrMagMin);
			float prog = Mathf.InverseLerp(vSettings.HighlightDistanceMax,
				vSettings.HighlightDistanceMin, dist*vSettings.ScaleMultiplier);

			vHighlightDistanceMap[pType] = dist;
			vHighlightProgressMap[pType] = prog;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void ResetAllCursorInteractions() {
			IsSelectionPrevented = false;

			foreach ( CursorType cursorType in CursorTypeUtil.AllCursorTypes ) {
				vCursorWorldPosMap[cursorType] = null;
				vHighlightDistanceMap[cursorType] = float.MaxValue;
				vHighlightProgressMap[cursorType] = 0;
				vIsNearestHighlightMap[cursorType] = false;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetAsNearestItem(CursorType pCursorType, bool pIsNearest) {
			vIsNearestHighlightMap[pCursorType] = pIsNearest;
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool UpdateSelectionProcess() {
			ISelectableItem selItem = (Item as ISelectableItem);

			if ( selItem == null ) {
				return false;
			}

			////

			float selectProg = SelectionProgress;
			
			if ( selectProg <= 0 || !IsNearestHighlight ) {
				selItem.DeselectStickySelections();
			}

			if ( !IsNearestHighlight || !selItem.AllowSelection ) {
				IsSelectionPrevented = false;
				vSelectionStart = null;
				return false;
			}

			////

			bool allNearestCursorsHavePartialHighlight = true; //where "partial" == "not 100%"

			foreach ( CursorType cursorType in vCursorWorldPosMap.Keys ) {
				if ( vIsNearestHighlightMap[cursorType] && vHighlightProgressMap[cursorType] >= 1 ) {
					allNearestCursorsHavePartialHighlight = false;
				}
			}

			if ( allNearestCursorsHavePartialHighlight ) {
				IsSelectionPrevented = false;
				vSelectionStart = null;
				return false;
			}

			////

			if ( IsSelectionPrevented ) {
				vSelectionStart = null;
				return false;
			}

			if ( vSelectionStart == null ) {
				vSelectionStart = DateTime.UtcNow;
				return false;
			}

			if ( selectProg < 1 ) {
				return false;
			}

			vSelectionStart = null;
			IsSelectionPrevented = true;
			vDistanceUponSelection = MinHighlightDistance;
			selItem.Select();
			return true;
		}

	}

}
