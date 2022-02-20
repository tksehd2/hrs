using Hrs.Gear;
using Hrs.Util;

namespace Hrs.Core
{
	/// <summary> 
	/// LogicStateChangeのInterface（View用）
	/// </summary>
	public interface ILogicStateChanger_ForView
	{
		/// <summary> コマンド通知</summary>
		void NotifyCommand(ICommand command);
	}

	/// <summary> 
	/// ロジックの状態を変更ための通知を管理するクラス）
	/// </summary>
	public class LogicStateChanger : GearHolder, ILogicStateChanger_ForView
	{
		/// <summary> ゲームロジック </summary>
		private IGameLogic_ForLogicStateChanger _gameLogic = null;

		/// <summary> 
		/// コンストラクター
		/// </summary>
		public LogicStateChanger() : base(false)
		{
		}

		/// <summary> 
		/// ギア開始（初期化にも使える / Absorb可）
		/// </summary>
		protected override void StartGearProcess()
		{
			base.StartGearProcess();

			_gameLogic = _gear.Absorb<GameLogic>(new PosInfos());
		}

		/// <summary> 
		/// コマンド通知
		/// </summary>
		/// <param name="command"> コマンド </param>
		public void NotifyCommand(ICommand command)
		{
			_gameLogic.NotifyCommand(command);
		}
	}
}