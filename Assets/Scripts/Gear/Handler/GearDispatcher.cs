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
		/// <param name="addBehavior"> メソッドタイプ </param>
		/// <param name="once"> １回だけ行うかどうか </param>
		/// <param name="pos"> 位置確認用クラス </param>
		/// </summary>
		public GearDispatcher(AddBehavior.eMethodType addBehavior, bool once, PosInfos pos)
		{
			GenericGearDispatcher = new GenericGearDispatcher<Action>(addBehavior, once, pos);
		}

		/// <summary>
		/// 追加（削除用のCancelKeyが返る）
		/// <param name="func"> 実行する処理 </param>
		/// <param name="addPos"> 位置確認用クラス </param>
		/// </summary>
		public CancelKey Add(Action func, PosInfos addPos)
		{
			return GenericGearDispatcher.Add(func, addPos);
		}

		/// <summary>
		/// 削除
		/// <param name="key"> キャンセルキー </param>
		/// </summary>
		public void Remove(CancelKey key)
		{
			GenericGearDispatcher.Remove(key);
		}

		/// <summary>
		/// 実行
		/// <param name="executePos"> 位置確認用クラス </param>
		/// </summary>
		public void Execute(PosInfos executePos)
		{
			GenericGearDispatcher.Execute(Trat, executePos);
		}

		/// <summary>
		/// 実行本体
		/// <param name="handler"> ハンドラー </param>
		/// </summary>
		private void Trat(GearDispatcherHandler<Action> handler)
		{
			handler._func();
		}
	}
}
