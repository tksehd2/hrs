namespace Hrs.Core
{
	/// <summary>
	/// すべてのコマンドの共通Interface
	/// </summary>
	public interface ICommand
	{
	}

	/// <summary>
	/// ボタンのタッチ種類
	/// </summary>
	public enum EButtonTouchKind
	{
		Tap,
		LongHold
	}

	/// <summary>
	/// ボタン用のコマンド
	/// </summary>
	public class ButtonCommand : ICommand
	{
		/// <summary> Id（ボタン識別用） </summary>
		private readonly string _id;
		/// <summary> Id（ボタン識別用） </summary>
		public string Id => _id;
		
		/// <summary> ボタンのタッチ種類 </summary>
		private readonly EButtonTouchKind _touchKind;
		/// <summary> ボタンのタッチ種類 </summary>
		public EButtonTouchKind TouchKind => _touchKind;

		/// <summary> コンストラクタ </summary>
		public ButtonCommand(string id, EButtonTouchKind touchKind)
		{
			_id = id;
			_touchKind = touchKind;
		}
	}


}