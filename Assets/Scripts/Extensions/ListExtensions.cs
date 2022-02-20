using System;
using System.Collections.Generic;
using System.Linq;
namespace Extensions
{
    /// <summary>
    /// 리스트 확장 메서드
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// 인덱스로 값을 취득, 없으면 디폴트값
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="idx">인덱스</param>
        /// <param name="default">디폴트값</param>
        /// <typeparam name="T">타입제한없음</typeparam>
        /// <returns>데이터</returns>
        public static T GetOrDefault<T>(this List<T> list, int idx, T @default = default(T))
        {
            if (list == null || (idx < 0 || idx >= list.Count))
            {
                return @default;
            }

            return list[idx];
        }

        /// <summary>
        /// 리스트를 탐색하면서 액션을 실행
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="action">액션</param>
        /// <typeparam name="T">타입제한없음</typeparam>
        public static void ForEachIndexed<T>(this List<T> list, Action<T, int> action)
        {
            var idx = 0;
            foreach (var item in list)
            {
                action?.Invoke(item, idx);
                ++idx;
            }
        }
    }
}
