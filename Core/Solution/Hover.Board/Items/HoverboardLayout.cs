using System;
using System.Collections.Generic;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Board.Items {

	/*================================================================================================*/
	public class HoverboardLayout : MonoBehaviour {
		
		public enum AnchorType {
			TopLeft,
			TopCenter,
			TopRight,
			MiddleLeft,
			MiddleCenter,
			MiddleRight,
			BottomLeft,
			BottomCenter,
			BottomRight
		}
		
		public enum DirectionType {
			Left,
			Right,
			Up,
			Down
		}
		
		public AnchorType Anchor;
		public Vector2 Position;
		public DirectionType GrowDirection = DirectionType.Right;
		public bool IsEnabled = true;
		public bool IsVisible = true;

		private ItemLayout vLayout;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IItemLayout GetLayout() {
			if ( vLayout == null ) {
				vLayout = new ItemLayout(GetChildItems);
				vLayout.DisplayContainer = gameObject;
				vLayout.Anchor = GetAnchorVector(Anchor);
				vLayout.Position = Position;
				vLayout.Direction = GetDirectionVector(GrowDirection);
				vLayout.IsEnabled = IsEnabled;
				vLayout.IsVisible = IsVisible;
			}

			return vLayout;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private IBaseItem[] GetChildItems() {
			Transform tx = gameObject.transform;
			int childCount = tx.childCount;
			var items = new List<IBaseItem>();
			
			for ( int i = 0 ; i < childCount ; ++i ) {
				HoverboardItem hni = tx.GetChild(i).GetComponent<HoverboardItem>();
				IBaseItem item = hni.GetItem();

				if ( !item.IsVisible ) {
					continue;
				}

				items.Add(item);
			}

			return items.ToArray();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Vector2 GetAnchorVector(AnchorType pAnchor) {
			switch ( pAnchor ) {
				case AnchorType.TopLeft:
					return new Vector2(0, 0);
					
				case AnchorType.TopCenter:
					return new Vector2(0.5f, 0);
					
				case AnchorType.TopRight:
					return new Vector2(1, 0);
					
				case AnchorType.MiddleLeft:
					return new Vector2(0, 0.5f);
					
				case AnchorType.MiddleCenter:
					return new Vector2(0.5f, 0.5f);
					
				case AnchorType.MiddleRight:
					return new Vector2(1, 0.5f);
					
				case AnchorType.BottomLeft:
					return new Vector2(0, 1);
					
				case AnchorType.BottomCenter:
					return new Vector2(0.5f, 1);
					
				case AnchorType.BottomRight:
					return new Vector2(1, 1);
					
				default:
					throw new Exception("Unhanded AnchorType: "+pAnchor);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static Vector2 GetDirectionVector(DirectionType pDirection) {
			switch ( pDirection ) {
				case DirectionType.Left:
					return new Vector2(-1, 0);
					
				case DirectionType.Right:
					return new Vector2(1, 0);
					
				case DirectionType.Up:
					return new Vector2(0, -1);
					
				case DirectionType.Down:
					return new Vector2(0, 1);
					
				default:
					throw new Exception("Unhanded DirectionType: "+pDirection);
			}
		}

	}

}
