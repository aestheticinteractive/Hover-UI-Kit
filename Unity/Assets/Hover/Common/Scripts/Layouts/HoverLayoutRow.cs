using Hover.Common.Layouts;
using Hover.Common.Renderers.Helpers;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Items {

	/*================================================================================================*/
	[ExecuteInEditMode]
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

		[DisableWhenControlled]
		public bool UsedFixedSize = true;
		
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
		public override void Update() {
			base.Update();

			if ( UsedFixedSize ) {
				Controllers.Unset(SizeXName, this);
				Controllers.Unset(SizeYName, this);
				UpdateLayoutWithFixedSize();
			}
			else {
				Controllers.Set(SizeXName, this);
				Controllers.Set(SizeYName, this);
			}
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetLayoutSize(float pSizeX, float pSizeY, ISettingsController pController) {
			if ( UsedFixedSize ) {
				return;
			}

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
			int itemCount = vChildItems.Count;

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
				HoverItemData childItem = vChildItems[childI];
				IRectangleLayoutElement elem = childItem.GetComponent<IRectangleLayoutElement>();

				if ( elem == null ) {
					Debug.LogWarning("Item '"+childItem.gameObject.name+"' does not have a renderer "+
						"that implements '"+typeof(IRectangleLayoutElement).Name+"'.");
					continue;
				}

				float cellPos = cellRowSize*(i-itemCount/2);
				Vector3 localPos = childItem.transform.localPosition;

				if ( isHoriz ) {
					localPos.x = anchorStartX + (cellPos+OuterPadding);
					localPos.y = anchorStartY - OuterPadding;
				}
				else {
					localPos.x = anchorStartX + OuterPadding;
					localPos.y = anchorStartY - (cellPos+OuterPadding);
				}
				
				elem.Controllers.Set("Transform.localPosition.x", this);
				elem.Controllers.Set("Transform.localPosition.y", this);

				elem.SetLayoutSize(itemSizeX, itemSizeY, this);
				childItem.transform.localPosition = localPos;
			}
		}

	}

}
