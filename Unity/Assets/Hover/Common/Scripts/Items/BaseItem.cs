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
		private string _Id;

		[SerializeField]
		private string _Label;

		[SerializeField]
		private float _Width = 1;

		[SerializeField]
		private float _Height = 1;

		[SerializeField]
		private bool _IsEnabled = true;

		[SerializeField]
		private bool _IsVisible = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected BaseItem() {
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
		public float Width {
			get { return _Width; }
			set { _Width = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float Height {
			get { return _Height; }
			set { _Height = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsEnabled {
			get { return _IsEnabled; }
			set { _IsEnabled = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsVisible { //TODO: remove this (in favor of GameObject.activeSelf)
			get { return _IsVisible; }
			set { _IsVisible = value; }
		}

	}

}
