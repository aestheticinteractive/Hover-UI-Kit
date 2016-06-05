using Hover.Common.Renderers.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Layouts.Rect {

	/*================================================================================================*/
	public class HoverLayoutRectRow : HoverLayoutRectGroup, IRectLayoutable {

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
		public void SetRectLayout(float pSizeX, float pSizeY, ISettingsController pController) {
			Controllers.Set(SizeXName, pController);
			Controllers.Set(SizeYName, pController);

			SizeX = pSizeX;
			SizeY = pSizeY;
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
			int itemCount = vChildItems.Count;

			if ( itemCount == 0 ) {
				return;
			}

			bool isHoriz = IsHorizontal;
			bool isRev = IsReversed;
			Vector2 anchorPos = RendererUtil.GetRelativeAnchorPosition(Anchor);
			float anchorStartX = anchorPos.x*SizeX;
			float anchorStartY = anchorPos.y*SizeY;
			float cellSumPad = OuterPadding*2 - InnerPadding;
			float itemsSumPad = InnerPadding*(itemCount-1) + OuterPadding*2;
			float outerSumPad = OuterPadding*2;
			float relSumX = 0;
			float relSumY = 0;
			float elemAvailSizeX;
			float elemAvailSizeY;
			float cellAvailSizeX;
			float cellAvailSizeY;

			if ( isHoriz ) {
				elemAvailSizeX = SizeX-itemsSumPad;
				elemAvailSizeY = SizeY-outerSumPad;
				cellAvailSizeX = SizeX-cellSumPad;
				cellAvailSizeY = elemAvailSizeY;
			}
			else {
				elemAvailSizeX = SizeX-outerSumPad;
				elemAvailSizeY = SizeY-itemsSumPad;
				cellAvailSizeX = elemAvailSizeX;
				cellAvailSizeY = SizeY-cellSumPad;
			}
			
			for ( int i = 0 ; i < itemCount ; i++ ) {
				HoverLayoutRectGroupChild item = vChildItems[i];
				relSumX += item.RelativeSizeX;
				relSumY += item.RelativeSizeY;
			}
			
			float posX = anchorStartX - (isHoriz ? cellAvailSizeX/2 : 0);
			float posY = anchorStartY - (isHoriz ? 0 : cellAvailSizeY/2);

			for ( int i = 0 ; i < itemCount ; i++ ) {
				int childI = (isRev ? itemCount-i-1 : i);
				HoverLayoutRectGroupChild item = vChildItems[childI];
				IRectLayoutable elem = item.Elem;

				Vector3 localPos = elem.transform.localPosition;
				float elemRelSizeX = elemAvailSizeX*item.RelativeSizeX/(isHoriz ? relSumX : 1);
				float elemRelSizeY = elemAvailSizeY*item.RelativeSizeY/(isHoriz ? 1 : relSumY);
				
				localPos.x = posX+(isHoriz ? (elemRelSizeX+InnerPadding)/2 : 0);
				localPos.y = posY+(isHoriz ? 0 : (elemRelSizeY+InnerPadding)/2);
				
				posX += (isHoriz ? elemRelSizeX+InnerPadding : 0);
				posY += (isHoriz ? 0 : elemRelSizeY+InnerPadding);
				
				elem.Controllers.Set("Transform.localPosition.x", this);
				elem.Controllers.Set("Transform.localPosition.y", this);

				elem.SetRectLayout(elemRelSizeX, elemRelSizeY, this);
				elem.transform.localPosition = localPos;
			}
		}

	}

}
