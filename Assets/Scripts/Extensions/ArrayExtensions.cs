using System;
namespace Extensions
{
    /// <summary>
    /// 배열 확장 메서드
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// 인덱스로 데이터를 취득, 없으면 default
        /// </summary>
        /// <param name="arr">대상</param>
        /// <param name="idx">인덱스</param>
        /// <param name="default">디폴트값</param>
        /// <typeparam name="T">타입제한없음</typeparam>
        /// <returns>데이터</returns>
        public static T GetOrDefault<T>(this T[] arr, int idx, T @default = default(T))
        {
            return (idx < 0 || idx >= arr.Length) ? @default : arr[idx];
        }

        /// <summary>
        /// 배열을 탐색하면서 액션을 실행
        /// </summary>
        /// <param name="arr">대상</param>
        /// <param name="action">액션</param>
        /// <typeparam name="T">타입제한없음</typeparam>
        public static void ForEach<T>(this T[] arr, Action<T> action)
        {
            for (var i = 0; i < arr?.Length; ++i)
            {
                action?.Invoke(arr[i]);
            }
        }

        /// <summary>
        /// 배열을 탐색하면서 액션을 실행。
        /// </summary>
        /// <param name="arr">대상</param>
        /// <param name="action">액션</param>
        /// <typeparam name="T">타입제한없음</typeparam>
        public static void ForEachIndexed<T>(this T[] arr, Action<T, int> action)
        {
            for (var i = 0; i < arr?.Length; ++i)
            {
                action?.Invoke(arr[i], i);
            }
        }

        /// <summary>
        /// 배열에서 특정값에대한 인덱스를 리턴
        /// </summary>
        /// <param name="arr">배열</param>
        /// <param name="find">찾을 데이터</param>
        /// <typeparam name="T">타입제한없음</typeparam>
        /// <returns>검색결과 인덱스, 없으면 -1</returns>
        public static int FindIndex<T>(this T[] arr, T find)
        {
            for (var i = 0; i < arr?.Length; ++i)
            {
                if (arr[i].Equals(find))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
