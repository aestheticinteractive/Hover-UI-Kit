using System;
using System.Collections.Generic;
using System.Linq;
using Hover.Common.Input;
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
		private readonly CursorSettings vSettings;
		private readonly Transform vBaseTx;
		private readonly IDictionary<CursorDomain, 
			IDictionary<string, CursorInteractState>> vInteractMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState(IInputCursor pInputCursor, CursorSettings pSettings, Transform pBaseTx) {
			vInputCursor = pInputCursor;
			vSettings = pSettings;
			vBaseTx = pBaseTx;

			vInteractMap = new Dictionary<CursorDomain, IDictionary<string, CursorInteractState>>();

			foreach ( CursorDomain cursorDom in Enum.GetValues(typeof(CursorDomain)) ) {
				vInteractMap.Add(cursorDom, new Dictionary<string, CursorInteractState>());
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ICursorInteractState AddOrGetInteractionState(CursorDomain pDomain, string pId) {
			IDictionary<string, CursorInteractState> map = vInteractMap[pDomain];

			if ( map.ContainsKey(pId) ) {
				return map[pId];
			}

			var interState = new CursorInteractState(Type, pDomain, pId);
			map.Add(pId, interState);
			return interState;
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool RemoveInteractionState(CursorDomain pDomain, string pId) {
			IDictionary<string, CursorInteractState> map = vInteractMap[pDomain];
			return (map.ContainsKey(pId) && map.Remove(pId));
		}

		/*--------------------------------------------------------------------------------------------*/
		public float GetMaxDisplayStrength() {
			return GetAllInteractStates()
				.Select(x => x.DisplayStrength)
				.DefaultIfEmpty(0)
				.Max();
		}

		/*--------------------------------------------------------------------------------------------*/
		public float GetMaxHighlightProgress() {
			return GetAllInteractStates()
				.Select(x => x.HighlightProgress)
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
			Type = vInputCursor.Type;
			IsInputAvailable = vInputCursor.IsAvailable;
			Size = vInputCursor.Size;

			Position = vInputCursor.Position+
				vInputCursor.Rotation*Vector3.back*vSettings.CursorForwardDistance;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private CursorInteractState[] GetAllInteractStates() {
			ICollection<IDictionary<string, CursorInteractState>> maps = vInteractMap.Values;
			var list = new List<CursorInteractState>();

			foreach ( IDictionary<string, CursorInteractState> map in maps ) {
				list.AddRange(map.Values);
			}

			return list.ToArray();
		}

	}

}
