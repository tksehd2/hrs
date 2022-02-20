using System;
using System.Collections.Generic;

namespace Hrs.Gear.Handler
{
	/// <summary>
	/// Gear用Handlerをリストにどういう風に追加するかのクラス
	/// 元はGearへのメソッド登録だったが、Genericが複雑になりすぎるのと、パターン数が多くないということでstaticに変更
	/// </summary>
	public static class AddBehavior
	{
		/// <summary>
		/// メソッドタイプ
		/// </summary>
		public enum eMethodType
		{
			LastOnly,
			AddTail,
			AddHead,
			AddError,
		};

		/// <summary>
		/// 追加メソッド（メソッドタイプを指定する）
		/// </summary>
		public static void Execute<T>(eMethodType methodType, List<T> list, T newProcess)
		{
			switch(methodType)
			{
			case eMethodType.LastOnly:
				LastOnly(list, newProcess);
				break;
			case eMethodType.AddTail:
				AddTail(list, newProcess);
				break;
			case eMethodType.AddHead:
				AddHead(list, newProcess);
				break;
			case eMethodType.AddError:
				AddError(list, newProcess);
				break;
			default:
				throw new Exception("存在しないメソッドタイプです");
			}
		}

		/// <summary>
		/// １つだけ
		/// </summary>
		private static void LastOnly<T>(List<T> list, T newProcess)
		{
			int last = list.Count - 1;
			list.RemoveAt(last);
			list.Add(newProcess);
		}

		/// <summary>
		/// 末尾追加
		/// </summary>
		private static void AddTail<T>(List<T> list, T newProcess)
		{
			list.Add(newProcess);
		}

		/// <summary>
		/// 先頭追加
		/// </summary>
		private static void AddHead<T>(List<T> list, T newProcess)
		{
			list.Insert(0, newProcess);
		}

		/// <summary>
		/// 空なら追加？？？
		/// </summary>
		private static void AddError<T>(List<T> list, T newProcess)
		{
			if (list.Count == 0)
			{
				list.Add(newProcess);
			}
			else
			{
				throw new Exception("このタスクは複数登録出来ません");
			}
		}

	}
}
