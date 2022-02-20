using Hrs.Gear;
using Hrs.Util;

namespace Hrs.Core
{
	/// <summary>
	/// ゲームロジックInterface（LogicStateChanger用）
	/// </summary>
	public interface IGameLogic_ForLogicStateChanger
	{
		/// <summary>コマンド通知</summary>
		void NotifyCommand(ICommand command);
	}

	/// <summary>
	/// ゲームロジックInterface（SceneLogic用）
	/// </summary>
	public interface IGameLogic_ForSceneLogic
	{
		/// <summary>シーン切り替え</summary>
		void ChangeScene(IBaseSceneLogic nextScene);
	}

	/// <summary>
	/// ゲームロジッククラス
	/// </summary>
	public class GameLogic : GearHolder, IGameLogic_ForLogicStateChanger, IGameLogic_ForSceneLogic
	{
		/// <summary> GameView </summary>
		private IGameViewOrder _gameView = null;
		/// <summary> 現在シーンのLogic </summary>
		private IBaseSceneLogic _currentSceneLogic = null;
		/// <summary> 前のシーンLogic </summary>
		private IBaseSceneLogic _prevSceneLogic = null;

		/// <summary> 
		/// コンストラクタ
		/// <param name="setting"> 設定情報 </param>
		/// </summary>
		public GameLogic(ISetting setting) : base(false)
		{
			// 開始シーン設定
			_currentSceneLogic = setting.StartScene;
			_gear.AddChildGear(_currentSceneLogic.GetGear());
		}

		/// <summary> 
		/// 初期化
		/// </summary>
		protected override void StartGearProcess()
		{
			base.StartGearProcess();
			_gameView = _gear.Absorb<GameView>(new PosInfos());
			IBaseSceneViewOrder sceneView = _gameView.StartUpSceneView(_currentSceneLogic);
			_currentSceneLogic.SetSceneViewOrder(sceneView);

			HrsLog.Debug("Game Logic Start");
		}

		/// <summary> 
		/// 解除
		/// </summary>
		protected override void EndGearProcess()
		{
			base.EndGearProcess();
			HrsLog.Debug("Game Logic End");
		}

		/// <summary> 
		/// 更新
		/// </summary>
		public void Update()
		{
			_currentSceneLogic.Update();
		}

		/// <summary> 
		/// シーン変更
		/// <param name="nextScene"> 次のシーン </param>
		/// </summary>
		public void ChangeScene(IBaseSceneLogic nextScene)
		{
			// 現在のSceneLogicが抜ける時の処理
			_currentSceneLogic.Exit();
			// ギアを解除
			_currentSceneLogic.AllDisposeGear();
			// 現在のSceneLogicを前のSceneLogicに格納する
			_prevSceneLogic = _currentSceneLogic;
			// ギアの親子関係から外す
			_gear.RemoveChildGear(_prevSceneLogic.GetGear());
			// 現在のSceneLogicを新しいものに入れ替える
			_currentSceneLogic = nextScene;
			// SceneView作成
			IBaseSceneViewOrder sceneView = _gameView.SetupSceneView(_currentSceneLogic);
			// SceneViewを設定
			_currentSceneLogic.SetSceneViewOrder(sceneView);
			// 新しいSceneLogicを子供として追加
			_gear.AddChildGear(_currentSceneLogic.GetGear());
			// 現在のSceneLogicのギアの初期化
			_currentSceneLogic.InitGear();
			// 現在のSceneViewのギアの初期化
			sceneView.InitGear();
			// 現在のSceneLogicに入る時の処理
			_currentSceneLogic.Enter();
		}

		/// <summary> 
		/// コマンド通達
		/// <param name="command"> コマンド </param>
		/// </summary>
		public void NotifyCommand(ICommand command)
		{
			_currentSceneLogic.CommandProcess(command);
		}
	}
}