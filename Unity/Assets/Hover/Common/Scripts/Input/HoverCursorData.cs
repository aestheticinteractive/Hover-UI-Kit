using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Common.Input {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverCursorData : MonoBehaviour, IHoverCursorDataForInput {

		[SerializeField]
		[FormerlySerializedAs("Type")]
		public CursorType _Type;
		
		[SerializeField]
		public bool _AllowUsage = true;

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
			get { return (enabled && AllowUsage); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool AllowUsage {
			get { return _AllowUsage; }
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
		public void SetAllowUsage(bool pAllowUsage) {
			_AllowUsage = pAllowUsage;
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
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetUsage(bool pIsUsed) {
			enabled = pIsUsed;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ActivateBasedOnUsage() {
			gameObject.SetActive(IsActive);
		}
	}

}
