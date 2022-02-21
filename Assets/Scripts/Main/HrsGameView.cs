using UnityEngine;
using Hrs.Core;

namespace Hrs.Main
{
	/// <summary>
	/// ゲームビュー
	/// </summary>
	public class HrsGameView : GameView
	{
		/// <summary> メニューシーン </summary>
		[SerializeField]
		private MenuSceneView _menuSceneView = null;

		/// <summary> バトルシーン </summary>
		[SerializeField]
		private BattleSceneView _battleSceneView = null;

		/// <summary> 
		/// シーンビュー作成
		/// </summary>
		/// <param name="sceneLogic"> シーンロジック </param>
		/// <returns>作成したシーンビュー</returns>
		protected override BaseSceneView CreateSceneView(IBaseSceneLogicForTypeCheck sceneLogic)
		{
			if (sceneLogic.GetType() == typeof(MenuSceneLogic))
			{
				return Instantiate(_menuSceneView, transform);
			}
			else if(sceneLogic.GetType() == typeof(BattleSceneLogic))
			{
				return Instantiate(_battleSceneView, transform);
			}

			return null;
		}
	}
}