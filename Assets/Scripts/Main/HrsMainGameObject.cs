using Hrs.Core;

namespace Hrs.Main
{
	/// <summary>
	/// メインオブジェクト
	/// エントリーポイント
	/// </summary>
	public class HrsMainGameObject : EntryGameObject
	{
		/// <summary>
		/// 開始
		/// </summary>
		protected override void Start()
		{
			Initialize(new HrsSetting());
		}
	}
}