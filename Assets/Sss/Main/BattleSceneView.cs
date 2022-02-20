using Hrs.Core;
using Hrs.Util;

namespace Hrs.Main
{
	public interface IBattleSceneViewOrder : IBaseSceneViewOrder
	{
	}
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
		/// </summary>
		public override void Render(int deltaFrame)
		{
			base.Render(deltaFrame);
		}
	}
}