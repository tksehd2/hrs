using UnityEngine;
using Hrs.Gear;
using Hrs.Util;

namespace Hrs.Core
{
	public class GameLoop : GearHolder
	{
		private FrameManager _frameManager = null;
		private GameLogic _gameLogic = null;
		private GameView _gameView = null;
		private LogicStateChanger _logicStateChanger = null;

		private float _debugLastUpdateSeconds = 0;
		private float _debugDeltaFrame = 0;
		private int _debugCount = 0;

		public GameLoop(ISetting setting, GameView gameView) : base(true)
		{
			_frameManager = new FrameManager(setting.Fps);
			_gameLogic = new GameLogic(setting);
			_gameView = gameView;
			_gameView.InitDI(false);
			_logicStateChanger = new LogicStateChanger();

			_gear.AddChildGear(_frameManager.GetGear());
			_gear.AddChildGear(_gameLogic.GetGear());
			_gear.AddChildGear(_gameView.GetGear());
			_gear.AddChildGear(_logicStateChanger.GetGear());
		}

		/// <summary> 
		/// 子供にギアを拡散する
		/// </summary>
		protected override void DiffuseGearProcess()
		{
			base.DiffuseGearProcess();
			
			_gear.Diffuse(_frameManager, typeof(FrameManager));
			_gear.Diffuse(_gameLogic, typeof(GameLogic));
			_gear.Diffuse(_gameView, typeof(GameView));
			_gear.Diffuse(_logicStateChanger, typeof(LogicStateChanger));
		}

		/// <summary> 
		/// 初期化
		/// </summary>
		protected override void StartGearProcess()
		{
			base.StartGearProcess();
			HrsLog.Debug("Game Loop Start");

			// FrameManagerの時間初期化
			_frameManager.RecordLastUpdateSeconds();
		}

		/// <summary> 
		/// 解除
		/// </summary>
		protected override void EndGearProcess()
		{
			base.EndGearProcess();
			HrsLog.Debug("Game Loop End");
		}

		/// <summary> 
		/// 更新
		/// </summary>
		public void Update()
		{
			int deltaFrame = _frameManager.CalcDeltaFrame();
			_frameManager.RecordLastUpdateSeconds();
			
			//////////////////// debug ////////////////
			_debugCount += deltaFrame;
			_debugDeltaFrame += (Time.time - _debugLastUpdateSeconds);
			if(_debugDeltaFrame >= 1.0f)
			{
				//HrsLog.Debug("count : "+ _debugCount);
				_debugDeltaFrame = 0.0f;
				_debugCount = 0;
			}
			_debugLastUpdateSeconds = Time.time;
			//////////////////// debug ////////////////

			for (int i = 0; i < deltaFrame; ++i)
			{
				 _gameLogic.Update();
			}

			if(deltaFrame > 0)
			{
				_gameView.Render(deltaFrame);
			}
		}

		/// <summary> 
		/// 現在SceneView取得
		/// </summary>
		public BaseSceneView GetCurrentSceneView()
		{
			return _gameView.GetCurrentSceneView();
		}
	}
}