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
		public object DisplayContainer { get; set; } //TODO: move setter to an "internal" interface
		public bool IsAncestryEnabled { get; set; } //TODO: move setter to an "internal" interface
		public bool IsAncestryVisible { get; set; } //TODO: move setter to an "internal" interface

		[SerializeField]
		private string vId;

		[SerializeField]
		private string vLabel;

		[SerializeField]
		private float vWidth;

		[SerializeField]
		private float vHeight;

		[SerializeField]
		private bool vIsEnabled;

		[SerializeField]
		private bool vIsVisible;


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
		public string Id {
			get { return vId; }
			set { vId = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual string Label {
			get { return vLabel; }
			set { vLabel = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float Width {
			get { return vWidth; }
			set { vWidth = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float Height {
			get { return vHeight; }
			set { vHeight = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsEnabled {
			get { return vIsEnabled; }
			set { vIsEnabled = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsVisible {
			get { return vIsVisible; }
			set { vIsVisible = value; }
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

		
		////////////////////////////////////////////////////////////////////////////////////////////////
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
