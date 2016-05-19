using Hover.Common.Layouts;
using Hover.Common.Renderers.Helpers;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Items {

	/*================================================================================================*/
	public class HoverLayoutRow : HoverLayoutGroup, IRectangleLayoutElement {

		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";

		public enum ArrangementType {
			LeftToRight,
			RightToLeft,
			TopToBottom,
			BottomToTop
		}
		
		[DisableWhenControlled(DisplayMessage=true)]
		public ArrangementType Arrangement = ArrangementType.LeftToRight;

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeX = 40;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeY = 8;

		[DisableWhenControlled(RangeMin=0, RangeMax=10)]
		public float OuterPadding = 0;

		[DisableWhenControlled(RangeMin=0, RangeMax=10)]
		public float InnerPadding = 0;

		[DisableWhenControlled]
		public AnchorType Anchor = AnchorType.MiddleCenter;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			UpdateLayoutWithFixedSize();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetLayoutSize(float pSizeX, float pSizeY, ISettingsController pController) {
			Controllers.Set(SizeXName, pController);
			Controllers.Set(SizeYName, pController);

			SizeX = pSizeX;
			SizeY = pSizeY;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void UnsetLayoutSize(ISettingsController pController) {
			Controllers.Unset(SizeXName, pController);
			Controllers.Unset(SizeYName, pController);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private bool IsHorizontal {
			get {
				return (Arrangement == ArrangementType.LeftToRight || 
					Arrangement == ArrangementType.RightToLeft);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private bool IsReversed {
			get {
				return (Arrangement == ArrangementType.RightToLeft || 
					Arrangement == ArrangementType.BottomToTop);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateLayoutWithFixedSize() {
			int itemCount = vChildElements.Count;

			if ( itemCount == 0 ) {
				return;
			}

			bool isHoriz = IsHorizontal;
			bool isRev = IsReversed;
			Vector2 anchorPos = RendererHelper.GetRelativeAnchorPosition(Anchor);
			float anchorStartX = anchorPos.x*SizeX;
			float anchorStartY = anchorPos.y*SizeY;
			float cellSumPad = OuterPadding*2 - InnerPadding;
			float itemsSumPad = InnerPadding*(itemCount-1) + OuterPadding*2;
			float outerSumPad = OuterPadding*2;
			float itemSizeX;
			float itemSizeY;
			float cellRowSize;

			if ( isHoriz ) {
				itemSizeX = (SizeX-itemsSumPad)/itemCount;
				itemSizeY = SizeY-outerSumPad;
				cellRowSize = (SizeX-cellSumPad)/itemCount;
			}
			else {
				itemSizeX = SizeX-outerSumPad;
				itemSizeY = (SizeY-itemsSumPad)/itemCount;
				cellRowSize = (SizeY-cellSumPad)/itemCount;
			}

			for ( int i = 0 ; i < itemCount ; i++ ) {
				int childI = (isRev ? itemCount-i-1 : i);
				IRectangleLayoutElement childElem = vChildElements[childI];

				if ( childElem == null ) {
					Debug.LogWarning("Item '"+childElem.transform.name+"' does not have a renderer "+
						"that implements '"+typeof(IRectangleLayoutElement).Name+"'.");
					continue;
				}

				float cellPos = cellRowSize*(i-itemCount/2f+0.5f);
				Vector3 localPos = childElem.transform.localPosition;

				if ( isHoriz ) {
					localPos.x = anchorStartX+cellPos;
					localPos.y = anchorStartY;
				}
				else {
					localPos.x = anchorStartX;
					localPos.y = anchorStartY-cellPos;
				}
				
				childElem.Controllers.Set("Transform.localPosition.x", this);
				childElem.Controllers.Set("Transform.localPosition.y", this);

				childElem.SetLayoutSize(itemSizeX, itemSizeY, this);
				childElem.transform.localPosition = localPos;
			}
		}

	}

}
