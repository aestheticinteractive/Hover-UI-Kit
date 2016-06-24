using Hover.Utils;
using UnityEngine;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	public class HovercastActiveDirection : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public float CurrentDegree { get; private set; }

		public Transform ActiveWhenFacing;
		public GameObject ChildForActivation;

		[Range(10, 180)]
		public float FullyActiveWithinDegree = 30;
		
		[Range(10, 180)]
		public float InactiveOutsideDegree = 55;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( ActiveWhenFacing == null ) {
				ActiveWhenFacing = Camera.main.transform;
			}

			if ( ChildForActivation == null ) {
				ChildForActivation = gameObject.transform.GetChild(0).gameObject;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HovercastInterface cast = GetComponent<HovercastInterface>();
			
			Vector3 castWorldDir = cast.transform.TransformDirection(Vector3.forward);
			Vector3 castToTxWorldDir = (ActiveWhenFacing.position-cast.transform.position).normalized;
			float dotBetweenDirs = Vector3.Dot(castWorldDir, castToTxWorldDir);

			CurrentDegree = Mathf.Acos(dotBetweenDirs)/Mathf.PI*180;
			ChildForActivation.SetActive(CurrentDegree <= InactiveOutsideDegree);
			
			//Vector3 castPos = cast.transform.position;
			//Debug.DrawLine(castPos, castPos+castWorldDir, Color.red);
			//Debug.DrawLine(castPos, castPos+castToTxWorldDir, Color.cyan);
		}

	}

}
