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

		public bool IsSelectionPrevented { get; private set; }
		public int DisplaySelectionPreventions { get; set; }

		private readonly InteractionSettings vSettings;
		private readonly IDictionary<CursorType, float> vHighlightDistanceMap;
		private readonly IDictionary<CursorType, float> vHighlightProgressMap;
		private readonly IDictionary<CursorType, bool> vIsNearestHighlightMap;

		private Func<Vector3, float> vCursorDistanceFunc;
		private DateTime? vSelectionStart;
		private float vDistanceUponSelection;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public BaseItemState(IBaseItem pItem, InteractionSettings pSettings) {
			Item = pItem;
			vSettings = pSettings;

			vHighlightDistanceMap = new Dictionary<CursorType, float>();
			vHighlightProgressMap = new Dictionary<CursorType, float>();
			vIsNearestHighlightMap = new Dictionary<CursorType, bool>();

			foreach ( CursorType cursorType in Enum.GetValues(typeof(CursorType)) ) {
				vHighlightDistanceMap[cursorType] = float.MaxValue;
				vHighlightProgressMap[cursorType] = 0;
				vIsNearestHighlightMap[cursorType] = false;
			}
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
		public float MinHighlightDistance {
			get {
				return vHighlightDistanceMap.Min(x => x.Value);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float MaxHighlightProgress {
			get {
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

					return Mathf.InverseLerp(vSettings.StickyReleaseDistance,
						vDistanceUponSelection, MinHighlightDistance);
				}

				float ms = (float)(DateTime.UtcNow-(DateTime)vSelectionStart).TotalMilliseconds;
				return Math.Min(1, ms/vSettings.SelectionMilliseconds);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetCursorDistanceFunction(Func<Vector3, float> pFunc) {
			vCursorDistanceFunc = pFunc;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithCursor(CursorType pType, Vector3? pCursorWorldPosition) {
			if ( pCursorWorldPosition == null || DisplaySelectionPreventions > 0 || !Item.IsEnabled ) {
				vHighlightDistanceMap[pType] = float.MaxValue;
				vHighlightProgressMap[pType] = 0;
				return;
			}

			if ( vCursorDistanceFunc == null ) {
				throw new Exception("No CursorDistanceFunction has been set.");
			}

			float dist = vCursorDistanceFunc((Vector3)pCursorWorldPosition);
			float prog = Mathf.InverseLerp(vSettings.HighlightDistanceMax,
				vSettings.HighlightDistanceMin, dist*vSettings.ScaleMultiplier);

			vHighlightDistanceMap[pType] = dist;
			vHighlightProgressMap[pType] = prog;
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

			bool isNearest = IsNearestHighlight;

			if ( !isNearest || SelectionProgress <= 0 ) {
				selItem.DeselectStickySelections();
			}

			if ( !isNearest || MaxHighlightProgress < 1 ) {
				vSelectionStart = null;
				IsSelectionPrevented = false;
				return false;
			}

			if ( IsSelectionPrevented || !selItem.AllowSelection ) {
				vSelectionStart = null;
				return false;
			}

			if ( vSelectionStart == null ) {
				vSelectionStart = DateTime.UtcNow;
				return false;
			}

			if ( SelectionProgress < 1 ) {
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
