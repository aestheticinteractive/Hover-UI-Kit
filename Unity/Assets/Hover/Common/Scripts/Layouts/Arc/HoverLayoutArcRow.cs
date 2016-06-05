using Hover.Common.Layouts.Rect;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Layouts.Arc {

	/*================================================================================================*/
	public class HoverLayoutArcRow : HoverLayoutArcGroup, IArcLayoutable, IRectLayoutable {
		
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcAngleName = "ArcAngle";

		public enum ArrangementType {
			Forward,
			Reverse
		}
		
		[DisableWhenControlled(DisplayMessage=true)]
		public ArrangementType Arrangement = ArrangementType.Forward;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float OuterRadius = 10;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float InnerRadius = 4;

		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		public float ArcAngle = 135;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=10)]
		public float RadiusPadding = 0;

		[DisableWhenControlled(RangeMin=0, RangeMax=90)]
		public float AnglePadding = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			UpdateLayoutWithFixedSize();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetArcLayout(float pOuterRadius, float pInnerRadius, 
													float pArcAngle, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);
			Controllers.Set(InnerRadiusName, pController);
			Controllers.Set(ArcAngleName, pController);

			OuterRadius = pOuterRadius;
			InnerRadius = pInnerRadius;
			ArcAngle = pArcAngle;
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetRectLayout(float pSizeX, float pSizeY, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);

			OuterRadius = Mathf.Min(pSizeX, pSizeY)/2;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateLayoutWithFixedSize() {
			int itemCount = vChildItems.Count;

			if ( itemCount == 0 ) {
				return;
			}

			bool isRev = (Arrangement == ArrangementType.Reverse);
			float angleSumPad = AnglePadding*(itemCount-1);// + RadiusPadding*2;
			float relSumArcAngle = 0;
			float paddedOuterRadius = OuterRadius-RadiusPadding;
			float paddedInnerRadius = InnerRadius+RadiusPadding;
			float availAngle = ArcAngle-angleSumPad;
			float angle = -ArcAngle/2;
			
			for ( int i = 0 ; i < itemCount ; i++ ) {
				HoverLayoutArcGroupChild item = vChildItems[i];
				relSumArcAngle += item.RelativeArcAngle;
			}

			for ( int i = 0 ; i < itemCount ; i++ ) {
				int childI = (isRev ? itemCount-i-1 : i);
				HoverLayoutArcGroupChild item = vChildItems[childI];
				IArcLayoutable elem = item.Elem;
				float elemRelAngle = availAngle*item.RelativeArcAngle/relSumArcAngle;
				float relativeInset = (paddedOuterRadius-paddedInnerRadius)*(1-item.RelativeThickness)/2;

				elem.SetArcLayout(
					paddedOuterRadius-relativeInset,
					paddedInnerRadius+relativeInset,
					elemRelAngle,
					this
				);

				elem.Controllers.Set("Transform.localRotation", this);
				elem.transform.localRotation = Quaternion.AngleAxis(angle+elemRelAngle/2, Vector3.back);
				angle += elemRelAngle+AnglePadding;
			}
		}

	}

}
