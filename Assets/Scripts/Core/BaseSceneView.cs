using Hrs.Gear;
using Hrs.Util;

namespace Hrs.Core
{
	/// <summary> 
	/// 基底シーンビューのInterface(読み書き専用)
	/// </summary>
	public interface IBaseSceneViewOrder : IGearHolder
	{

	}

	/// <summary> 
	/// 基底シーンビューのInterface（UIEventRegister用）
	/// </summary>
	public interface IBaseSceneView_ForUIEventRegister
	{
		void NotifyCommand(ICommand command);
	}

	/// <summary> 
	/// 基底シーンビュークラス
	/// </summary>
	public class BaseSceneView : GearHolderBehavior, IBaseSceneViewOrder, IBaseSceneView_ForUIEventRegister
	{
		/// <summary> ロジック状態変更機</summary>
		private ILogicStateChanger_ForView _logicStateChanger = null;

		/// <summary>
		/// 初期化関数
		/// </summary>
		protected override void StartGearProcess()
		{
			base.StartGearProcess();
			_logicStateChanger = _gear.Absorb<LogicStateChanger>(new PosInfos());

			HrsLog.Debug("BaseSceneView Start");
		}

		/// <summary>
		/// 解除処理
		/// </summary>
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			HrsLog.Debug("BaseSceneView End");
		}

		/// <summary>
		/// 描画処理
		/// </summary>
		/// <param name="deltaFrame"> 経過フレーム </param>
		public virtual void Render(int deltaFrame)
		{

		}

		/// <summary>
		/// コマンド通知
		/// </summary>
		/// <param name="command"> コマンド </param>
		public virtual void NotifyCommand(ICommand command)
		{
			HrsLog.Debug("NotifyCommand");
			_logicStateChanger.NotifyCommand(command);
		}
	}
}