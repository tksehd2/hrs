using Hrs.Core;
using Hrs.Util;

namespace Hrs.Main
{
	/// <summary> 
	/// Hrsの設定情報
	/// </summary>
	public class HrsSetting : ISetting
	{
		/// <summary> 開始シーン </summary>
		private readonly IBaseSceneLogic _startScene;
		/// <summary> 開始シーン </summary>
		public IBaseSceneLogic StartScene =>_startScene;

		/// <summary> Frame per sencod (1秒に回るFrame数) </summary>
		public int Fps => 60;

		/// <summary> 表示するログレベル </summary>
		private readonly uint _displayLogLevel = 0;
		/// <summary> 表示するログレベル </summary>
		public uint DisplayLogLevel => _displayLogLevel;

		/// <summary> 
		/// コンストラクタ
		/// </summary>
		public HrsSetting()
		{
			_startScene = new MenuSceneLogic();
			_displayLogLevel = HrsLogLevelDefine.Error | HrsLogLevelDefine.Warning | HrsLogLevelDefine.Info | HrsLogLevelDefine.Debug;
		}
	}
}