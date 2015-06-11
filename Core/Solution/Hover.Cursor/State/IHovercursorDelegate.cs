using System.Collections.ObjectModel;
using Hover.Common.Input;
using Hover.Common.State;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public interface IHovercursorDelegate {

		CursorDomain Domain { get; }
		bool IsCursorInteractionEnabled { get; }
		ReadOnlyCollection<CursorType> ActiveCursorTypes { get; }
		float CursorDisplayStrength { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		ReadOnlyCollection<IBaseItemInteractionState> GetActiveCursorInteractions(
			CursorType pCursorType);

		/*--------------------------------------------------------------------------------------------*/
		ReadOnlyCollection<PlaneData> GetActiveCursorPlanes(CursorType pCursorType);

	}

}
