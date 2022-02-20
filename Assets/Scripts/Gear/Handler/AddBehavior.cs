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
			/// <summary> 最新 </summary>
			LastOnly,
			/// <summary> 後ろに追加 </summary>
			AddTail,
			/// <summary> 先頭に追加 </summary>
			AddHead,
			/// <summary> エラー </summary>
			AddError,
		};

		/// <summary>
		/// 追加メソッド（メソッドタイプを指定する）
		/// </summary>
		/// <param name="methodType"> メソッドタイプ </param>
		/// <param name="newProcess"> 行う処理 </param>
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
		/// <param name="newProcess"> 行う処理 </param>
		private static void LastOnly<T>(List<T> list, T newProcess)
		{
			int last = list.Count - 1;
			list.RemoveAt(last);
			list.Add(newProcess);
		}

		/// <summary>
		/// 末尾追加
		/// </summary>
		/// <param name="newProcess"> 行う処理 </param>
		private static void AddTail<T>(List<T> list, T newProcess)
		{
			list.Add(newProcess);
		}

		/// <summary>
		/// 先頭追加
		/// </summary>
		/// <param name="newProcess"> 行う処理 </param>
		private static void AddHead<T>(List<T> list, T newProcess)
		{
			list.Insert(0, newProcess);
		}

		/// <summary>
		/// 空なら追加？？？
		/// </summary>
		/// <param name="newProcess"> 行う処理 </param>
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
