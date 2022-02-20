
namespace Hrs.Core
{
	public interface ISetting
	{
		/// <summary> 開始シーン </summary>
		IBaseSceneLogic StartScene { get; }
		/// <summary> Frame per sencod (1秒に回るFrame数) </summary>
		int Fps { get; }
		/// <summary> 表示するログレベル </summary>
		uint DisplayLogLevel { get; }
	}
}