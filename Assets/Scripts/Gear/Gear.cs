using Hrs.Gear.Handler;
using Hrs.Util;
using System;
using System.Collections.Generic;

namespace Hrs.Gear
{
	public enum eGearPhase 
	{
		Create,
		Preparation,
		Run,
		Dispose,
	};

	/// <summary>
	/// diffuserを楽に扱えるようにするためのクラス
	/// こちらも循環参照からのメモリ管理を厳しくするためにIDisposableを持つ
	/// </summary>
	public class Gear : IDisposable 
	{

		/// このGearインスタンスを保持しているクラス（要IGearHolder）
		private IGearHolder _holder;
		/// 子Gearリスト
		private List<Gear> _childGearList;
		/// このギアを保持するインスタンス用のdiffuser
		public Diffuser _diffuser;

		/// handler
		private eGearPhase _phase;

		private GearDispatcher _preparationDispatcher;
		private GearDispatcher _startDispatcher;
		private GearDispatcher _endDispatcher;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Gear(IGearHolder holder) 
		{
			_holder = holder;
			_childGearList = new List<Gear>();
			_diffuser = new Diffuser(this);

			_phase = eGearPhase.Create;

			_preparationDispatcher = new GearDispatcher(AddBehavior.eMethodType.AddTail, true, new PosInfos());
			_startDispatcher = new GearDispatcher(AddBehavior.eMethodType.AddTail, true, new PosInfos());
			_endDispatcher = new GearDispatcher(AddBehavior.eMethodType.AddHead, true, new PosInfos());
		}

		// ==== 以下、diffuse/absorb ====

		/// <summary>
		/// 自身の子Gearを設定しつつ、子Diffuserの親に自身のDiffuserを設定させる
		/// </summary>
		public void AddChildGear(Gear gear) 
		{
			_childGearList.Add(gear); // 子Gearに追加
			gear._diffuser.SetParent(_diffuser); // 子のdiffuserの親を自分に
		}

		/// <summary>
		// 子のDiffuserの親を空に設定したうえで、子Gearをリストからを削除
		/// </summary>
		public void RemoveChildGear(Gear gear) 
		{
			gear._diffuser.SetParent(null); // 子のdiffuserの親を空に
			_childGearList.Remove(gear);
		}

		/// <summary>
		/// diffuse。Diffuserにインスタンスを登録
		/// </summary>
		public void Diffuse(object diffuseInstance, Type clazz) 
		{
			_diffuser.Add(diffuseInstance, clazz);
		}

		/// <summary>
		/// absorb。Diffuserから該当クラスのインスタンスを取得
		/// </summary>
		public T Absorb<T>(PosInfos pos) 
		{
			if (_holder == null) return default(T);
			return _diffuser.Get<T>(pos);
		}

		/// <summary>
		// for Debug
		/// </summary>
		public string DILog() 
		{
			if (_holder == null) return "";
			string ret = "== " + _holder.GetType().FullName + "\n";
			ret += _diffuser.DILog();

			var enumerator = _childGearList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ret += enumerator.Current.DILog() + "\n";
			}

			return ret;
		}

		// ==== 以下、フェーズチェック ====
		public bool CheckPhaseCreate() 
		{
			switch (_phase) 
			{
			case eGearPhase.Create:
				return true;
			case eGearPhase.Preparation:
			case eGearPhase.Run:
			case eGearPhase.Dispose:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		public bool CheckPhaseCanPreparationTool() 
		{
			switch (_phase) 
			{
			case eGearPhase.Preparation:
				return true;
			case eGearPhase.Create:
			case eGearPhase.Run:
			case eGearPhase.Dispose:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		public bool CheckPhaseCanAbsorb() 
		{
			switch (_phase) 
			{
			case eGearPhase.Preparation:
			case eGearPhase.Run:
				return true;
			case eGearPhase.Create:
			case eGearPhase.Dispose:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		public bool CheckPhaseRun() 
		{
			switch (_phase) 
			{
			case eGearPhase.Run:
				return true;
			case eGearPhase.Create:
			case eGearPhase.Preparation:
			case eGearPhase.Dispose:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		public bool CheckPhaseBeforeDispose() 
		{
			switch (_phase) 
			{
			case eGearPhase.Create:
			case eGearPhase.Preparation:
			case eGearPhase.Run:
				return true;
			case eGearPhase.Dispose:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		// ==== 以下、handlerへの追加処理 ====
		public void AddPreparationProcess(Action func, PosInfos pos) 
		{
			_preparationDispatcher.Add(func, pos);
		}

		public void AddStartProcess(Action func, PosInfos pos) 
		{
			_startDispatcher.Add(func, pos);
		}

		public CancelKey AddEndProcess(Action func, PosInfos pos) 
		{
			if (!CheckPhaseBeforeDispose()) 
			{
				throw new Exception("既に消去処理が開始されているため、消去時のハンドラを登録できません" + _phase);
			}
			return _endDispatcher.Add(func, pos);
		}

		public void Initialize() 
		{
			if (!CheckPhaseCreate()) return;

			_phase = eGearPhase.Preparation;
			_preparationDispatcher.Execute(new PosInfos());

			_phase = eGearPhase.Run;
			// タスクが無くなったらrun実行
			_startDispatcher.Execute(new PosInfos());
			_startDispatcher = null;

			var enumerator = _childGearList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Initialize();
			}
		}

		// ==== 以下、IDispose ====
		~Gear() 
		{
			this.Dispose(false);
		}

		public void Dispose() 
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing) 
		{
			if (_phase != eGearPhase.Dispose) 
			{
				if (isDisposing) 
				{
					//
					_holder = null;

					var enumerator = _childGearList.GetEnumerator();
					while (enumerator.MoveNext())
					{
						enumerator.Current.Dispose();
					}

					_endDispatcher.Execute(new PosInfos());// 逆順で実行する
					_endDispatcher = null;

					_childGearList.Clear();
					_diffuser.Dispose();
				}
				_phase = eGearPhase.Dispose;
			}
		}

		// for debug
		public void DebugPrint(string str) 
		{
			UnityEngine.Debug.Log("[" + str + "]:" + this._holder + "(" + _phase + ")");
		}
	}
}
