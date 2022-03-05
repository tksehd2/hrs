using System;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
namespace Hrs.Extensions
{
    /// <summary>
    /// NullSafe확장메서드 클래스
    /// </summary>
    public static class NullSafeExtensions
    {
        /// <summary>
        /// 대상이 null 이면 InvalidDataException를 발생
        /// </summary>
        /// <param name="target">대상</param>
        /// <param name="message">예외메세지</param>
        /// <typeparam name="T">타입제한없음</typeparam>
        [Conditional("UNITY_ASSERTIONS")]
        [AssertionMethod]
        [ContractAnnotation("target:null=>halt")]
        public static void ThrowWhenNull<T>(this T target, string message = "Object is null") where T : class
        {
            switch (target)
            {
                case UnityEngine.Object uo when !uo:
                    throw new InvalidDataException(message);
                case null:
                    throw new InvalidDataException(message);
            }
        }

        /// <summary>
        /// 타겟이 null이 아닐경우에만 액션을 실행, 액션의 파라메터를 결과로 리턴
        /// </summary>
        /// <param name="target">대상</param>
        /// <param name="action">액션</param>
        /// <typeparam name="T">타입제한없음</typeparam>
        /// <typeparam name="TResult">리턴타입 </typeparam>
        /// <returns>TResult타입의 액션의결과</returns>
        public static TResult Let<T, TResult>(this T target, Func<T, TResult> action)
        {
            if (target is UnityEngine.Object uo)
            {
                if (uo && action != null)
                {
                    return action.Invoke(target);
                }
            }
            else
            {
                if (target != null && action != null)
                {
                    return action.Invoke(target);
                }
            }
            return default;
        }

        /// <summary>
        /// 타겟이 null이아닐경우에만 액션을 실행
        /// </summary>
        /// <param name="target">대상</param>
        /// <param name="action">액션</param>
        /// <typeparam name="T">타입제한없음</typeparam>
        public static void Apply<T>(this T target, Action<T> action)
        {
            if (target is UnityEngine.Object uo)
            {
                if (uo)
                {
                    action?.Invoke(target);
                }
            }
            else
            {
                if (target != null)
                {
                    action?.Invoke(target);
                }
            }
        }
    }
}
