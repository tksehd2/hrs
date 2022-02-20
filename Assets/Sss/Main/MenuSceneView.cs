using Hrs.Core;
using Hrs.Util;

namespace Hrs.Main
{
	// TODO: 名称検討(Writer/reader)
	public interface IMenuSceneViewOrder : IBaseSceneViewOrder 
	{
	}

	public class MenuSceneView : BaseSceneView, IMenuSceneViewOrder
	{
		/// <summary>
		/// 初期化関数
		/// </summary>
		protected override void StartGearProcess()
		{
			base.StartGearProcess();

			HrsLog.Debug("MenuSceneView Start");
		}

		/// <summary>
		/// 解除処理
		/// </summary>
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			HrsLog.Debug("MenuSceneView End");
		}

		/// <summary>
		/// 描画処理
		/// </summary>
		public override void Render(int deltaFrame)
		{
			base.Render(deltaFrame);
			//HrsLog.Debug("MenuScene Render");
		}
	}
}
