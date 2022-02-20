using Hrs.Gear;
using Hrs.Util;

namespace Hrs.Core
{
	public interface IGameViewOrder
	{
		/// <summary> SceneView設定</summary>
		IBaseSceneViewOrder SetupSceneView(IBaseSceneLogic sceneLogic);
		/// <summary> SceneView設定</summary>
		IBaseSceneViewOrder StartUpSceneView(IBaseSceneLogic sceneLogic);
	}

	/// <summary> 
	/// GameViewの基本クラス
	/// 必ずCreateSceneViewをオーバーライドする
	/// </summary>
	abstract public class GameView : GearHolderBehavior, IGameViewOrder
	{
		private BaseSceneView _currentSceneView = null;
		private ILogicStateChnager_ForView _logicStateChanger = null;

		/// <summary> 
		/// 初期化関数
		/// </summary>
		protected override void StartGearProcess()
		{
			base.StartGearProcess();
			_logicStateChanger = _gear.Absorb<LogicStateChanger>(new PosInfos());
			
			HrsLog.Debug("GameView Start");
		}

		/// <summary> 
		/// 解除処理
		/// </summary>
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			HrsLog.Debug("GameView End");
		}

		/// <summary> 
		/// 描画処理
		/// </summary>
		public void Render(int deltaFrame)
		{
			_currentSceneView.Render(deltaFrame);
		}

		/// <summary> 
		/// 現在SceneView取得
		/// </summary>
		public BaseSceneView GetCurrentSceneView()
		{
			return _currentSceneView;
		}

		/// <summary> 
		/// SceneView作成
		/// </summary>
		abstract protected BaseSceneView CreateSceneView(IBaseSceneLogic sceneLogic);

		/// <summary> 
		/// SceneView設定
		/// </summary>
		public IBaseSceneViewOrder StartUpSceneView(IBaseSceneLogic sceneLogic)
		{
			// SceneView生成
			_currentSceneView = CreateSceneView(sceneLogic);
			_currentSceneView.InitDI(false);
			// ギアに追加
			_gear.AddChildGear(_currentSceneView.GetGear());

			return _currentSceneView;
		}

		/// <summary> 
		/// SceneView設定
		/// </summary>
		public IBaseSceneViewOrder SetupSceneView(IBaseSceneLogic sceneLogic)
		{
			// シーンを親から外す
			_currentSceneView.AllDisposeGear();
			_gear.RemoveChildGear(_currentSceneView.GetGear());
			Destroy(_currentSceneView.gameObject);
			
			// SceneView生成
			_currentSceneView = CreateSceneView(sceneLogic);
			_currentSceneView.InitDI(false);
			// ギアに追加
			_gear.AddChildGear(_currentSceneView.GetGear());

			return _currentSceneView;
		}
	}
}