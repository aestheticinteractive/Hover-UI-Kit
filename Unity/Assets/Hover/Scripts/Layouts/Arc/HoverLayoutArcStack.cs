using Hover.Layouts.Rect;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Layouts.Arc {

	/*================================================================================================*/
	public class HoverLayoutArcStack : HoverLayoutArcGroup, IArcLayoutable, IRectLayoutable {
		
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcAngleName = "ArcAngle";
		public const string RectAnchorName = "RectAnchor";

		public enum ArrangementType {
			InnerToOuter,
			OuterToInner
		}
		
		[DisableWhenControlled(DisplayMessage=true)]
		public ArrangementType Arrangement = ArrangementType.InnerToOuter;
		
		[DisableWhenControlled(RangeMin=0)]
		public float OuterRadius = 0.1f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float InnerRadius = 0.04f;

		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		public float ArcAngle = 135;
		
		[DisableWhenControlled(RangeMin=0)]
		public float RadiusPadding = 0;

		[DisableWhenControlled(RangeMin=0, RangeMax=90)]
		public float AnglePadding = 0;

		[DisableWhenControlled(RangeMin=-180, RangeMax=180)]
		public float StartingAngle = 0;

		[DisableWhenControlled]
		public AnchorType RectAnchor = AnchorType.MiddleCenter;

		private Vector2? vRectSize;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
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
													float pArcAngle, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);
			Controllers.Set(InnerRadiusName, pController);
			Controllers.Set(ArcAngleName, pController);

			OuterRadius = pOuterRadius;
			InnerRadius = pInnerRadius;
			ArcAngle = pArcAngle;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetRectLayout(float pSizeX, float pSizeY, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);

			OuterRadius = Mathf.Min(pSizeX, pSizeY)/2;
			vRectSize = new Vector2(pSizeX, pSizeY);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void DisableAllChildrenExcept(
								IArcLayoutable pIgnoreChildA=null, IArcLayoutable pIgnoreChildB=null) {
			for ( int i = 0 ; i < vChildItems.Count ; i++ ) {
				HoverLayoutArcGroupChild item = vChildItems[i];

				if ( item.Elem == pIgnoreChildA || item.Elem == pIgnoreChildB ) {
					continue;
				}

				item.Elem.transform.gameObject.SetActive(false);
			}
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
			float paddedOuterRadius = OuterRadius-RadiusPadding;
			float paddedInnerRadius = InnerRadius+RadiusPadding;
			float availAngle = ArcAngle-AnglePadding*(itemCount-1);
			float availThick = paddedOuterRadius-paddedInnerRadius;
			float innerRadius = paddedInnerRadius;

			Vector2 anchorPos = RendererUtil.GetRelativeAnchorPosition(RectAnchor);
			anchorPos.x *= (vRectSize == null ? OuterRadius*2 : ((Vector2)vRectSize).x);
			anchorPos.y *= (vRectSize == null ? OuterRadius*2 : ((Vector2)vRectSize).y);

			for ( int i = 0 ; i < itemCount ; i++ ) {
				HoverLayoutArcGroupChild item = vChildItems[i];
				relSumThickness += item.RelativeThickness;
			}

			for ( int i = 0 ; i < itemCount ; i++ ) {
				int childI = (isRev ? itemCount-i-1 : i);
				HoverLayoutArcGroupChild item = vChildItems[childI];
				IArcLayoutable elem = item.Elem;
				float elemRelThick = availThick*item.RelativeThickness/relSumThickness;
				float elemRelArcAngle = availAngle*item.RelativeArcAngle;
				float radiusOffset = elemRelThick*item.RelativeRadiusOffset;
				float elemStartAngle = StartingAngle + elemRelArcAngle*item.RelativeStartAngleOffset;

				elem.SetArcLayout(
					innerRadius+elemRelThick+radiusOffset,
					innerRadius+radiusOffset,
					elemRelArcAngle,
					this
				);
				
				elem.Controllers.Set("Transform.localPosition.x", this);
				elem.Controllers.Set("Transform.localPosition.y", this);
				elem.Controllers.Set("Transform.localRotation", this);

				Vector3 localPos = elem.transform.localPosition;
				localPos.x = anchorPos.x;
				localPos.y = anchorPos.y;

				elem.transform.localPosition = localPos;
				elem.transform.localRotation = Quaternion.AngleAxis(elemStartAngle, Vector3.back);

				innerRadius += elemRelThick;
			}
		}

	}

}
