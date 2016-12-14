using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverCursorFollower : MonoBehaviour, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(DisplaySpecials=true)]
		public HoverCursorDataProvider CursorDataProvider;

		[DisableWhenControlled]
		public CursorType CursorType = CursorType.LeftPalm;

		[DisableWhenControlled]
		public bool FollowCursorActive = true;
		
		public GameObject[] ObjectsToActivate; //should not include self or parent

		[DisableWhenControlled]
		public bool FollowCursorPosition = true;

		[DisableWhenControlled]
		public bool FollowCursorRotation = true;

		[DisableWhenControlled]
		public bool ScaleUsingCursorSize = false;

		[DisableWhenControlled]
		public float CursorSizeMultiplier = 1;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverCursorFollower() {
			Controllers = new SettingsControllerMap();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ICursorData GetCursorData() {
			return CursorDataProvider.GetCursorData(CursorType);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				CursorDataProvider = FindObjectOfType<HoverCursorDataProvider>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			Controllers.TryExpireControllers();

			if ( CursorDataProvider == null  ) {
				Debug.LogError("Reference to "+typeof(HoverCursorDataProvider).Name+
					" must be set.", this);
				return;
			}

			ICursorData cursor = GetCursorData();
			RaycastResult? raycast = cursor.BestRaycastResult;
			Transform tx = gameObject.transform;

			if ( FollowCursorActive ) {
				foreach ( GameObject go in ObjectsToActivate ) {
					if ( go == null ) {
						continue;
					}

					go.SetActive(cursor.IsActive);
				}
			}

			if ( FollowCursorPosition ) {
				Controllers.Set(SettingsControllerMap.TransformPosition, this, 0);
				tx.position = (raycast == null ? cursor.WorldPosition : raycast.Value.WorldPosition);
			}

			if ( FollowCursorRotation ) {
				Controllers.Set(SettingsControllerMap.TransformRotation, this, 0);

				if ( raycast == null ) {
					tx.rotation = cursor.WorldRotation;
				}
				else {
					RaycastResult rc = raycast.Value;
					Vector3 perpDir = (cursor.RaycastLocalDirection == Vector3.up ? 
						Vector3.right : Vector3.up); //TODO: does this work in all cases?
					Vector3 castUpPos = rc.WorldPosition + rc.WorldRotation*perpDir;
					Vector3 cursorUpPos = rc.WorldPosition + cursor.WorldRotation*perpDir;
					float upToPlaneDist = rc.WorldPlane.GetDistanceToPoint(cursorUpPos);
					Vector3 cursorUpOnPlanePos = cursorUpPos - rc.WorldPlane.normal*upToPlaneDist;
					Quaternion invCastRot = Quaternion.Inverse(rc.WorldRotation);
					Vector3 fromLocalVec = invCastRot*(castUpPos-rc.WorldPosition);
					Vector3 toLocalVec = invCastRot*(cursorUpOnPlanePos-rc.WorldPosition);
					Quaternion applyRot = Quaternion.FromToRotation(fromLocalVec, toLocalVec);
					//Debug.DrawLine(rc.WorldPosition, castUpPos, Color.red);
					//Debug.DrawLine(rc.WorldPosition, cursorUpOnPlanePos, Color.blue);

					tx.rotation = rc.WorldRotation*applyRot;
				}
			}

			if ( ScaleUsingCursorSize ) {
				Controllers.Set(SettingsControllerMap.TransformLocalScale, this, 0);
				tx.localScale = Vector3.one*(cursor.Size*CursorSizeMultiplier);
			}
		}

	}

}
