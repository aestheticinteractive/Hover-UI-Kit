using UnityEngine;

namespace Hover.Common.Items {

	/*================================================================================================*/
	public abstract class HoverItemData : MonoBehaviour, IBaseItem {
		
		private static int ItemCount;

		public int AutoId { get; internal set; }
		public bool IsVisible { get; set; }
		public bool IsAncestryEnabled { get; set; } //TODO: move setter to an "internal" interface
		public bool IsAncestryVisible { get; set; } //TODO: move setter to an "internal" interface

		[SerializeField]
		private string _Id;

		[SerializeField]
		private string _Label;

		[SerializeField]
		private bool _IsEnabled = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverItemData() {
			AutoId = ++ItemCount;
			Id = "Item-"+AutoId;
			IsAncestryEnabled = true;
			IsAncestryVisible = true;
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string Id {
			get { return _Id; }
			set { _Id = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual string Label {
			get { return _Label; }
			set { _Label = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsEnabled {
			get { return _IsEnabled; }
			set { _IsEnabled = value; }
		}

	}

}
