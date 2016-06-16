using UnityEngine;

namespace Hover.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverCursorData : MonoBehaviour, IHoverCursorDataForInput {

		[SerializeField]
		public CursorType _Type;
		
		[SerializeField]
		public CursorCapabilityType _Capability = CursorCapabilityType.Full;

		[SerializeField]
		public float _Size = 1;

		[SerializeField]
		public float _DisplayStrength = 1; //read-only
		

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
		public float Size {
			get { return _Size; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float DisplayStrength {
			get { return _DisplayStrength; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 WorldPosition {
			get { return transform.position; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public Quaternion WorldRotation {
			get { return transform.rotation; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetCapability(CursorCapabilityType pCapability) {
			_Capability = pCapability;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetSize(float pSize) {
			_Size = pSize;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetWorldPosition(Vector3 pWorldPosition) {
			transform.position = pWorldPosition;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetWorldRotation(Quaternion pWorldRotation) {
			transform.rotation = pWorldRotation;
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
