using System;
using System.Collections.Generic;
using System.Linq;
using Hover.Common.Input;
using Hover.Common.State;
using Hover.Cursor.Custom;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public class CursorState : ICursorState {

		public CursorType Type { get; private set; }
		public bool IsInputAvailable { get; private set; }
		public Vector3 Position { get; private set; }
		public float Size { get; private set; }

		private readonly IInputCursor vInputCursor;
		private readonly ICursorSettings vSettings;
		private readonly Transform vBaseTx;
		private readonly IDictionary<CursorDomain, float> vDisplayStrengthMap;
		private readonly IDictionary<CursorDomain,
			IDictionary<int, IBaseItemInteractionState>> vInteractMaps;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState(IInputCursor pInputCursor, ICursorSettings pSettings, Transform pBaseTx) {
			vInputCursor = pInputCursor;
			vSettings = pSettings;
			vBaseTx = pBaseTx;

			vDisplayStrengthMap = new Dictionary<CursorDomain, float>();
			vInteractMaps = new Dictionary<CursorDomain, IDictionary<int, IBaseItemInteractionState>>();

			foreach ( CursorDomain cursorDom in Enum.GetValues(typeof(CursorDomain)) ) {
				vDisplayStrengthMap.Add(cursorDom, 0);
				vInteractMaps.Add(cursorDom, new Dictionary<int, IBaseItemInteractionState>());
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetDisplayStrength(CursorDomain pDomain, float pStrength) {
			vDisplayStrengthMap[pDomain] = pStrength;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddOrUpdateInteraction(CursorDomain pDomain, IBaseItemInteractionState pItem) {
			IDictionary<int, IBaseItemInteractionState> map = vInteractMaps[pDomain];
			int key = pItem.ItemAutoId;

			if ( map.ContainsKey(key) ) {
				return;
			}

			map.Add(key, pItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void RemoveAllInteractions(CursorDomain pDomain) {
			vInteractMaps[pDomain].Clear();
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool RemoveInteraction(CursorDomain pDomain, IBaseItemInteractionState pItem) {
			IDictionary<int, IBaseItemInteractionState> map = vInteractMaps[pDomain];
			int key = pItem.ItemAutoId;
			return (map.ContainsKey(key) && map.Remove(key));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float GetMaxDisplayStrength() {
			return vDisplayStrengthMap.Values.Max();
		}

		/*--------------------------------------------------------------------------------------------*/
		public float GetMaxHighlightProgress() {
			return GetAllInteractStates()
				.Select(x => x.MaxHighlightProgress)
				.DefaultIfEmpty(0)
				.Max();
		}

		/*--------------------------------------------------------------------------------------------*/
		public float GetMaxSelectionProgress() {
			return GetAllInteractStates()
				.Select(x => x.SelectionProgress)
				.DefaultIfEmpty(0)
				.Max();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetWorldPosition() {
			return vBaseTx.TransformPoint(Position);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			if ( vInteractMaps.Count == 0 ) {
				return;
			}

			Type = vInputCursor.Type;
			IsInputAvailable = vInputCursor.IsAvailable;
			Size = vInputCursor.Size;

			Position = vInputCursor.Position+
				vInputCursor.Rotation*Vector3.back*vSettings.CursorForwardDistance;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IEnumerable<IBaseItemInteractionState> GetAllInteractStates() {
			return vInteractMaps.Values.SelectMany(map => map.Values);
		}

	}

}
