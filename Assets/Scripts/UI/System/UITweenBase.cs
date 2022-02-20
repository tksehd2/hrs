using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Hrs.UI.System
{
    /// <summary>
    /// DOTweenアニメーションを管理するベースクラス
    /// </summary>
    /// <typeparam name="TMotionType">Enumのみ</typeparam>
    public abstract class UITweenBase<TMotionType> : MonoBehaviour where TMotionType : Enum
    {
        /// <summary>継承先のMotionType</summary>
        protected TMotionType MainMotionType;

        /// <summary>シーケンス</summary>
        protected Sequence MainSequence;

        /// <summary>基準時間</summary>
        protected const float BaseTime = 0.2f;

        /// <summary>KillMotionされたか</summary>
        private bool _isKill;

        /// <summary>
        /// オブジェクト破棄時にアニメーション破棄
        /// </summary>
        protected virtual void OnDestroy()
        {
            KillMotion();
        }

        /// <summary>
        /// アニメーション設定
        /// </summary>
        /// <param name="motionType">MotionType</param>
        /// <param name="completeAction">終了時コールバック</param>
        public void SetMotion(TMotionType motionType, TweenCallback completeAction = null)
        {
            MainMotionType = motionType;

            if (MainSequence != null)
            {
                if (MainSequence.IsActive() && MainSequence.IsPlaying() && MainSequence.onComplete != null)
                {
                    //再生途中であればonCompleteを実行する
                    MainSequence.onComplete();
                }
            }

            KillMotion();
            RunMotion(completeAction);
        }

        /// <summary>
        /// アニメーション破棄
        /// </summary>
        protected virtual void KillMotion()
        {
            _isKill = true;
            MainSequence.Kill();
        }

        /// <summary>
        /// アニメーション実行
        /// </summary>
        protected virtual void RunMotion(TweenCallback completeAction = null)
        {
            MainSequence = DOTween.Sequence();
            _isKill = false;
            MainSequence.onComplete += completeAction;
        }

        /// <summary>
        /// アニメーション再生中か
        /// </summary>
        /// <returns>プレイ中か</returns>
        public bool IsPlaying()
        {
            if (_isKill) { return false; }
            if (MainSequence.IsActive())
            {
                return MainSequence.IsPlaying();
            }
            return false;
        }

        /// <summary>
        /// 指定モーションタイプの再生判定
        /// </summary>
        /// <param name="motionType">モーションタイプ</param>
        /// <returns>プレイ中か</returns>
        public bool IsPlaying(TMotionType motionType)
        {
            if (!MainMotionType.Equals(motionType))
            {
                return false;
            }
            if (_isKill) { return false; }
            if (MainSequence.IsActive())
            {
                return MainSequence.IsPlaying();
            }
            return false;
        }

        /// <summary>
        /// MotionTypeを取得
        /// </summary>
        /// <returns>MotionType</returns>
        public TMotionType GetMotionType()
        {
            return MainMotionType;
        }

        /// <summary>
        /// MotionTypeの最大値を取得
        /// </summary>
        /// <returns>モーションタイプの最大値</returns>
        public int GetMotionNum()
        {
            return Enum.GetValues(typeof(TMotionType)).Cast<int>().Last();
        }
    }
}
