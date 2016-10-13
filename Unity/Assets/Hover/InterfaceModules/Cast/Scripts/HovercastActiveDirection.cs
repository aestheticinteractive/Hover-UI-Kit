using Hover.Core.Utils;
using UnityEngine;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	public class HovercastActiveDirection : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public const string ActiveWhenFacingTransformName = "ActiveWhenFacingTransform";

		public ISettingsControllerMap Controllers { get; private set; }
		public float CurrentDegree { get; private set; }

		public bool ActiveWhenFacingMainCamera = true;

		[DisableWhenControlled]
		public Transform ActiveWhenFacingTransform;

		public GameObject ChildForActivation;

		public Vector3 LocalFacingDirection = Vector3.forward;

		[Range(10, 180)]
		public float FullyActiveWithinDegree = 30;

		[Range(10, 180)]
		public float InactiveOutsideDegree = 55;

		//TODO: events for active, inactive, fully-active


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HovercastActiveDirection() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
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
			UpdateFacingTransform();
			UpdateDegree();
			Controllers.TryExpireControllers();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateFacingTransform() {
			if ( ActiveWhenFacingMainCamera ) {
				Controllers.Set(ActiveWhenFacingTransformName, this);
				ActiveWhenFacingTransform = null;
			}

			if ( ActiveWhenFacingTransform == null ) {
				ActiveWhenFacingTransform = (Camera.main == null ? transform : Camera.main.transform);
			}
		}
	
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateDegree() {
			HovercastInterface cast = GetComponent<HovercastInterface>();

			Vector3 castWorldDir = cast.transform.TransformDirection(LocalFacingDirection.normalized);
			Vector3 castToTxWorldVec = (ActiveWhenFacingTransform.position-cast.transform.position);
			Vector3 castToTxWorldDir = castToTxWorldVec.normalized;
			float dotBetweenDirs = Vector3.Dot(castWorldDir, castToTxWorldDir);

			if ( dotBetweenDirs >= 1 ) {
				CurrentDegree = 0;
			}
			else {
				CurrentDegree = Mathf.Acos(dotBetweenDirs)/Mathf.PI*180;
			}

			ChildForActivation.SetActive(CurrentDegree <= InactiveOutsideDegree);

			//Vector3 castPos = cast.transform.position;
			//Debug.DrawLine(castPos, castPos+castWorldDir, Color.red);
			//Debug.DrawLine(castPos, castPos+castToTxWorldDir, Color.cyan);
		}

	}

}
