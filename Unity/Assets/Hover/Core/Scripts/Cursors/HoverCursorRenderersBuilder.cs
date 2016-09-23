using Hover.Core.Renderers.Cursors;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverCursorRenderersBuilder : MonoBehaviour {

		public CursorCapabilityType MinimumCapabilityType = CursorCapabilityType.None;
		public GameObject CursorRendererPrefab;
		public GameObject IdleRendererPrefab;
		public bool ClickToBuild = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorRendererPrefab == null ) {
				CursorRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverOpaqueCursorArcRenderer-Default");
			}

			if ( IdleRendererPrefab == null ) {
				IdleRendererPrefab = Resources.Load<GameObject>(
					"Prefabs/HoverOpaqueIdleArcRenderer-Default");
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !ClickToBuild ) {
				return;
			}

			ClickToBuild = false;
			UnityUtil.FindOrAddHoverManagerPrefab();
			PerformBuild();
			DestroyImmediate(this, false);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void PerformBuild() {
			HoverCursorDataProvider cursorProv = FindObjectOfType<HoverCursorDataProvider>();

			foreach ( ICursorData cursorData in cursorProv.Cursors ) {
				if ( cursorData.Capability < MinimumCapabilityType ) {
					continue;
				}

				BuildCursor(cursorProv, cursorData);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildCursor(HoverCursorDataProvider pProv, ICursorData pData) {
			var curGo = new GameObject(pData.Type+"");
			curGo.transform.SetParent(gameObject.transform, false);

			TreeUpdater treeUp = curGo.AddComponent<TreeUpdater>();

			HoverCursorFollower follow = curGo.AddComponent<HoverCursorFollower>();
			follow.CursorDataProvider = pProv;
			follow.CursorType = pData.Type;
			follow.FollowCursorActive = false;
			follow.ScaleUsingCursorSize = true;

			HoverRendererCursorUpdater cursRendUp = curGo.AddComponent<HoverRendererCursorUpdater>();
			cursRendUp.CursorRendererPrefab = CursorRendererPrefab;

			if ( pData.Idle != null ) {
				HoverRendererIdleUpdater idleRendUp = curGo.AddComponent<HoverRendererIdleUpdater>();
				idleRendUp.IdleRendererPrefab = IdleRendererPrefab;
			}

			follow.Update(); //moves interface to the correct cursor transform
			treeUp.Update(); //force renderer creation
		}

	}

}
