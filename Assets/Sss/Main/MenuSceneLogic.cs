using Hrs.Core;
using Hrs.Util;

namespace Hrs.Main
{
	public class MenuSceneLogic : BaseSceneLogic<IMenuSceneViewOrder>
	{

		public override void Update()
		{
			base.Update();
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化
		protected override void StartGearProcess()
		{
			base.StartGearProcess();

			HrsLog.Debug("MenuSceneLogic Start");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			HrsLog.Debug("MenuSceneLogic End");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ボタンのコマンド処理（Tap）
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