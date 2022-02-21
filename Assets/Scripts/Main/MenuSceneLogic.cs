using Hrs.Core;
using Hrs.Util;

namespace Hrs.Main
{
	/// <summary>
	/// メニューシーンロジック
	/// </summary>
	public class MenuSceneLogic : BaseSceneLogic<IMenuSceneViewOrder>
	{
		/// <summary>
		/// 更新
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

			HrsLog.Debug("MenuSceneLogic Start");
		}

		/// <summary>
		/// 解除
		/// </summary>
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			HrsLog.Debug("MenuSceneLogic End");
		}

		/// <summary>
		/// ボタンのコマンド処理（Tap）
		/// </summary>
		/// <param name="commandId"> コマンドId </param>
		protected override void TapButtonCommandProcess(string commandId)
		{
			base.TapButtonCommandProcess(commandId);

			if (commandId == MenuSceneCommandDefine.GoToBattle)
			{
				_gameLogic.ChangeScene(new BattleSceneLogic());
			}
		}
	}
}