using System.Collections.Generic;
using System.Linq;
using Hover.Common.Items;
using Hover.Common.Renderers;
using Hover.Common.Styles;
using UnityEngine;

namespace Hover.Common.Components.Items {

	/*================================================================================================*/
	public abstract class HoverBaseItem : MonoBehaviour, IBaseItem {

		private static int ItemCount;

		public int AutoId { get; }

		public string Id {
			get { return vId; }
			set { vId = value; }
		}

		public virtual string Label {
			get { return vLabel; }
			set { vLabel = value; }
		}

		public float Width { get; set; }
		public float Height { get; set; }
		public object DisplayContainer { get; set; } //TODO: move setter to an "internal" interface

		public bool IsEnabled { get; set; }
		public bool IsVisible { get; set; }
		public bool IsAncestryEnabled { get; set; } //TODO: move setter to an "internal" interface
		public bool IsAncestryVisible { get; set; } //TODO: move setter to an "internal" interface

		[SerializeField] //TODO: serialize all item properties...
		protected string vId = "";

		[SerializeField]
		protected string vLabel = "";


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverBaseItem() {
			AutoId = ++ItemCount;
			Id = GetType().Name+AutoId;

			IsEnabled = true;
			IsVisible = true;
			IsAncestryEnabled = true;
			IsAncestryVisible = true;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IItemStyle GetStyle() {
			return gameObject.GetComponents<MonoBehaviour>()
				.OfType<IItemStyle>()
				.FirstOrDefault();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public IHoverItemRenderer GetRenderer() {
			return gameObject.GetComponents<MonoBehaviour>()
				.OfType<IHoverItemRenderer>()
				.FirstOrDefault();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static IBaseItem[] GetChildItems(GameObject pParentGo) {
			Transform tx = pParentGo.transform;
			int childCount = tx.childCount;
			var items = new List<IBaseItem>();

			for ( int i = 0 ; i < childCount ; ++i ) {
				HoverBaseItem hni = tx.GetChild(i).GetComponent<HoverBaseItem>();

				if ( !hni.IsVisible ) {
					continue;
				}

				items.Add(hni);
			}

			return items.ToArray();
		}

	}

}
