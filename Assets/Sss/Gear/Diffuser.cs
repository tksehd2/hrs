﻿using System;
using System.Collections.Generic;
using Hrs.Util;

namespace Hrs.Gear
{
	/// <summary>
	/// diffuse, absorbのコア部分
	/// メモリ管理があいまいになるとまずいのでIDisposableを持つ
	/// </summary>
	public class Diffuser : IDisposable
	{
		/// <summary>このdiffuserを持つインスタンス（何か </summary>
		private object _holder;
		/// <summary>親のdiffuser </summary>
		private Diffuser _parent;
		/// <summary>クラス名:インスタンスの辞書 </summary>
		private Dictionary<string, object> _instanceClassDictionary;
		/// <summary>IDispose用フラグ </summary>
		private bool _disposed = false;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Diffuser(object holder)
		{
			_holder = holder;
			_parent = null;
			_instanceClassDictionary = new Dictionary<string, object>();
		}

		/// <summary>
		/// 親のdiffuserの登録
		/// </summary>
		public void SetParent(Diffuser parent)
		{
			_parent = parent;
		}

		/// <summary>
		/// absorb用にインスタンスを登録する
		/// </summary>
		public void Add(object diffuseInstance, Type clazz)
		{
			string className = clazz.ToString();
			if (_instanceClassDictionary.ContainsKey(className))
			{
				throw new Exception(string.Format("既に登録されているクラス{0}を登録しようとしました", className));
			}
			_instanceClassDictionary[className] = diffuseInstance;
		}

		/// <summary>
		/// 登録されたインスタンスを削除（ほぼ使わない）
		/// </summary>
		public void Remove(Type clazz)
		{
			string className = clazz.ToString();
			_instanceClassDictionary.Remove(className);
		}

		/// <summary>
		/// クラス名で指定したインスタンスを取得（初期ステップ）
		/// </summary>
		public T Get<T>(PosInfos pos)
		{
			Type clazz = typeof(T);
			string className = clazz.ToString();
			return GetWithClassName<T>(className, this, pos);
		}

		/// <summary>
		/// 登録されたインスタンスを削除（ほぼ使わない）
		/// </summary>
		private T GetWithClassName<T>(string className, Diffuser startDiffuser, PosInfos pos)
		{
			if (_instanceClassDictionary.ContainsKey(className))
			{
				/// 自分がそのクラスのインスタンスを保持していれば返す
				return (T)_instanceClassDictionary[className];
			}
			else
			{
				/// ないので親に聞いてみる
				if (_parent == null)
				{
					/// 親なしなので見つからない
					throw new Exception(string.Format("指定されたクラス{0}は{1}のDiffuserに登録されていません。;pos={2}", className, startDiffuser._holder, pos));
				}
				return _parent.GetWithClassName<T>(className, startDiffuser, pos);
			}
		}

		/// <summary>
		/// for Debug
		/// </summary>
		public string DILog()
		{
			string ret = "";

			var enumerator = _instanceClassDictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, object> kv = enumerator.Current;
				ret += " - " + kv.Key + "\n";
			}

			ret += "\n";
			return ret;
		}

		// IDisposable method
		~Diffuser()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing)
		{
			if (!_disposed)
			{
				if (isDisposing)
				{
					_holder = null;
					_parent = null;
					_instanceClassDictionary.Clear();
				}
				_disposed = true;
			}
		}
	}
}