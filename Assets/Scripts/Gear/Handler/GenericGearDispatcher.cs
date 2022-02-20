using Hrs.Util;
using System;
using System.Collections.Generic;

namespace Hrs.Gear.Handler
{
	/// <summary>
	/// TFuncは引数なし→戻り値なし。よってAction。
	/// Gear用Handlerを実行するDispatcher本体
	/// </summary>
	public class GenericGearDispatcher<TFunc>
	{
		private List<GearDispatcherHandler<TFunc>> _list;

		private readonly AddBehavior.eMethodType _addBehavior;
		private bool _executeLock;
		private readonly bool _once;
		private readonly PosInfos _dispatcherPos;
		public  PosInfos DispatcherPos { get { return _dispatcherPos; } }

		/// <summary>
		/// 初期化
		/// </summary>
		public GenericGearDispatcher(AddBehavior.eMethodType addBehavior, bool once, PosInfos dispatcherPos)
		{
			_addBehavior = addBehavior;
			_once = once;
			_dispatcherPos = dispatcherPos;
			
			_executeLock = false;
			_list = new List<GearDispatcherHandler<TFunc>>();
		}

		/// <summary>
		/// 実行すべきHandlerのクリア
		/// </summary>
		private void Clear()
		{
			_list.Clear();
		}

		/// <summary>
		/// Handler追加（削除用のCancelKeyが返る）
		/// </summary>
		public CancelKey Add(TFunc func, PosInfos addPos)
		{
			GearDispatcherHandler<TFunc> handler = new GearDispatcherHandler<TFunc>(func, addPos);
			AddBehavior.Execute(_addBehavior, _list, handler);
			return handler;
		}

		/// <summary>
		/// Handler削除（追加時に得たCancelKeyを使う）
		/// </summary>
		public void Remove(CancelKey key)
		{
			if (_executeLock)
			{
				throw new Exception("実行最中の削除はできません");
			}

			GearDispatcherHandler<TFunc> rmhandler = null;
			var enumerator = _list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				GearDispatcherHandler<TFunc> handler = enumerator.Current;
				if ((CancelKey)handler == key)
				{
					rmhandler = handler;
					break;
				}
			}

			if (rmhandler != null)
			{
				_list.Remove(rmhandler);
			}
		}

		/// <summary>
		/// 実行
		/// 同時実行されたときにおかしくならないようにいったんローカルに対比してから実行している
		/// </summary>
		public void Execute(Action<GearDispatcherHandler<TFunc>> treat, PosInfos executePos)
		{
			if (_executeLock)
			{
				throw new Exception("実行関数が入れ子になっています");
			}

			_executeLock = true;
			List<GearDispatcherHandler<TFunc>> tmpHandlerList = new List<GearDispatcherHandler<TFunc>>(_list);

			if (_once)
			{
				Clear();
			}

			for (int i=0; i<tmpHandlerList.Count; i++)
			{
				treat(tmpHandlerList[i]);
			}

			tmpHandlerList.Clear();
			tmpHandlerList = null;
			_executeLock = false;
		}
	}
}
