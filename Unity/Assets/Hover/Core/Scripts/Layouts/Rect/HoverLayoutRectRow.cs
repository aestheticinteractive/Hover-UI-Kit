using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Layouts.Rect {

	/*================================================================================================*/
	public class HoverLayoutRectRow : HoverLayoutRectGroup, ILayoutableRect {

		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";

		public enum ArrangementType {
			LeftToRight,
			RightToLeft,
			TopToBottom,
			BottomToTop
		}

		[SerializeField]
		[DisableWhenControlled(DisplaySpecials=true)]
		[FormerlySerializedAs("Arrangement")]
		private ArrangementType _Arrangement = ArrangementType.LeftToRight;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		[FormerlySerializedAs("SizeX")]
		private float _SizeX = 0.4f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		[FormerlySerializedAs("SizeY")]
		private float _SizeY = 0.08f;

		[SerializeField]
		[FormerlySerializedAs("Padding")]
		private HoverLayoutRectPaddingSettings _Padding;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("Anchor")]
		private AnchorType _Anchor = AnchorType.MiddleCenter;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ArrangementType Arrangement {
			get => _Arrangement;
			set => this.UpdateValueWithTreeMessage(ref _Arrangement, value, "Arrangement");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SizeX {
			get => _SizeX;
			set => this.UpdateValueWithTreeMessage(ref _SizeX, value, "SizeX");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SizeY {
			get => _SizeY;
			set => this.UpdateValueWithTreeMessage(ref _SizeY, value, "SizeY");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverLayoutRectPaddingSettings Padding {
			get => _Padding;
			set => this.UpdateValueWithTreeMessage(ref _Padding, value, "Padding");
		}

		/*--------------------------------------------------------------------------------------------*/
		public AnchorType Anchor {
			get => _Anchor;
			set => this.UpdateValueWithTreeMessage(ref _Anchor, value, "Anchor");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			Padding.ClampValues(this);
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
		public bool IsHorizontal {
			get {
				return (Arrangement == ArrangementType.LeftToRight || 
					Arrangement == ArrangementType.RightToLeft);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsReversed {
			get {
				return (Arrangement == ArrangementType.RightToLeft || 
					Arrangement == ArrangementType.TopToBottom);
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
			Vector2 anchorPos = LayoutUtil.GetRelativeAnchorPosition(Anchor);
			float anchorStartX = anchorPos.x*SizeX;
			float anchorStartY = anchorPos.y*SizeY;
			float horizOuterPad = Padding.Left+Padding.Right;
			float vertOuterPad = Padding.Top+Padding.Bottom;
			float betweenSumPad = Padding.Between*(itemCount-1);
			float relSumX = 0;
			float relSumY = 0;
			float elemAvailSizeX;
			float elemAvailSizeY;
			float cellAvailSizeX;
			float cellAvailSizeY;

			if ( isHoriz ) {
				elemAvailSizeX = SizeX-horizOuterPad-betweenSumPad;
				elemAvailSizeY = SizeY-vertOuterPad;
				cellAvailSizeX = SizeX-horizOuterPad;
				cellAvailSizeY = elemAvailSizeY;
			}
			else {
				elemAvailSizeX = SizeX-horizOuterPad;
				elemAvailSizeY = SizeY-vertOuterPad-betweenSumPad;
				cellAvailSizeX = elemAvailSizeX;
				cellAvailSizeY = SizeY-vertOuterPad;
			}

			for ( int i = 0 ; i < itemCount ; i++ ) {
				HoverLayoutRectGroupChild item = vChildItems[i];
				relSumX += item.RelativeSizeX;
				relSumY += item.RelativeSizeY;
			}

			float posX = anchorStartX - (Padding.Right-Padding.Left)/2 -
				(isHoriz ? cellAvailSizeX/2 : 0);
			float posY = anchorStartY - (Padding.Top-Padding.Bottom)/2 -
				(isHoriz ? 0 : cellAvailSizeY/2);

			for ( int i = 0 ; i < itemCount ; i++ ) {
				int childI = (isRev ? itemCount-i-1 : i);
				HoverLayoutRectGroupChild item = vChildItems[childI];
				ILayoutableRect elem = item.Elem;

				Vector3 localPos = elem.transform.localPosition;
				float elemRelSizeX = elemAvailSizeX*item.RelativeSizeX/(isHoriz ? relSumX : 1);
				float elemRelSizeY = elemAvailSizeY*item.RelativeSizeY/(isHoriz ? 1 : relSumY);
				
				localPos.x = posX+(isHoriz ? elemRelSizeX/2 : 0)+
					elemRelSizeX*item.RelativePositionOffsetX;
				localPos.y = posY+(isHoriz ? 0 : elemRelSizeY/2)+
					elemRelSizeY*item.RelativePositionOffsetY;

				posX += (isHoriz ? elemRelSizeX+Padding.Between : 0);
				posY += (isHoriz ? 0 : elemRelSizeY+Padding.Between);

				elem.Controllers.Set(
					SettingsControllerMap.SpecialPrefix+"Transform.localPosition.x", this);
				elem.Controllers.Set(
					SettingsControllerMap.SpecialPrefix+"Transform.localPosition.y", this);

				elem.SetRectLayout(elemRelSizeX, elemRelSizeY, this);
				elem.transform.localPosition = localPos;
			}
		}

	}

}
