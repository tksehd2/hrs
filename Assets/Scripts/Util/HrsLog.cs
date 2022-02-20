using System.Collections.Generic;
using System;

namespace Hrs.Util
{
	/// <summary>
	/// ログレベル定義
	/// </summary>
	public static class HrsLogLevelDefine
	{
		/// <summary> デバック </summary>
		public const uint Debug = 1;     // 0001
		/// <summary> 情報 </summary>
		public const uint Info = 2;  // 0010
		/// <summary> 警告 </summary>
		public const uint Warning = 4; // 0100
		/// <summary> エラー </summary>
		public const uint Error = 8;     // 1000
		/// <summary> 全部 </summary>
		public const uint All = Debug | Info | Warning | Error;
	}

	/// <summary>
	/// ログクラス
	/// </summary>
	public static class HrsLog
	{
		/// <summary> 先頭文字（プリピックス） </summary>
		const string PREFIX_ARK_LOG = "HRS";

		/// <summary> 先頭文字Dic </summary>
		static Dictionary<uint, string> _prefixDic = null;

		/// <summary> ログ出力レベル </summary>
		static uint _displayLogLevel = 0;

		/// <summary>
		/// プリピックス取得
		/// </summary>
		/// <param name="colorName"> 色名 </param>
		/// <param name="level"> ログレベル </param>
		/// <returns>プリピックス</returns>
		static string GetLogPrefix(string colorName, string level)
		{
			return String.Format("<b> <color='{0}'>[{1}][{2}] </color> </b>",
				colorName,
				PREFIX_ARK_LOG,
				level);
		}

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="displayLogLevel"> 出力するログレベル </param>
		public static void Init(uint displayLogLevel)
		{
			_prefixDic = new Dictionary<uint, string>()
			{
				{HrsLogLevelDefine.Error, GetLogPrefix ("red", "Error")},
				{HrsLogLevelDefine.Warning,  GetLogPrefix ("yellow", "Warning")},
				{HrsLogLevelDefine.Info,  GetLogPrefix ("green",  "Info")},
				{HrsLogLevelDefine.Debug, GetLogPrefix ("black",  "Debug")},
			};

			_displayLogLevel = displayLogLevel;
		}

		/// <summary>
		/// 解放
		/// </summary>
		public static void Release()
		{
			_prefixDic = null;
		}

		/// <summary>
		/// 情報ログを出す
		/// </summary>
		/// <param name="message"> メッセージ </param>
		public static void Info(string message)
		{
			ShowMessage(message, HrsLogLevelDefine.Info);
		}

		/// <summary>
		/// デバッグログを出す
		/// </summary>
		/// <param name="message"> メッセージ </param>
		public static void Debug(string message)
		{
			ShowMessage(message, HrsLogLevelDefine.Debug);
		}

		/// <summary>
		/// 警告ログを出す
		/// </summary>
		/// <param name="message"> メッセージ </param>
		public static void Warning(string message)
		{
			ShowMessage(message, HrsLogLevelDefine.Warning);
		}

		/// <summary>
		/// エラーログを出す
		/// </summary>
		/// <param name="message"> メッセージ </param>
		public static void Error(string message)
		{
			ShowMessage(message, HrsLogLevelDefine.Error);
		}

		/// <summary>
		/// メッセージを出す
		/// </summary>
		/// <param name="message"> メッセージ </param>
		/// <param name="level"> 出力するログレベル </param>
		private static void ShowMessage(string message, uint level)
		{
			if (!CanShowLogLevel(level))
			{
				return;
			}

			PosInfos posInfoCallLog = new PosInfos(3);
			UnityEngine.Debug.Log(_prefixDic[level] + message + "\n " + posInfoCallLog.ToString() + " \n\n");
		}

		/// <summary>
		/// 出力可能なレベルかチェック
		/// </summary>
		/// <param name="level"> 出力するログレベル </param>
		/// <returns>出力可能かどうか</returns>
		private static bool CanShowLogLevel(uint level)
		{
			return (_displayLogLevel & level) == level;
		}
	}
}
