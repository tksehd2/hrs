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
		public GearDispatcher(AddBehavior.eMethodType addBehavior, bool once, PosInfos pos)
		{
			GenericGearDispatcher = new GenericGearDispatcher<Action>(addBehavior, once, pos);
		}

		/// <summary>
		/// 追加（削除用のCancelKeyが返る）
		/// </summary>
		public CancelKey Add(Action func, PosInfos addPos)
		{
			return GenericGearDispatcher.Add(func, addPos);
		}

		/// <summary>
		/// 削除
		/// </summary>
		public void Remove(CancelKey key)
		{
			GenericGearDispatcher.Remove(key);
		}

		/// <summary>
		/// 実行
		/// </summary>
		public void Execute(PosInfos executePos)
		{
			GenericGearDispatcher.Execute(Trat, executePos);
		}

		/// <summary>
		/// 実行本体
		/// </summary>
		private void Trat(GearDispatcherHandler<Action> handler)
		{
			handler._func();
		}
	}
}
