using System.Collections.Generic;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Items.Sliders {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShape))]
	public class HoverFillSlider : HoverFill {

		public const string SegmentInfoName = "SegmentInfo";
		public const int SegmentCount = 4;

		[DisableWhenControlled(DisplaySpecials=true)]
		public HoverRendererSliderSegments SegmentInfo;
		
		[DisableWhenControlled]
		public GameObject TickPrefab;

		[DisableWhenControlled]
		public HoverMesh SegmentA;

		[DisableWhenControlled]
		public HoverMesh SegmentB;

		[DisableWhenControlled]
		public HoverMesh SegmentC;

		[DisableWhenControlled]
		public HoverMesh SegmentD;
		
		[DisableWhenControlled]
		public List<HoverMesh> Ticks;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildMeshCount() {
			return SegmentCount+Ticks.Count;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMesh GetChildMesh(int pIndex) {
			switch ( pIndex ) {
				case 0: return SegmentA;
				case 1: return SegmentB;
				case 2: return SegmentC;
				case 3: return SegmentD;
			}

			return Ticks[pIndex-4];
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			UpdateTickList();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTickList() {
			if ( SegmentInfo == null || SegmentInfo.TickInfoList == null ) {
				return;
			}

			int newTickCount = SegmentInfo.TickInfoList.Count;

			if ( Ticks.Count == newTickCount ) {
				return;
			}

#if UNITY_EDITOR
			//ticks are often added within a prefab; this forces serialization of the "Ticks" list
			UnityEditor.EditorUtility.SetDirty(this);
#endif

			if ( TickPrefab == null ) {
				Debug.LogWarning("Cannot build ticks without a prefab reference.", this);
				return;
			}

			while ( Ticks.Count < newTickCount ) {
				HoverMesh tickMesh = RendererUtil.TryBuildPrefabRenderer<HoverMesh>(TickPrefab);
				tickMesh.name = "Tick"+Ticks.Count;
				tickMesh.transform.SetParent(gameObject.transform, false);
				Ticks.Add(tickMesh);
			}

			while ( Ticks.Count > newTickCount ) {
				int lastTickIndex = Ticks.Count-1;
				HoverMesh tick = Ticks[lastTickIndex];

				Ticks.RemoveAt(lastTickIndex);

				if ( Application.isPlaying ) {
					Destroy(tick.gameObject);
				}
				else {
					tick.gameObject.SetActive(false);
					tick.GetComponent<TreeUpdater>().enabled = false;
					DestroyImmediate(tick.gameObject);
				}
			}

			GetComponent<TreeUpdater>().ImmediateReloadTreeChildren();
		}

	}

}
