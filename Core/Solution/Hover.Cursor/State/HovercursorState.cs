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
	public class HovercursorState : IHovercursorState {

		public HovercursorVisualSettings VisualSettings { get; private set; }
		public Transform CameraTransform { get; private set; }
		public CursorType[] ActiveCursorTypes { get; private set; }

		private readonly HovercursorInput vInput;
		private readonly Transform vBaseTx;
		private readonly IDictionary<CursorType, CursorState> vCursorMap;
		private readonly IDictionary<CursorType, Transform> vTransformMap;
		private readonly IList<IHovercursorDelegate> vDelegates;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercursorState(Transform pBaseTx, HovercursorInput pInput,
											HovercursorVisualSettings pVisualSett, Transform pCamera) {
			vBaseTx = pBaseTx;
			vInput = pInput;

			VisualSettings = pVisualSett;
			CameraTransform = pCamera;
			ActiveCursorTypes = new CursorType[0];

			vCursorMap = new Dictionary<CursorType, CursorState>();
			vTransformMap = new Dictionary<CursorType, Transform>();
			vDelegates = new List<IHovercursorDelegate>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IHovercursorInput Input {
			get {
				return vInput;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ICursorState GetCursorState(CursorType pType) {
			TryInitCursor(pType);
			return vCursorMap[pType];
		}

		/*--------------------------------------------------------------------------------------------*/
		public Transform GetCursorTransform(CursorType pType) {
			TryInitCursor(pType);
			return vTransformMap[pType];
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetCursorTransform(CursorType pType, Transform pTransform) {
			vTransformMap[pType] = pTransform;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateBeforeInput() {
			IHovercursorDelegate[] activeDelegates = GetActiveDelegates();
			var typeMap = new HashSet<CursorType>();

			foreach ( IHovercursorDelegate del in activeDelegates ) {
				foreach ( CursorType type in del.ActiveCursorTypes ) {
					typeMap.Add(type);
				}
			}

			ActiveCursorTypes = typeMap.ToArray();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			IHovercursorDelegate[] activeDelegates = GetActiveDelegates();

			foreach ( CursorType type in vCursorMap.Keys ) {
				CursorState cursor = vCursorMap[type];
				float maxDispStren = 0;
				var interacts = new List<IBaseItemInteractionState>();

				foreach ( IHovercursorDelegate del in activeDelegates ) {
					if ( !del.ActiveCursorTypes.Contains(type) ) {
						continue;
					}

					maxDispStren = Math.Max(maxDispStren, del.CursorDisplayStrength);
					interacts.AddRange(del.GetActiveCursorInteractions(type));
				}

				cursor.UpdateAfterInput(maxDispStren, interacts.ToArray());
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddDelegate(IHovercursorDelegate pDelegate) {
			if ( vDelegates.Contains(pDelegate) ) {
				throw new Exception("This "+typeof(IHovercursorDelegate).Name+
					" has already been added.");
			}

			vDelegates.Add(pDelegate);
			vInput.SetPlaneProvider(GetActiveCursorPlanes);
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool RemoveDelegate(IHovercursorDelegate pDelegate) {
			return vDelegates.Remove(pDelegate);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IHovercursorDelegate[] GetActiveDelegates() {
			var activeList = new List<IHovercursorDelegate>();

			foreach ( IHovercursorDelegate del in vDelegates ) {
				if ( !del.IsCursorInteractionEnabled || del.CursorDisplayStrength <= 0 ) {
					continue;
				}

				if ( del.ActiveCursorTypes == null ) {
					throw new Exception("The '"+del.Domain+"' "+typeof(IHovercursorDelegate).Name+
						".ActiveCursorTypes list should not be null.");
				}

				if ( del.ActiveCursorTypes.Length == 0 ) {
					continue;
				}

				activeList.Add(del);
			}

			return activeList.ToArray();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private PlaneData[] GetActiveCursorPlanes(CursorType pType) {
			IHovercursorDelegate[] activeDelegates = GetActiveDelegates();
			var planes = new List<PlaneData>();

			foreach ( IHovercursorDelegate del in activeDelegates ) {
				planes.AddRange(del.GetActiveCursorPlanes(pType));
			}

			return planes.ToArray();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void TryInitCursor(CursorType pType) {
			if ( vCursorMap.ContainsKey(pType) ) {
				return;
			}

			var cursor = new CursorState(Input.GetCursor(pType), VisualSettings.GetSettings(), vBaseTx);
			vCursorMap.Add(pType, cursor);

			ActiveCursorTypes = ActiveCursorTypes
				.Concat(new[] { pType })
				.ToArray();
		}

	}

}
