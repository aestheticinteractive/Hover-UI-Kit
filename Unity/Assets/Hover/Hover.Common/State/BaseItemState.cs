using System;
using System.Collections.ObjectModel;
using Hover.Common.Custom;
using Hover.Common.Input;
using Hover.Common.Items;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Common.State {

	/*================================================================================================*/
	public class BaseItemState : IBaseItemState {

		private static readonly Func<float, float, bool> FindMin = ((a,b) => (a < b));
		private static readonly Func<float, float, bool> FindMax = ((a,b) => (a > b));

		public IBaseItem Item { get; private set; }
		public Action<IBaseItemPointsState, Vector3> HoverPointUpdater { get; set; }

		public bool IsHighlightPrevented { get; private set; }
		public bool IsSelectionPrevented { get; private set; }

		private readonly BaseInteractionSettings vSettings;
		private readonly BaseItemPointsState vPoints;
		private readonly ListMap<CursorType, Vector3?> vCursorWorldPosMap;
		private readonly ListMap<CursorType, float> vHighlightDistanceMap;
		private readonly ListMap<CursorType, float> vHighlightProgressMap;
		private readonly ListMap<CursorType, bool> vIsNearestHighlightMap;
		private readonly ListMap<string, bool> vPreventSelectionViaDisplayMap;

		private DateTime? vSelectionStart;
		private float vDistanceUponSelection;
		private bool vIsInResetState;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public BaseItemState(IBaseItem pItem, BaseInteractionSettings pSettings) {
			Item = pItem;
			vSettings = pSettings;
			vPoints = new BaseItemPointsState();

			vCursorWorldPosMap = new ListMap<CursorType, Vector3?>(
				EnumIntKeyComparer.CursorType, false, true);
			vHighlightDistanceMap = new ListMap<CursorType, float>(EnumIntKeyComparer.CursorType);
			vHighlightProgressMap = new ListMap<CursorType, float>(EnumIntKeyComparer.CursorType);
			vIsNearestHighlightMap = new ListMap<CursorType, bool>(EnumIntKeyComparer.CursorType);
			vPreventSelectionViaDisplayMap = new ListMap<string, bool>(null);
			
			ResetAllCursorInteractions();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float GetHighlightDistance(CursorType pCursorType) {
			if ( !vHighlightDistanceMap.ContainsKey(pCursorType) ) {
				return float.MaxValue;
			}

			return vHighlightDistanceMap[pCursorType];
		}

		/*--------------------------------------------------------------------------------------------*/
		public float GetHighlightProgress(CursorType pCursorType) {
			if ( !vHighlightProgressMap.ContainsKey(pCursorType) ) {
				return 0;
			}

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
				return vHighlightDistanceMap.GetValue(FindMin);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float MaxHighlightProgress {
			get {
				ISelectableItem selItem = (Item as ISelectableItem);

				if ( selItem != null && selItem.IsStickySelected ) {
					return 1;
				}

				if ( vHighlightProgressMap.KeysReadOnly.Count == 0 ) {
					return 0;
				}

				return vHighlightProgressMap.GetValue(FindMax);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsNearestHighlight {
			get {
				return vIsNearestHighlightMap.HasValue(x => x);
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

				for ( int i = 0 ; i < vHighlightDistanceMap.KeysReadOnly.Count ; i++ ) {
					CursorType cursorType = vHighlightDistanceMap.KeysReadOnly[i];
					float dist = vHighlightDistanceMap[cursorType];

					if ( dist >= minDist ) {
						continue;
					}

					minDist = dist;
					nearestType = cursorType;
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
			return vPreventSelectionViaDisplayMap.HasValue(x => x);
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsSelectionPreventedViaDisplay(string pName) {
			return (vPreventSelectionViaDisplayMap.ContainsKey(pName) && 
				vPreventSelectionViaDisplayMap[pName]);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateBeforeCursors() {
			IsHighlightPrevented = (
				!Item.IsEnabled ||
				!Item.IsVisible ||
				!Item.IsAncestryEnabled ||
				!Item.IsAncestryVisible ||
				!(Item is ISelectableItem) ||
				vPreventSelectionViaDisplayMap.HasValue(x => x)
			);

			if ( !IsHighlightPrevented ) {
				vIsInResetState = false;
				return;
			}

			ResetAllCursorInteractions();
			vSelectionStart = null;

			ISelectableItem selItem = (Item as ISelectableItem);

			if ( selItem != null ) {
				selItem.DeselectStickySelections();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithCursor(CursorType pType, Vector3? pCursorWorldPos) {
			vCursorWorldPosMap[pType] = pCursorWorldPos;

			if ( IsHighlightPrevented ) {
				return;
			}

			if ( pCursorWorldPos == null ) {
				vHighlightDistanceMap[pType] = float.MaxValue;
				vHighlightProgressMap[pType] = 0;
				return;
			}

			Vector3 cursorWorldPos = (Vector3)pCursorWorldPos;
			HoverPointUpdater(vPoints, cursorWorldPos);

			ReadOnlyCollection<Vector3> pointWorldPosList = vPoints.GetWorldPoints();

			if ( pointWorldPosList.Count == 0 ) {
				throw new Exception("No hover points provided.");
			}

			float sqrMagMin = float.MaxValue;
			//Vector3 nearest = Vector3.zero;

			for ( int i = 0 ; i < pointWorldPosList.Count ; i++ ) {
				float sqrMag = (pointWorldPosList[i]-cursorWorldPos).sqrMagnitude;

				if ( sqrMag < sqrMagMin ) {
					sqrMagMin = sqrMag;
					//nearest = pointWorldPosList[i];
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
			if ( vIsInResetState ) {
				return;
			}

			IsSelectionPrevented = false;
			vIsInResetState = true;

			for ( int i = 0 ; i < vCursorWorldPosMap.KeysReadOnly.Count ; i++ ) {
				CursorType cursorType = vCursorWorldPosMap.KeysReadOnly[i];

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

			for ( int i = 0 ; i < vCursorWorldPosMap.KeysReadOnly.Count ; i++ ) {
				CursorType cursorType = vCursorWorldPosMap.KeysReadOnly[i];

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
