using System;
using Hrs.Util;

namespace Hrs.Gear.Handler
{
	/// <summary>
	/// Gear用Handlerを実行するDispatcher
	/// 追加・削除・実行
	/// </summary>
	class GearDispatcher
	{
		/// <summary>実行してくれる本体</summary>
		private GenericGearDispatcher<Action> GenericGearDispatcher;

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="addBehavior"> メソッドタイプ </param>
		/// <param name="once"> １回だけ行うかどうか </param>
		/// <param name="pos"> 位置確認用クラス </param>
		public GearDispatcher(AddBehavior.eMethodType addBehavior, bool once, PosInfos pos)
		{
			GenericGearDispatcher = new GenericGearDispatcher<Action>(addBehavior, once, pos);
		}

		/// <summary>
		/// 追加（削除用のCancelKeyが返る）
		/// </summary>
		/// <param name="func"> 実行する処理 </param>
		/// <param name="addPos"> 位置確認用クラス </param>
		public CancelKey Add(Action func, PosInfos addPos)
		{
			return GenericGearDispatcher.Add(func, addPos);
		}

		/// <summary>
		/// 削除
		/// </summary>
		/// <param name="key"> キャンセルキー </param>
		public void Remove(CancelKey key)
		{
			GenericGearDispatcher.Remove(key);
		}

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="executePos"> 位置確認用クラス </param>
		public void Execute(PosInfos executePos)
		{
			GenericGearDispatcher.Execute(Trat, executePos);
		}

		/// <summary>
		/// 実行本体
		/// </summary>
		/// <param name="handler"> ハンドラー </param>
		private void Trat(GearDispatcherHandler<Action> handler)
		{
			handler._func();
		}
	}
}
