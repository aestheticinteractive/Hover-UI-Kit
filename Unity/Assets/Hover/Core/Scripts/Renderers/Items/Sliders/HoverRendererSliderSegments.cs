using System.Collections.Generic;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Items.Sliders {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverRendererSlider))]
	[RequireComponent(typeof(HoverShape))]
	public abstract class HoverRendererSliderSegments : MonoBehaviour, ITreeUpdateable {

		public bool IsJumpVisible { get; private set; }

		public List<SliderUtil.SegmentInfo> SegmentInfoList;
		public List<SliderUtil.SegmentInfo> TickInfoList;

		protected SliderUtil.SliderInfo vInfo;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			UpdateInfo();
			UpdateInfoWithShape();
			BuildSegments();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateInfo() {
			HoverRendererSlider rendSlider = gameObject.GetComponent<HoverRendererSlider>();

			vInfo.FillType = rendSlider.FillStartingPoint;
			vInfo.HandleValue = rendSlider.HandleValue;
			vInfo.JumpValue = rendSlider.JumpValue;
			vInfo.ZeroValue = rendSlider.ZeroValue;
			vInfo.TickCount = rendSlider.TickCount;

			/*Debug.Log("INFO: "+info.TrackStartPosition+" / "+info.TrackEndPosition);

			foreach ( SliderUtil.Segment seg in vSegmentInfoList ) {
				Debug.Log(" - "+seg.Type+": "+seg.StartPosition+" / "+seg.EndPosition);
			}*/
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateInfoWithShape();

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void BuildSegments() {
			SliderUtil.CalculateSegments(vInfo, SegmentInfoList);
			SliderUtil.CalculateTicks(vInfo, SegmentInfoList, TickInfoList);
			IsJumpVisible = false;

			for ( int i = 0 ; i < SegmentInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo segInfo = SegmentInfoList[i];

				if ( segInfo.Type == SliderUtil.SegmentType.Jump ) {
					IsJumpVisible = true;
					break;
				}
			}
		}

	}

}
