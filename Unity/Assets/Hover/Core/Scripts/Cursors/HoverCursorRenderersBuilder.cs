using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverCursorRenderersBuilder : MonoBehaviour {

		public CursorCapabilityType MinimumCapabilityType = CursorCapabilityType.None;
		public GameObject CursorRendererPrefab;
		public GameObject IdleRendererPrefab;

		[TriggerButton("Build Cursor Renderers")]
		public bool ClickToBuild;


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
		public void OnEditorTriggerButtonSelected() {
			UnityUtil.FindOrAddHoverKitPrefab();
			PerformBuild();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( ClickToBuild ) {
				DestroyImmediate(this, false);
			}
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

			HoverCursorRendererUpdater cursRendUp = curGo.AddComponent<HoverCursorRendererUpdater>();
			cursRendUp.CursorRendererPrefab = CursorRendererPrefab;

			if ( pData.Idle != null ) {
				HoverIdleRendererUpdater idleRendUp = curGo.AddComponent<HoverIdleRendererUpdater>();
				idleRendUp.IdleRendererPrefab = IdleRendererPrefab;
			}

			follow.Update(); //moves interface to the correct cursor transform
			treeUp.Update(); //force renderer creation
		}

	}

}
