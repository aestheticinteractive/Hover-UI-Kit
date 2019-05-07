using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverSceneUpgrader : MonoBehaviour {

		public static readonly string[] ChangedPropertyNames = {
			"Action",
			"ActiveWhenFacingMainCamera",
			"Alignment",
			"AllowIdleDeselection",
			"AllowJump",
			"Alpha",
			"Anchor",
			"ArcDegrees",
			"ArcSegmentsPerDegree",
			"AreItemsEnabled",
			"Arrangement",
			"AutoUvViaRadiusType",
			"AutoUvViaSizeType",
			"CanvasScale",
			"Capability",
			"ControlChildShapes",
			"DepthDistance",
			"DisabledAlpha",
			"DisplayMode",
			"EdgePosition",
			"EdgeThickness",
			"EnabledAlpha",
			"FillStartingPoint",
			"FindOnlyImmediateChildren",
			"FlashColor",
			"FlashColorMilliseconds",
			"FlipLayoutDimensions",
			"FullyActiveWithinDegree",
			"HandleValue",
			"HighlightDistanceMax",
			"HighlightDistanceMin",
			"HighlightProgress",
			"IconSize",
			"IconType",
			"IdleDistanceThreshold",
			"IdleMilliseconds",
			"InactiveOutsideDegree",
			"InnerOffset",
			"InnerRadius",
			"InnerRadiusType",
			"InnerSizeType",
			"InsetInner",
			"InsetLeft",
			"InsetOuter",
			"InsetRight",
			"IsEnabled",
			"IsOpen",
			"IsRaycast",
			"ItemType",
			"JumpValue",
			"LabelFormat",
			"LocalFacingDirection",
			"MasterAlpha",
			"MinHightlightProgress",
			"MinSelectionProgress",
			"OnlyDuringTransitions",
			"OuterOffset",
			"OuterRadius",
			"OuterRadiusType",
			"OuterSizeType",
			"PaddingX",
			"PaddingY",
			"RangeMax",
			"RangeMin",
			"RaycastLocalDirection",
			"RaycastOffsetZ",
			"RectAnchor",
			"RelativeArcDegrees",
			"RelativePositionOffsetX",
			"RelativePositionOffsetY",
			"RelativeRadiusOffset",
			"RelativeSizeX",
			"RelativeSizeY",
			"RelativeStartDegreeOffset",
			"RelativeThickness",
			"RowThickness",
			"Scale",
			"SelectionMilliseconds",
			"SelectionProgress",
			"ShowButtonEdges",
			"ShowEdge",
			"ShowTabE",
			"ShowTabN",
			"ShowTabS",
			"ShowTabW",
			"Size",
			"SizeX",
			"SizeY",
			"SliderFillColor",
			"Snaps",
			"SortingLayer",
			"SortingOrder",
			"StandardColor",
			"StartingDegree",
			"StickyReleaseDistance",
			"TabOutward",
			"TabThickness",
			"TickArcDegrees",
			"TickCount",
			"TickRelativeSizeX",
			"Ticks",
			"TickSizeY",
			"TransitionExponent",
			"TransitionMilliseconds",
			"TransitionProgress",
			"TriggerAgainThreshold",
			"TriggerStrength",
			"UseFollowedCursorType",
			"UseItemSelectionState",
			"UseMirrorLayout",
			"UseTrackUv",
			"UvBottom",
			"UvInnerRadius",
			"UvInset",
			"UvLeft",
			"UvMaxArcDegree",
			"UvMinArcDegree",
			"UvOuterRadius",
			"UvRight",
			"UvTop",
			"ZeroValue"
		};

		//public string HoverRootDir = "C:/Path/To/Unity/Assets/Hover";
		public string ScenesRootDir = "C:/Path/To/Unity/Assets/HoverDemos";
		public bool ClickToUpgradeScenes = false;


#if UNITY_EDITOR
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( ClickToUpgradeScenes ) {
				ClickToUpgradeScenes = false;
				//PrintChangedPropertyNames();
				UpgradeScenes();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		private void PrintChangedPropertyNames() {
			IEnumerable<string> filePaths = Directory.EnumerateFiles(
				HoverRootDir, "*.cs", SearchOption.AllDirectories);
			var propNameMap = new HashSet<string>();
			var propNameList = new List<string>();

			foreach ( string filePath in filePaths ) {
				string text = File.ReadAllText(filePath);
				MatchCollection matches = Regex.Matches(text, @"private\s+\w+\s+_(\w+)\s+=");

				if ( matches.Count == 0 ) {
					continue;
				}

				foreach ( Match match in matches ) {
					string propName = match.Groups[1].Captures[0].Value;

					if ( propNameMap.Add(propName) ) {
						propNameList.Add(propName);
					}
				}
			}

			////

			propNameList.Sort();

			string propNamesStr = "";

			foreach ( string propName in propNameList ) {
				propNamesStr += "\""+propName+"\",\n";
			}

			Debug.Log(propNamesStr);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpgradeScenes() {
			string[] scenePaths = Directory.EnumerateFiles(
				ScenesRootDir, "*.unity", SearchOption.AllDirectories).ToArray();
			Debug.Log("Upgrader found "+scenePaths.Length+" scenes...", this);

			foreach ( string scenePath in scenePaths ) {
				string text = File.ReadAllText(scenePath);
				var scenePropMap = new HashSet<string>();
				int textLen = text.Length;

				foreach ( string propName in ChangedPropertyNames ) {
					MatchCollection matches = Regex.Matches(
						text, "propertyPath: "+propName+@"\b(?!\.Array)");

					foreach ( Match match in matches ) {
						scenePropMap.Add(match.Value);
					}

					text = Regex.Replace(
						text,
						"propertyPath: "+propName+@"\b(?!\.Array)",
						"propertyPath: _"+propName,
						RegexOptions.Multiline
					);
				}

				File.WriteAllText(scenePath, text);

				Debug.Log("Upgraded scene: "+scenePath+
					"\nFrom size "+textLen+" to "+text.Length+
					"\nScene changes..."+
					"\n"+(scenePropMap.Count == 0 ? "<none> " : string.Join("\n", scenePropMap)));
			}
		}
#endif

	}

}
