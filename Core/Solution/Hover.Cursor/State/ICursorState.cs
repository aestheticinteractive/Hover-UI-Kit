using Hover.Common.Input;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public interface ICursorState {

		CursorType Type { get; }
		bool IsInputAvailable { get; }
		Vector3 Position { get; }
		float Size { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetDisplayStrength(CursorDomain pDomain, float pStrength);

		/*--------------------------------------------------------------------------------------------*/
		//TODO: consider adding a per-domain "ItemProvider" that requests a new list every Update()
		void AddOrUpdateInteraction(CursorDomain pDomain, IBaseItemInteractionState pItem);

		/*--------------------------------------------------------------------------------------------*/
		void RemoveAllInteractions(CursorDomain pDomain);

		/*--------------------------------------------------------------------------------------------*/
		bool RemoveInteraction(CursorDomain pDomain, IBaseItemInteractionState pItem);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		float GetMaxDisplayStrength();

		/*--------------------------------------------------------------------------------------------*/
		float GetMaxHighlightProgress();

		/*--------------------------------------------------------------------------------------------*/
		float GetMaxSelectionProgress();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetWorldPosition();

	}

}
