using Hrs.Gear;
using Hrs.Util;

namespace Hrs.Core
{
	/// <summary> 
	/// GameViewのInterface（書き込み専用）
	/// </summary>
	public interface IGameViewOrder
	{
		/// <summary> SceneView設定</summary>
		IBaseSceneViewOrder SetupSceneView(IBaseSceneLogicForTypeCheck sceneLogic);
		/// <summary> SceneView設定（最初の開始時）</summary>
		IBaseSceneViewOrder StartUpSceneView(IBaseSceneLogicForTypeCheck sceneLogic);
	}

	/// <summary> 
	/// GameViewの基本クラス
	/// 必ずCreateSceneViewをオーバーライドする
	/// </summary>
	abstract public class GameView : GearHolderBehavior, IGameViewOrder
	{
		/// <summary> 現在のシーンビュー </summary>
		private BaseSceneView _currentSceneView = null;
		/// <summary> ロジック変更機 </summary>
		private ILogicStateChanger_ForView _logicStateChanger = null;

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
		/// <param name="deltaFrame"> フレーム差分 </param>
		public void Render(int deltaFrame)
		{
			_currentSceneView.Render(deltaFrame);
		}

		/// <summary> 
		/// 現在SceneView取得
		/// </summary>
		/// <returns>現在シーンビュー</returns>
		public BaseSceneView GetCurrentSceneView()
		{
			return _currentSceneView;
		}

		/// <summary> 
		/// SceneView作成
		/// </summary>
		/// <param name="sceneLogic"> シーンロジック </param>
		/// <returns>作成されたシーンビュー</returns>
		abstract protected BaseSceneView CreateSceneView(IBaseSceneLogicForTypeCheck sceneLogic);

		/// <summary> 
		/// SceneView設定
		/// </summary>
		/// <param name="sceneLogic"> シーンロジック </param>
		/// <returns>現在シーンビュー</returns>
		public IBaseSceneViewOrder StartUpSceneView(IBaseSceneLogicForTypeCheck sceneLogic)
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
		/// <param name="sceneLogic"> シーンロジック </param>
		/// <returns>現在シーンビュー</returns>
		public IBaseSceneViewOrder SetupSceneView(IBaseSceneLogicForTypeCheck sceneLogic)
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