using Hrs.Util;

namespace Hrs.Gear.Handler
{
	/// <summary>
	/// Gear用Handlerを保持するクラス
	/// 追加された場所も保持
	/// </summary>
	public class GearDispatcherHandler<TFunc> : CancelKey
	{
		/// <summary> 処理行う関数ポインター </summary>
		public TFunc _func;
		/// <summary> 位置確認用クラス </summary>
		public PosInfos _addPos;

		/// <summary> 
		/// コンストラクタ―
		/// <param name="func"> 処理行う関数ポインター </param>
		/// <param name="addPos"> 位置確認用クラス </param>
		/// </summary>
		public GearDispatcherHandler(TFunc func, PosInfos addPos)
		{
			_func = func;
			_addPos = addPos;
		}

		/// <summary> 
		/// String変換
		/// </summary>
		public override string ToString()
		{
			return string.Format("[Handler {0} {1}]", _addPos, typeof(TFunc));
		}
	}

}