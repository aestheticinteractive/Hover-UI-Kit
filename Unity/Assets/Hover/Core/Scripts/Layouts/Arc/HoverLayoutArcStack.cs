using Hover.Core.Layouts.Rect;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Layouts.Arc {

	/*================================================================================================*/
	public class HoverLayoutArcStack : HoverLayoutArcGroup, ILayoutableArc, ILayoutableRect {
		
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcDegreesName = "ArcDegrees";
		public const string RectAnchorName = "RectAnchor";

		public enum ArrangementType {
			InnerToOuter,
			OuterToInner
		}
		
		[DisableWhenControlled(DisplaySpecials=true)]
		public ArrangementType Arrangement = ArrangementType.InnerToOuter;
		
		[DisableWhenControlled(RangeMin=0)]
		public float OuterRadius = 0.1f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float InnerRadius = 0.04f;

		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		public float ArcDegrees = 135;

		public HoverLayoutArcPaddingSettings Padding = new HoverLayoutArcPaddingSettings();

		[DisableWhenControlled(RangeMin=-180, RangeMax=180)]
		public float StartingDegree = 0;

		[DisableWhenControlled]
		public AnchorType RectAnchor = AnchorType.MiddleCenter;

		private Vector2? vRectSize;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			Padding.ClampValues(this);
			UpdateLayoutWithFixedSize();

			if ( vRectSize == null ) {
				Controllers.Set(RectAnchorName, this);
				RectAnchor = AnchorType.MiddleCenter;
			}

			vRectSize = null;
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetArcLayout(float pOuterRadius, float pInnerRadius, 
												float pArcDegrees, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);
			Controllers.Set(InnerRadiusName, pController);
			Controllers.Set(ArcDegreesName, pController);

			OuterRadius = pOuterRadius;
			InnerRadius = pInnerRadius;
			ArcDegrees = pArcDegrees;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetRectLayout(float pSizeX, float pSizeY, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);

			OuterRadius = Mathf.Min(pSizeX, pSizeY)/2;
			vRectSize = new Vector2(pSizeX, pSizeY);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateLayoutWithFixedSize() {
			int itemCount = vChildItems.Count;

			if ( itemCount == 0 ) {
				return;
			}

			bool isRev = (Arrangement == ArrangementType.OuterToInner);
			float relSumThickness = 0;
			float paddedOuterRadius = OuterRadius-Padding.OuterRadius;
			float paddedInnerRadius = InnerRadius+Padding.InnerRadius;
			float availDeg = ArcDegrees-Padding.StartDegree-Padding.EndDegree;
			float availThick = paddedOuterRadius-paddedInnerRadius-Padding.Between*(itemCount-1);
			float innerRadius = paddedInnerRadius;
			float paddedStartDeg = StartingDegree + (Padding.StartDegree-Padding.EndDegree)/2;

			Vector2 anchorPos = LayoutUtil.GetRelativeAnchorPosition(RectAnchor);
			anchorPos.x *= (vRectSize == null ? OuterRadius*2 : ((Vector2)vRectSize).x);
			anchorPos.y *= (vRectSize == null ? OuterRadius*2 : ((Vector2)vRectSize).y);

			for ( int i = 0 ; i < itemCount ; i++ ) {
				HoverLayoutArcGroupChild item = vChildItems[i];
				relSumThickness += item.RelativeThickness;
			}

			for ( int i = 0 ; i < itemCount ; i++ ) {
				int childI = (isRev ? itemCount-i-1 : i);
				HoverLayoutArcGroupChild item = vChildItems[childI];
				ILayoutableArc elem = item.Elem;
				float elemRelThick = availThick*item.RelativeThickness/relSumThickness;
				float elemRelArcDeg = availDeg*item.RelativeArcDegrees;
				float radiusOffset = elemRelThick*item.RelativeRadiusOffset;
				float elemStartDeg = paddedStartDeg + elemRelArcDeg*item.RelativeStartDegreeOffset;

				elem.SetArcLayout(
					innerRadius+elemRelThick+radiusOffset,
					innerRadius+radiusOffset,
					elemRelArcDeg,
					this
				);
				
				elem.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".x", this);
				elem.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".y", this);
				elem.Controllers.Set(SettingsControllerMap.TransformLocalRotation, this);

				Vector3 localPos = elem.transform.localPosition;
				localPos.x = anchorPos.x;
				localPos.y = anchorPos.y;

				elem.transform.localPosition = localPos;
				elem.transform.localRotation = Quaternion.AngleAxis(elemStartDeg, Vector3.back);

				innerRadius += elemRelThick+Padding.Between;
			}
		}

	}

}
