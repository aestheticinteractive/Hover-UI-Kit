using System;
using UnityEditor;

namespace Hover.Editor {

	/*================================================================================================*/
	public class AutomatedBuilds {

		public const string DemoPath = "Assets/Hover/Demo/";
		public const string BoardKeysPath = DemoPath+"BoardKeys/Scenes/";
		public const string CastCubesPath = DemoPath+"CastCubes/Scenes/";


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void BuildBoardKeysVr() {
			Build(BuildTarget.StandaloneWindows, BoardKeysPath, "HoverboardDemo-LeapVR");
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void BuildBoardKeysNonVr() {
			const string leapHeadScene = "HoverboardDemo-LeapOnly-HeadMount";
			const string leapTableScene = "HoverboardDemo-LeapOnly-TableMount";

			Build(BuildTarget.StandaloneWindows, BoardKeysPath, leapHeadScene);
			Build(BuildTarget.StandaloneWindows, BoardKeysPath, leapTableScene);

			//Build(BuildTarget.StandaloneOSXIntel, BoardKeysPath, leapHeadScene);
			//Build(BuildTarget.StandaloneOSXIntel, BoardKeysPath, leapTableScene);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void BuildCastCubesVr() {
			Build(BuildTarget.StandaloneWindows, CastCubesPath, "HovercastDemo-LeapVR");
			Build(BuildTarget.StandaloneWindows, CastCubesPath, "HovercastDemo-LeapLookVR");
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void BuildCastCubesNonVr() {
			const string leapHeadScene = "HovercastDemo-LeapOnly-HeadMount";
			const string leapTableScene = "HovercastDemo-LeapOnly-TableMount";

			Build(BuildTarget.StandaloneWindows, CastCubesPath, leapHeadScene);
			Build(BuildTarget.StandaloneWindows, CastCubesPath, leapTableScene);

			//Build(BuildTarget.StandaloneOSXIntel, CastCubesPath, leapHeadScene);
			//Build(BuildTarget.StandaloneOSXIntel, CastCubesPath, leapTableScene);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static void Build(BuildTarget pPlatform, string pPath, string pScene) {
			BuildPipeline.BuildPlayer(
				new[] { pPath+pScene+".unity" },
				GetPath(pPlatform, pScene),
				pPlatform,
				BuildOptions.None
			);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private static string GetPath(BuildTarget pPlatform, string pScene) {
			string platformLabel;
			string outputFilename = pScene;

			switch ( pPlatform ) {
				case BuildTarget.StandaloneWindows:
					platformLabel = "PC";
					outputFilename += ".exe";
					break;

				case BuildTarget.StandaloneOSXIntel:
					platformLabel = "Mac";
					break;

				default:
					throw new Exception("Unsupported build target: "+pPlatform);
			}

			string demoGroup = pScene.Substring(0, pScene.IndexOf('-'));
			//string date = DateTime.UtcNow.ToString("yyyy-MM-dd");
			return "../Builds/Auto/"+demoGroup+"-"+/*date+"-"+*/platformLabel+"/"+outputFilename;
		}

	}

}
