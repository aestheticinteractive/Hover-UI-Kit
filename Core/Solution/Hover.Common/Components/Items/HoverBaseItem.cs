using Hover.Common.Items;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Common.Components.Items {

	/*================================================================================================*/
	public abstract class HoverBaseItem : MonoBehaviour, IBaseItem {

		//TODO: remove the IBaseItem interface, keep the old GetItem() method, keep the ValueBinders

		private int _AutoId;
		public string _Id;
		public string _Label;
		public float _Width;
		public float _Height;

		public bool _IsEnabled;
		public bool _IsVisible;
		private bool _IsAncestryEnabled;
		private bool _IsAncestryVisible;

		private readonly BaseItem vCoreItem;

		private readonly ValueBinder<string> vBindId;
		private readonly ValueBinder<string> vBindLabel;
		private readonly ValueBinder<float> vBindWidth;
		private readonly ValueBinder<float> vBindHeight;

		private readonly ValueBinder<bool> vBindEnabled;
		private readonly ValueBinder<bool> vBindVisible;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverBaseItem(BaseItem pCoreItem) {
			vCoreItem = pCoreItem;
			vCoreItem.DisplayContainer = gameObject;

			vBindId = new ValueBinder<string>(
				(x => { vCoreItem.Id = x; }),
				(x => { _Id = x; }),
				ValueBinder.AreStringsEqual
			);

			vBindLabel = new ValueBinder<string>(
				(x => { vCoreItem.Label = x; }),
				(x => { _Label = x; }),
				ValueBinder.AreStringsEqual
			);

			vBindWidth = new ValueBinder<float>(
				(x => { vCoreItem.Width = x; }),
				(x => { _Width = x; }),
				ValueBinder.AreFloatsEqual
			);

			vBindHeight = new ValueBinder<float>(
				(x => { vCoreItem.Height = x; }),
				(x => { _Height = x; }),
				ValueBinder.AreFloatsEqual
			);

			vBindEnabled = new ValueBinder<bool>(
				(x => { vCoreItem.IsEnabled = x; }),
				(x => { _IsEnabled = x; }),
				ValueBinder.AreBoolsEqual
			);

			vBindVisible = new ValueBinder<bool>(
				(x => { vCoreItem.IsVisible = x; }),
				(x => { _IsVisible = x; }),
				ValueBinder.AreBoolsEqual
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public int AutoId {
			get {
				return vCoreItem.AutoId;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public string Id {
			get {
				return vCoreItem.Id;
			}
			set {
				vBindId.UpdateValuesIfChanged(value);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual string Label {
			get {
				return vCoreItem.Label;
			}
			set {
				vBindLabel.UpdateValuesIfChanged(value);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float Width {
			get {
				return vCoreItem.Width;
			}
			set {
				vBindWidth.UpdateValuesIfChanged(value);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float Height {
			get {
				return vCoreItem.Height;
			}
			set {
				vBindHeight.UpdateValuesIfChanged(value);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public object DisplayContainer {
			get {
				return vCoreItem.DisplayContainer;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool IsEnabled {
			get {
				return vCoreItem.IsEnabled;
			}
			set {
				vBindEnabled.UpdateValuesIfChanged(value);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsVisible {
			get {
				return vCoreItem.IsVisible;
			}
			set {
				vBindVisible.UpdateValuesIfChanged(value);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsAncestryEnabled {
			get {
				return vCoreItem.IsAncestryEnabled;
			}
			set {
				vCoreItem.IsAncestryEnabled = value;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsAncestryVisible {
			get {
				return vCoreItem.IsAncestryVisible;
			}
			set {
				vCoreItem.IsAncestryVisible = value;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			UpdateAllValues(true);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			UpdateAllValues();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateAllValues(bool pForceUpdate=false) {
			_AutoId = vCoreItem.AutoId;
			vBindId.UpdateValuesIfChanged(_Id, pForceUpdate);
			vBindLabel.UpdateValuesIfChanged(_Label, pForceUpdate);
			vBindWidth.UpdateValuesIfChanged(_Width, pForceUpdate);
			vBindHeight.UpdateValuesIfChanged(_Height, pForceUpdate);

			vBindEnabled.UpdateValuesIfChanged(_IsEnabled, pForceUpdate);
			vBindVisible.UpdateValuesIfChanged(_IsVisible, pForceUpdate);
			_IsAncestryEnabled = vCoreItem.IsAncestryEnabled;
			_IsAncestryVisible = vCoreItem.IsAncestryVisible;
		}

	}

}
