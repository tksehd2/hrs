using Hrs.Gear;
using Hrs.Util;
using UnityEngine;

namespace Hrs.Core
{
	public interface IBaseSceneViewOrder : IGearHolder
	{

	}

	public interface IBaseSceneView_ForUIEventRegister
	{
		void NotifyCommand(ICommand command);
	}

	public class BaseSceneView : GearHolderBehavior, IBaseSceneViewOrder, IBaseSceneView_ForUIEventRegister
	{
		private ILogicStateChnager_ForView _logicStateChanger = null;

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
		public virtual void Render(int deltaFrame)
		{

		}

		/// <summary>
		/// コマンド通知
		/// </summary>
		public virtual void NotifyCommand(ICommand command)
		{
			HrsLog.Debug("NotifyCommand");
			_logicStateChanger.NotifyCommand(command);
		}
	}
}