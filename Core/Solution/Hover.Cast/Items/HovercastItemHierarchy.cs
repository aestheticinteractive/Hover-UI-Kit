using System;
using Hover.Common.Items;
using Hover.Common.Items.Groups;
using Hover.Common.Util;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Cast.Items {

	/*================================================================================================*/
	public class HovercastItemHierarchy : MonoBehaviour { 

		[Serializable]
		public class LevelChangeEventHandler : UnityEvent<int> {}
		
		[Serializable]
		public class ItemSelectionEventHandler : UnityEvent<IItemGroup, ISelectableItem> {}
		
		public string Title = "Hovercast VR";
		public LevelChangeEventHandler OnLevelChanged;
		public ItemSelectionEventHandler OnItemSelected;
		
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
			
			root.OnLevelChanged += (d => {
				if ( OnLevelChanged != null ) {
					OnLevelChanged.Invoke(d);
				}
			});
			
			root.OnItemSelected += ((g,i) => {
				if ( OnItemSelected != null ) {
					OnItemSelected.Invoke(g, i);
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
			vBindTitle.UpdateValuesIfChanged(Root.Title, Title, pForceUpdate);
		}

	}

}
