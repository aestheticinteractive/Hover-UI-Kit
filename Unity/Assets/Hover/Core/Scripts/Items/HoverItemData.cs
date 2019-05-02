using System;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Core.Items {

	/*================================================================================================*/
	public abstract class HoverItemData : MonoBehaviour, IItemData {
		
		private static int ItemCount;

		[Serializable]
		public class EnabledChangedEventHandler : UnityEvent<IItemData> {}

		public int AutoId { get; internal set; }
		public object Custom { get; set; }

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
			_Id = "Item-"+AutoId;
			OnEnabledChanged += (x => { OnEnabledChangedEvent.Invoke(x); });
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string Id {
			get => _Id;
			set => this.UpdateValueWithTreeMessage(ref _Id, value, "Id");
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual string Label {
			get => _Label;
			set => this.UpdateValueWithTreeMessage(ref _Label, value, "Label");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsEnabled {
			get {
				return _IsEnabled;
			}
			set {
				if ( this.UpdateValueWithTreeMessage(ref _IsEnabled, value, "IsEnabled") ) {
					OnEnabledChanged(this);
				}
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnValidate() {
			TreeUpdater.SendTreeUpdatableChanged(this, "OnValidate");
		}

	}

}
