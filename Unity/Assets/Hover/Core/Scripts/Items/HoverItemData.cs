using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Core.Items {

	/*================================================================================================*/
	public abstract class HoverItemData : MonoBehaviour, IItemData {
		
		private static int ItemCount;

		[Serializable]
		public class EnabledChangedEventHandler : UnityEvent<IItemData> {}

		public int AutoId { get; internal set; }
		public bool IsVisible { get; set; }
		public bool IsAncestryEnabled { get; set; } //TODO: move setter to an "internal" interface
		public bool IsAncestryVisible { get; set; } //TODO: move setter to an "internal" interface

		public EnabledChangedEventHandler OnEnabledChangedEvent = new EnabledChangedEventHandler();

		public event ItemEvents.EnabledChangedHandler OnEnabledChanged;

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
			OnEnabledChanged += (x => { OnEnabledChangedEvent.Invoke(x); });
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
			get {
				return _IsEnabled;
			}
			set {
				if ( _IsEnabled == value ) {
					return;
				}

				_IsEnabled = value;
				OnEnabledChanged(this);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			OnEnabledChanged(this); //TODO: keep this?
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnDisable() {
			OnEnabledChanged(this); //TODO: keep this?
		}

	}

}
