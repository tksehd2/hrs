using Hrs.Core;
using Hrs.Util;

namespace Hrs.Main
{
	/// <summary>
	/// MenuSceneViewのInterface（書き込み専用）
	/// </summary>
	public interface IMenuSceneViewOrder : IBaseSceneViewOrder 
	{
	}

	/// <summary>
	/// メニューシーンビュー
	/// </summary>
	public class MenuSceneView : BaseSceneView, IMenuSceneViewOrder
	{
		/// <summary>
		/// 初期化関数
		/// </summary>
		protected override void StartGearProcess()
		{
			base.StartGearProcess();

			HrsLog.Debug("MenuSceneView Start");
		}

		/// <summary>
		/// 解除処理
		/// </summary>
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			HrsLog.Debug("MenuSceneView End");
		}

		/// <summary>
		/// 描画処理
		/// <param name="deltaFrame"> 差分フレーム </param>
		/// </summary>
		public override void Render(int deltaFrame)
		{
			base.Render(deltaFrame);
			//HrsLog.Debug("MenuScene Render");
		}
	}
}
