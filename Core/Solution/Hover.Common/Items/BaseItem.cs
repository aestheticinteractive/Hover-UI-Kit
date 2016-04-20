using System;
using UnityEngine;

namespace Hover.Common.Items {

	/*================================================================================================*/
	[Serializable]
	public abstract class BaseItem : ScriptableObject, IBaseItem {
		
		private static int ItemCount;

		public int AutoId { get; internal set; }
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
		protected BaseItem() {
			AutoId = ++ItemCount;
			Id = "Item-"+AutoId;

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

	}

}
