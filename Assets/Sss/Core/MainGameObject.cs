using UnityEngine;
using Hrs.Util;

namespace Hrs.Core
{
	public interface IMainGameObject_ForButton
	{

	}


	abstract public class MainGameObject : MonoBehaviour, IMainGameObject_ForButton
	{
		private GameLoop _gameLoop = null;

		[SerializeField]
		protected GameView _gameView;

		// Use this for initialization
		abstract protected void Start();

		protected void Initialize (ISetting setting) 
		{
			HrsLog.Init(setting.DisplayLogLevel);
			_gameLoop = new GameLoop(setting, _gameView);
			_gameLoop.InitGear();
		}

		// Update is called once per frame
		void Update()
		{
			_gameLoop.Update();
		}

		void OnApplicationQuit()
		{
			_gameLoop.AllDisposeGear();
			HrsLog.Release();
		}
		
		public BaseSceneView GetCurrentSceneView()
		{
			return _gameLoop.GetCurrentSceneView();
		}
	}
}
