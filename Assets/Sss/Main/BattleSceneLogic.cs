using Hrs.Core;
using Hrs.Util;

namespace Hrs.Main
{
	public class BattleSceneLogic : BaseSceneLogic<IBattleSceneViewOrder>
	{
		/// <summary>
		/// 更新処理
		/// </summary>
		public override void Update()
		{
			base.Update();
		}

		/// <summary>
		/// 初期化
		/// </summary>
		protected override void StartGearProcess()
		{
			base.StartGearProcess();
			
			HrsLog.Debug("BattleSceneLogic Start");
		}

		/// <summary>
		/// 解除
		/// </summary>
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			HrsLog.Debug("BattleSceneLogic End");
		}

		/// <summary>
		/// ボタンのコマンド処理（Tap）
		/// </summary>
		protected override void TapButtonCommandProcess(string commandId)
		{
			base.TapButtonCommandProcess(commandId);

			if (commandId == BattleSceneCommandDefine.GoToMenu)
			{
				_gameLogic.ChangeScene(new MenuSceneLogic());
			}
		}
	}
}