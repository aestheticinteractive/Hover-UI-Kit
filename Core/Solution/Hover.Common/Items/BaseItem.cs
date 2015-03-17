namespace Hover.Common.Items {

	/*================================================================================================*/
	public abstract class BaseItem : IBaseItem {

		public event ItemEvents.IsEnabledChangedHandler OnIsEnabledChanged;
		public event ItemEvents.IsVisibleChangedHandler OnIsVisibleChanged;

		private static int ItemCount;

		public int AutoId { get; private set; }
		public string Id { get; set; }
		public virtual string Label { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public object DisplayContainer { get; set; }

		protected bool vIsEnabled;
		protected bool vIsVisible;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected BaseItem() {
			AutoId = (++ItemCount);
			Id = GetType().Name+AutoId;
			vIsEnabled = true;
			vIsVisible = true;

			OnIsEnabledChanged += (i => { });
			OnIsVisibleChanged += (i => { });
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual bool IsEnabled {
			get {
				return vIsEnabled;
			}
			set {
				if ( value == vIsEnabled ) {
					return;
				}

				vIsEnabled = value;
				OnIsEnabledChanged(this);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual bool IsVisible {
			get {
				return vIsVisible;
			}
			set {
				if ( value == vIsVisible ) {
					return;
				}

				vIsVisible = value;
				OnIsVisibleChanged(this);
			}
		}

	}

}
