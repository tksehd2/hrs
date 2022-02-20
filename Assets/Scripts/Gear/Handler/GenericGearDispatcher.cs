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
		/// <summary> Handlerを実行するDispatcherリスト </summary>
		private List<GearDispatcherHandler<TFunc>> _list;

		/// <summary> メソッドタイプ </summary>
		private readonly AddBehavior.eMethodType _methodType;
		/// <summary> 実行ロック </summary>
		private bool _executeLock;
		/// <summary> １回だけ実行するかどうか </summary>
		private readonly bool _once;
		/// <summary> Dispatcherの位置 </summary>
		private readonly PosInfos _dispatcherPos;
		/// <summary> Dispatcherの位置 </summary>
		public PosInfos DispatcherPos { get { return _dispatcherPos; } }

		/// <summary>
		/// 初期化
		/// <param name="methodType"> メソッドタイプ </param>
		/// <param name="once"> １回だけ行うかどうか </param>
		/// <param name="dispatcherPos"> Dispatcherの位置 </param>
		/// </summary>
		public GenericGearDispatcher(AddBehavior.eMethodType methodType, bool once, PosInfos dispatcherPos)
		{
			_methodType = methodType;
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
		/// <param name="func"> 実行する処理 </param>
		/// <param name="addPos"> 追加された時の位置 </param>
		/// </summary>
		public CancelKey Add(TFunc func, PosInfos addPos)
		{
			GearDispatcherHandler<TFunc> handler = new GearDispatcherHandler<TFunc>(func, addPos);
			AddBehavior.Execute(_methodType, _list, handler);
			return handler;
		}

		/// <summary>
		/// Handler削除（追加時に得たCancelKeyを使う）
		/// <param name="key"> キャンセルキー </param>
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
		/// <param name="treat"> 処理するAction </param>
		/// <param name="executePos"> 実行した時の位置 </param>
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