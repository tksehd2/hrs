﻿using Hrs.Gear;
using Hrs.Util;
using System;

namespace Hrs.Core
{
	public interface IBaseSceneLogic : IGearHolder
	{
		/// <summary>シーンに入る時の処理</summary>
		void Enter();
		/// <summary>シーンから抜け出す時の処理</summary>
		void Exit();
		/// <summary>更新</summary>
		void Update();
		/// <summary>ViewOrderを設定する</summary>
		void SetSceneViewOrder(IBaseSceneViewOrder sceneView);
		/// <summary>コマンド処理</summary>
		void CommandProcess(ICommand command);
	}

	public class BaseSceneLogic<TView> : GearHolder, IBaseSceneLogic where TView : class, IBaseSceneViewOrder
	{
		/// <summary> ゲームロジック </summary>
		protected IGameLogic_ForSceneLogic _gameLogic = null;
		/// <summary> ゲームビュー </summary>
		protected TView _sceneView = default(TView);

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public BaseSceneLogic() : base(false)
		{
		}

		/// <summary>
		/// 初期化
		/// </summary>
		protected override void StartGearProcess()
		{
			base.StartGearProcess();

			_gameLogic = _gear.Absorb<GameLogic>(new PosInfos());

			HrsLog.Debug("BaseSceneLogic Start");
		}

		/// <summary>
		/// シーンに入る時の処理
		/// </summary>
		public virtual void Enter()
		{
		}

		/// <summary>
		/// シーンから抜け出す時の処理
		/// </summary>
		public virtual void Exit()
		{
		}

		/// <summary>
		/// 更新
		/// </summary>
		public virtual void Update()
		{

		}

		/// <summary>
		/// 解除
		/// </summary>
		protected override void EndGearProcess()
		{
			base.EndGearProcess();
			HrsLog.Debug("BaseSceneLogic End");
		}

		/// <summary>
		/// ViewOrderを設定する
		/// </summary>
		public void SetSceneViewOrder(IBaseSceneViewOrder sceneView)
		{
			_sceneView = (TView)sceneView;
		}

		/// <summary>
		/// コマンド処理
		/// </summary>
		public void CommandProcess(ICommand command)
		{
			Type commandType = command.GetType();
			if (commandType == typeof(ButtonCommand))
			{
				ButtonCommand buttonCommand = (ButtonCommand)command;
				ButtonCommandProcess(buttonCommand);
			}
			else
			{
				HrsLog.Error("処理を定義してないコマンドが来た");
			}
		}

		/// <summary>
		/// ボタンのコマンド処理
		/// </summary>
		protected void ButtonCommandProcess(ButtonCommand buttonCommand)
		{
			switch (buttonCommand.TouchKind)
			{
				case EButtonTouchKind.Tap: TapButtonCommandProcess(buttonCommand.Id);	break;
				case EButtonTouchKind.LongHold: LongHoldButtonCommandProcess(buttonCommand.Id); break;
			}
		}

		/// <summary>
		/// ボタンのコマンド処理（Tap）
		/// </summary>
		protected virtual void TapButtonCommandProcess(string commandId)
		{

		}

		/// <summary>
		/// ボタンのコマンド処理（LongHold）
		/// </summary>
		protected virtual void LongHoldButtonCommandProcess(string commandId)
		{

		}
	}
}