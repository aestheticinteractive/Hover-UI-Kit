using System;
using System.Collections.Generic;
using Hover.Common.Items.Types;
using UnityEngine;
using UnityEngine.Events;

namespace Hover.Common.Items.Groups {

	/*================================================================================================*/
	public class HoverItemHierarchy : MonoBehaviour, IItemHierarchy { 

		[Serializable]
		public class LevelChangeEventHandler : UnityEvent<int> {}
		
		[Serializable]
		public class ItemSelectionEventHandler : UnityEvent<IItemGroup, ISelectableItem> {}

		public LevelChangeEventHandler OnLevelChangedEvent;
		public ItemSelectionEventHandler OnItemSelectedEvent;
		
		public event ItemEvents.HierarchyLevelChangedHandler OnLevelChanged;
		public event ItemEvents.GroupItemSelectedHandler OnItemSelected;
		
		[SerializeField]
		private string vTitle = "Hovercast VR";

		private readonly Stack<IItemGroup> vHistory;
		private IItemGroup vCurrLevel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverItemHierarchy() {
			vHistory = new Stack<IItemGroup>();

			OnLevelChanged += (d => { OnLevelChangedEvent.Invoke(d); });
			OnItemSelected += ((l,i) => { OnItemSelectedEvent.Invoke(l, i); });
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			var rootLevel = new ItemGroup(() => HoverBaseItem.GetChildItems(gameObject));
			SetNewLevel(rootLevel, 0);
		}
			

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string Title {
			get { return vTitle; }
			set { vTitle = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public IItemGroup CurrentLevel {
			get {
				return vCurrLevel;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IItemGroup ParentLevel {
			get {
				return (vHistory.Count == 0 ? null : vHistory.Peek());
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public string CurrentLevelTitle {
			get {
				IItemGroup parLevel = ParentLevel;
				return (parLevel == null ? Title : parLevel.LastSelectedItem.Label);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Back() {
			if ( vHistory.Count == 0 ) {
				return;
			}

			SetNewLevel(vHistory.Pop(), -1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleItemSelected(IItemGroup pLevel, ISelectableItem pItem) {
			OnItemSelected(pLevel, pItem);

			IParentItem parItem = (pItem as IParentItem);
			ISelectorItem selectorItem = (pItem as ISelectorItem);

			if ( parItem != null ) {
				vHistory.Push(vCurrLevel);
				SetNewLevel(parItem.ChildGroup, 1);
				return;
			}

			if ( selectorItem != null && selectorItem.NavigateBackUponSelect ) {
				Back();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void SetNewLevel(IItemGroup pNewLevel, int pDirection) {
			if ( vCurrLevel != null ) {
				vCurrLevel.OnItemSelected -= HandleItemSelected;
				vCurrLevel.IsEnabled = false;
			}

			vCurrLevel = pNewLevel;
			vCurrLevel.ReloadActiveItems();
			vCurrLevel.IsEnabled = true;
			vCurrLevel.OnItemSelected += HandleItemSelected;

			OnLevelChanged(pDirection);
		}

	}

}
