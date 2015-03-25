using System.Collections.Generic;
using System.Linq;
using Hover.Common.Input;
using Hover.Cursor.Custom;
using Hover.Cursor.Input;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public class HovercursorState : IHovercursorState {

		public HovercursorVisualSettings VisualSettings { get; private set; }
		public Transform CameraTransform { get; private set; }
		public CursorType[] InitializedCursorTypes { get; private set; }

		private readonly HovercursorInput vInput;
		private readonly Transform vBaseTx;
		private readonly IDictionary<CursorType, CursorState> vCursorMap;
		private readonly IDictionary<CursorType, Transform> vTransformMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercursorState(Transform pBaseTx, HovercursorInput pInput,
											HovercursorVisualSettings pVisualSett, Transform pCamera) {
			vBaseTx = pBaseTx;
			vInput = pInput;

			VisualSettings = pVisualSett;
			CameraTransform = pCamera;
			InitializedCursorTypes = new CursorType[0];

			vCursorMap = new Dictionary<CursorType, CursorState>();
			vTransformMap = new Dictionary<CursorType, Transform>();
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
		public void UpdateAfterInput() {
			foreach ( CursorState cursorState in vCursorMap.Values ) {
				cursorState.UpdateAfterInput();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddOrUpdatePlane(string pId, Vector3 pPointWorld, Vector3 pNormalWorld) {
			vInput.AddOrUpdatePlane(pId, pPointWorld, pNormalWorld);
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool RemovePlane(string pId) {
			return vInput.RemovePlane(pId);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TryInitCursor(CursorType pType) {
			if ( vCursorMap.ContainsKey(pType) ) {
				return;
			}

			var cursor = new CursorState(Input.GetCursor(pType),
				VisualSettings.GetSettings(), vBaseTx);
			vCursorMap.Add(pType, cursor);

			InitializedCursorTypes = InitializedCursorTypes
				.Concat(new[] { pType })
				.ToArray();
		}

	}

}
