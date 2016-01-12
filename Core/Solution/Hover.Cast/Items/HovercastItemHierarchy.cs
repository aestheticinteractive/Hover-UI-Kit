using Hover.Common.Components.Items;
using Hover.Common.Items;
using Hover.Common.Items.Groups;
using Hover.Common.Util;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Cast.Items {

	/*================================================================================================*/
	public class HovercastItemHierarchy : MonoBehaviour { 

		public string Title = "Hovercast VR";
		public UnityEvent<int> OnLevelChange;
		public UnityEvent<IItemGroup, ISelectableItem> OnItemSelection;
		
		private readonly ValueBinder<string> vBindTitle;
		private IItemHierarchy vRoot;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HovercastItemHierarchy() {
			vBindTitle = new ValueBinder<string>(
				(x => { vRoot.Title = x; }),
				(x => { Title = x; }),
				ValueBinder.AreStringsEqual
			);
		}
			

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IItemHierarchy Root {
			get {
				if ( vRoot == null ) {
					vRoot = BuildRoot();
				}

				return vRoot;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private IItemHierarchy BuildRoot() {
			var rootLevel = new ItemGroup(() => HoverBaseItem.GetChildItems(gameObject));
			
			var root = new ItemHierarchy();
			root.Build(rootLevel);
			
			root.OnLevelChange += (d => {
				if ( OnLevelChange != null ) {
					OnLevelChange.Invoke(d);
				}
			});
			
			root.OnItemSelection += ((g,i) => {
				if ( OnItemSelection != null ) {
					OnItemSelection.Invoke(g, i);
				}
			});
			
			return root;
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
			vBindTitle.UpdateValuesIfChanged(vRoot.Title, Title, pForceUpdate);
		}

	}

}
