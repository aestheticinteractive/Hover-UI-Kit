using System.Collections.Generic;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Cursors {

	/*================================================================================================*/
	public class HoverCursorData : MonoBehaviour, ICursorDataForInput {
		
		public RaycastResult? BestRaycastResult { get; set; }
		public float MaxItemHighlightProgress { get; set; }
		public float MaxItemSelectionProgress { get; set; }
		public List<StickySelectionInfo> ActiveStickySelections { get; private set; }

		[SerializeField]
		private CursorType _Type;
		
		[SerializeField]
		private CursorCapabilityType _Capability = CursorCapabilityType.Full;
		
		[SerializeField]
		private bool _IsRaycast = false;

		[SerializeField]
		private Vector3 _RaycastLocalDirection = Vector3.up;

		[SerializeField]
		private float _Size = 1;

		[SerializeField]
		[Range(0, 1)]
		private float _TriggerStrength = 0;

		private ICursorIdle vIdle;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverCursorData() {
			ActiveStickySelections = new List<StickySelectionInfo>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorType Type {
			get { return _Type; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public bool IsActive {
			get { return isActiveAndEnabled; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public bool CanCauseSelections {
			get { return (IsActive && Capability == CursorCapabilityType.Full); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public CursorCapabilityType Capability {
			get { return _Capability; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsRaycast {
			get { return _IsRaycast; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 RaycastLocalDirection {
			get { return _RaycastLocalDirection; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float Size {
			get { return _Size; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float TriggerStrength {
			get { return _TriggerStrength; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 WorldPosition {
			get { return transform.position; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public Quaternion WorldRotation {
			get { return transform.rotation; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public ICursorIdle Idle {
			get {
				if ( vIdle == null ) {
					vIdle = GetComponent<ICursorIdle>();
				}

				return vIdle;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetIsRaycast(bool pIsRaycast) {
			_IsRaycast = pIsRaycast;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetRaycastLocalDirection(Vector3 pRaycastLocalDirection) {
			_RaycastLocalDirection = pRaycastLocalDirection;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetCapability(CursorCapabilityType pCapability) {
			_Capability = pCapability;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetSize(float pSize) {
			_Size = pSize;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetTriggerStrength(float pTriggerStrength) {
			_TriggerStrength = pTriggerStrength;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetWorldPosition(Vector3 pWorldPosition) {
			transform.position = pWorldPosition;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetWorldRotation(Quaternion pWorldRotation) {
			transform.rotation = pWorldRotation;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetIdle(ICursorIdle pIdle) {
			vIdle = pIdle;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetUsedByInput(bool pIsUsed) {
			enabled = pIsUsed;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ActivateIfUsedByInput() {
			gameObject.SetActive(enabled && Capability != CursorCapabilityType.None);
		}

	}

}
