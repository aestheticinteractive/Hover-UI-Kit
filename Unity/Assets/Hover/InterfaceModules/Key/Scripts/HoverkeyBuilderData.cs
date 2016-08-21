using UnityEngine;

namespace Hover.InterfaceModules.Key {

	/*================================================================================================*/
	public static class HoverkeyBuilderData {
		
		public static HoverkeyBuilderKeyInfo[] MainRow0 = {
			HoverkeyBuilderKeyInfo.Char(KeyCode.BackQuote, "`").Shift("~"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Alpha1, "1").Shift("!"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Alpha2, "2").Shift("@"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Alpha3, "3").Shift("#"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Alpha4, "4").Shift("$"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Alpha5, "5").Shift("%"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Alpha6, "6").Shift("^"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Alpha7, "7").Shift("&"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Alpha8, "8").Shift("*"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Alpha9, "9").Shift("("),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Alpha0, "0").Shift(")"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Minus, "-").Shift("_"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Equals, "=").Shift("+"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.Backspace, "Backspace").RelSize(1.75f)
		};

		public static HoverkeyBuilderKeyInfo[] MainRow1 = {
			HoverkeyBuilderKeyInfo.Nav(KeyCode.Tab, "Tab").RelSize(1.75f),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Q, "q").Shift("Q"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.W, "w").Shift("W"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.E, "e").Shift("E"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.R, "r").Shift("R"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.T, "t").Shift("T"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Y, "y").Shift("Y"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.U, "u").Shift("U"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.I, "i").Shift("I"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.O, "o").Shift("O"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.P, "p").Shift("P"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.LeftBracket, "[").Shift("{"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.RightBracket, "]").Shift("}"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Backslash, "\\").Shift("|")
		};

		public static HoverkeyBuilderKeyInfo[] MainRow2 = {
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.CapsLock, "Caps").RelSize(1.95f).Checkbox(),
			HoverkeyBuilderKeyInfo.Char(KeyCode.A, "a").Shift("A"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.S, "s").Shift("S"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.D, "d").Shift("D"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.F, "f").Shift("F"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.G, "g").Shift("G"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.H, "h").Shift("H"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.J, "j").Shift("J"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.K, "k").Shift("K"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.L, "l").Shift("L"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Semicolon, ";").Shift(":"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Quote, "'").Shift("\""),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.Return, "Enter").RelSize(1.8f)
		};

		public static HoverkeyBuilderKeyInfo[] MainRow3 = {
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.LeftShift, "Shift").RelSize(2.375f).Sticky(),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Z, "z").Shift("Z"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.X, "x").Shift("X"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.C, "c").Shift("C"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.V, "v").Shift("V"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.B, "b").Shift("B"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.N, "n").Shift("N"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.M, "m").Shift("M"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Comma, ",").Shift("<"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Period, ".").Shift(">"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Slash, "/").Shift("?"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.RightShift, "Shift").RelSize(2.375f).Sticky()
		};

		public static HoverkeyBuilderKeyInfo[] MainRow4 = {
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.LeftControl, "Ctrl"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.LeftWindows, "Fn"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.LeftAlt, "Alt"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Space, " ").RelSize(4.5f),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.RightAlt, "Alt"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.RightWindows, "OS"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.Menu, "Menu"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.RightControl, "Ctrl")
		};

		public static HoverkeyBuilderKeyInfo[] FuncRow = {
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.Escape, "Esc").RelSize(0.25f),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.F1, "F1"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.F2, "F2"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.F3, "F3"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.F4, "F4"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.F5, "F5"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.F6, "F6"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.F7, "F7"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.F8, "F8"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.F9, "F9"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.F10, "F10"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.F11, "F11"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.F12, "F12")
		};

		public static HoverkeyBuilderKeyInfo[] ThreeRow = {
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.Print, "Print"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.ScrollLock, "Scroll"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.Pause, "Pause")
		};

		public static HoverkeyBuilderKeyInfo[] SixRow0 = {
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.Insert, "Ins"),
			HoverkeyBuilderKeyInfo.Nav(KeyCode.Home, "Home"),
			HoverkeyBuilderKeyInfo.Nav(KeyCode.PageUp, "Page Up")
		};

		public static HoverkeyBuilderKeyInfo[] SixRow1 = {
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.Delete, "Del"),
			HoverkeyBuilderKeyInfo.Nav(KeyCode.End, "End"),
			HoverkeyBuilderKeyInfo.Nav(KeyCode.PageDown, "Page Dn")
		};

		public static HoverkeyBuilderKeyInfo[] Arrows = { //TODO: use arrow icons
			HoverkeyBuilderKeyInfo.Nav(KeyCode.UpArrow, "^"),
			HoverkeyBuilderKeyInfo.Nav(KeyCode.LeftArrow, "<"),
			HoverkeyBuilderKeyInfo.Nav(KeyCode.DownArrow, "v"),
			HoverkeyBuilderKeyInfo.Nav(KeyCode.RightArrow, ">")
		};

		public static HoverkeyBuilderKeyInfo[] NumRow0 = {
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.Numlock, "Num"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.KeypadDivide, "/"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.KeypadMultiply, "*"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.KeypadMinus, "-")
		};

		public static HoverkeyBuilderKeyInfo[] NumRow1 = {
			HoverkeyBuilderKeyInfo.Char(KeyCode.Keypad7, "7"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Keypad8, "8"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Keypad9, "9")
		};

		public static HoverkeyBuilderKeyInfo[] NumRow2 = {
			HoverkeyBuilderKeyInfo.Char(KeyCode.Keypad4, "4"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Keypad5, "5"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Keypad6, "6")
		};

		public static HoverkeyBuilderKeyInfo[] NumRow3 = {
			HoverkeyBuilderKeyInfo.Char(KeyCode.Keypad1, "1"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Keypad2, "2"),
			HoverkeyBuilderKeyInfo.Char(KeyCode.Keypad3, "3")
		};

		public static HoverkeyBuilderKeyInfo[] NumRow4 = {
			HoverkeyBuilderKeyInfo.Char(KeyCode.Keypad0, "0").RelSize(2),
			HoverkeyBuilderKeyInfo.Char(KeyCode.KeypadPeriod, ".")
		};

		public static HoverkeyBuilderKeyInfo[] NumCol = {
			HoverkeyBuilderKeyInfo.Char(KeyCode.KeypadPlus, "+"),
			HoverkeyBuilderKeyInfo.Ctrl(KeyCode.KeypadEnter, "=")
		};

	}

}
