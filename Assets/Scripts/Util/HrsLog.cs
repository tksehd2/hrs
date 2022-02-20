using System.Collections.Generic;
using System;

namespace Hrs.Util
{
	public static class HrsLogLevelDefine
	{
		public const uint Debug = 1;     // 0001
		public const uint Info = 2;  // 0010
		public const uint Warning = 4; // 0100
		public const uint Error = 8;     // 1000
		public const uint All = Debug | Info | Warning | Error;
	}

	public static class HrsLog
	{
		const string PREFIX_ARK_LOG = "HRS";

		static Dictionary<uint, string> _prefixDict = null;

		static uint _displayLogLevel = 0;

		static string GetLogPrefix(string colorName, string level)
		{
			return String.Format("<b> <color='{0}'>[{1}][{2}] </color> </b>",
				colorName,
				PREFIX_ARK_LOG,
				level);
		}

		static HrsLog()
		{
		}

		public static void Init(uint displayLogLevel)
		{
			_prefixDict = new Dictionary<uint, string>()
			{
				{HrsLogLevelDefine.Error, GetLogPrefix ("red", "Error")},
				{HrsLogLevelDefine.Warning,  GetLogPrefix ("yellow", "Warning")},
				{HrsLogLevelDefine.Info,  GetLogPrefix ("green",  "Info")},
				{HrsLogLevelDefine.Debug, GetLogPrefix ("black",  "Debug")},
			};

			_displayLogLevel = displayLogLevel;
		}

		public static void Release()
		{
			_prefixDict = null;
		}

		public static void Info(string message)
		{
			ShowMessage(message, HrsLogLevelDefine.Info);
		}

		public static void Debug(string message)
		{
			ShowMessage(message, HrsLogLevelDefine.Debug);
		}

		public static void Warning(string message)
		{
			ShowMessage(message, HrsLogLevelDefine.Warning);
		}

		public static void Error(string message)
		{
			ShowMessage(message, HrsLogLevelDefine.Error);
		}

		private static void ShowMessage(string message, uint level)
		{
			if (!CanShowLogLevel(level))
			{
				return;
			}

			PosInfos posInfoCallLog = new PosInfos(3);
			UnityEngine.Debug.Log(_prefixDict[level] + message + "\n " + posInfoCallLog.ToString() + " \n\n");
		}
		
		private static bool CanShowLogLevel(uint level)
		{
			return (_displayLogLevel & level) == level;
		}
	}
}
