using Hrs.Gear.Handler;
using Hrs.Util;
using System;
using System.Collections.Generic;

namespace Hrs.Gear
{
	/// <summary> 
	/// ギアのフェーズ
	/// </summary>
	public enum eGearPhase 
	{
		/// <summary> 作成 </summary>
		Create,
		/// <summary> 準備 </summary>
		Preparation,
		/// <summary> 実行 </summary>
		Run,
		/// <summary> 解除 </summary>
		Dispose,
	};

	/// <summary>
	/// diffuserを楽に扱えるようにするためのクラス
	/// こちらも循環参照からのメモリ管理を厳しくするためにIDisposableを持つ
	/// </summary>
	public class Gear : IDisposable 
	{

		/// <summary> このGearインスタンスを保持しているクラス（要IGearHolder） </summary>
		private IGearHolder _holder;
		/// <summary> 子Gearリスト </summary>
		private List<Gear> _childGearList;
		/// <summary> このギアを保持するインスタンス用のdiffuser </summary>
		public Diffuser _diffuser;

		/// <summary> handler </summary>
		private eGearPhase _phase;

		/// <summary> 準備処理 </summary>
		private GearDispatcher _preparationDispatcher;
		/// <summary> 開始処理 </summary>
		private GearDispatcher _startDispatcher;
		/// <summary> 修了処理 </summary>
		private GearDispatcher _endDispatcher;

		/// <summary>
		/// コンストラクタ
		/// <param name="holder"> このGearインスタンスを保持しているクラス </param>
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
		/// <param name="gear"> 追加するギア </param>
		/// </summary>
		public void AddChildGear(Gear gear) 
		{
			_childGearList.Add(gear); // 子Gearに追加
			gear._diffuser.SetParent(_diffuser); // 子のdiffuserの親を自分に
		}

		/// <summary>
		// 子のDiffuserの親を空に設定したうえで、子Gearをリストからを削除
		/// <param name="gear"> 削除するギア </param>
		/// </summary>
		public void RemoveChildGear(Gear gear) 
		{
			gear._diffuser.SetParent(null); // 子のdiffuserの親を空に
			_childGearList.Remove(gear);
		}

		/// <summary>
		/// diffuse。Diffuserにインスタンスを登録
		/// <param name="diffuseInstance"> DiffuserのInstance </param>
		/// <param name="clazz"> クラスタイプ </param>
		/// </summary>
		public void Diffuse(object diffuseInstance, Type clazz) 
		{
			_diffuser.Add(diffuseInstance, clazz);
		}

		/// <summary>
		/// absorb。Diffuserから該当クラスのインスタンスを取得
		/// <param name="pos"> 位置 </param>
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
		/// <summary>
		/// チェック（作成フェーズ）
		/// </summary>
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

		/// <summary>
		/// チェック（準備フェーズ）
		/// </summary>
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

		/// <summary>
		/// チェック（Absorbができるか？）
		/// </summary>
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

		/// <summary>
		/// チェック（実行フェーズ）
		/// </summary>
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

		/// <summary>
		/// チェック（すでに削除処理が走っているか？）
		/// </summary>
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
		/// <summary>
		/// 準備処理追加
		/// <param name="func"> 処理 </param>
		/// <param name="pos"> 位置 </param>
		/// </summary>
		public void AddPreparationProcess(Action func, PosInfos pos) 
		{
			_preparationDispatcher.Add(func, pos);
		}

		/// <summary>
		/// 開始処理追加
		/// <param name="func"> 処理 </param>
		/// <param name="pos"> 位置 </param>
		/// </summary>
		public void AddStartProcess(Action func, PosInfos pos) 
		{
			_startDispatcher.Add(func, pos);
		}

		/// <summary>
		/// 修了処理追加
		/// <param name="func"> 処理 </param>
		/// <param name="pos"> 位置 </param>
		/// </summary>
		public CancelKey AddEndProcess(Action func, PosInfos pos) 
		{
			if (!CheckPhaseBeforeDispose()) 
			{
				throw new Exception("既に消去処理が開始されているため、消去時のハンドラを登録できません" + _phase);
			}
			return _endDispatcher.Add(func, pos);
		}

		/// <summary>
		/// 初期化
		/// </summary>
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
		/// <summary>
		/// 解除
		/// </summary>
		~Gear() 
		{
			this.Dispose(false);
		}

		/// <summary>
		///
		/// </summary>
		public void Dispose() 
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 解除
		/// <param name="isDisposing"> 解除中か </param>
		/// </summary>
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

		/// <summary>
		// 出力（Debug）
		/// </summary>
		public void DebugPrint(string str) 
		{
			UnityEngine.Debug.Log("[" + str + "]:" + this._holder + "(" + _phase + ")");
		}
	}
}
