using UnityEngine;
using Hrs.Util;

namespace Hrs.Core
{
	/// <summary> 
	/// エントリーポイントとなるGameObject
	/// ここからプログラムが始めるようにする
	/// </summary>
	abstract public class EntryGameObject : MonoBehaviour
	{
		/// <summary> ゲームループ </summary>
		private GameLoop _gameLoop = null;

		/// <summary> ゲームビュー </summary>
		[SerializeField]
		protected GameView _gameView;

		/// <summary> 
		/// 開始処理 
		/// </summary>
		abstract protected void Start();

		/// <summary> 
		/// 初期化
		/// </summary>
		protected void Initialize (ISetting setting) 
		{
			HrsLog.Init(setting.DisplayLogLevel);
			_gameLoop = new GameLoop(setting, _gameView);
			_gameLoop.InitGear();
		}

		/// <summary> 
		/// 更新
		/// </summary>
		void Update()
		{
			_gameLoop.Update();
		}

		/// <summary> 
		/// 修了
		/// </summary>
		void OnApplicationQuit()
		{
			_gameLoop.AllDisposeGear();
			HrsLog.Release();
		}

		/// <summary> 
		/// 現在シーンビューを取ってくる
		/// </summary>
		public BaseSceneView GetCurrentSceneView()
		{
			return _gameLoop.GetCurrentSceneView();
		}
	}
}
