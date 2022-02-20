using Hrs.Core;
using Hrs.Util;

namespace Hrs.Main
{
	/// <summary>
	/// BattleSceneViewのInterface（書き込み専用）
	/// </summary>
	public interface IBattleSceneViewOrder : IBaseSceneViewOrder
	{
	}

	/// <summary>
	/// バトルシーンビュークラス
	/// </summary>
	public class BattleSceneView : BaseSceneView, IBattleSceneViewOrder
	{
		/// <summary>
		/// 初期化関数
		/// </summary>
		protected override void StartGearProcess()
		{
			base.StartGearProcess();

			HrsLog.Debug("BattleSceneView Start");
		}

		/// <summary>
		/// 解除処理
		/// </summary>
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			HrsLog.Debug("BattleSceneView End");
		}

		/// <summary>
		/// 描画処理
		/// <param name="deltaFrame"> 差分フレーム </param>
		/// </summary>
		public override void Render(int deltaFrame)
		{
			base.Render(deltaFrame);
		}
	}
}