using Hover.Common.Input;
using Hover.Common.State;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public interface IHovercursorDelegate {

		CursorDomain Domain { get; }
		bool IsCursorInteractionEnabled { get; }
		CursorType[] ActiveCursorTypes { get; }
		float CursorDisplayStrength { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IBaseItemInteractionState[] GetActiveCursorInteractions(CursorType pCursorType);

		/*--------------------------------------------------------------------------------------------*/
		PlaneData[] GetActiveCursorPlanes(CursorType pCursorType);

	}

}
