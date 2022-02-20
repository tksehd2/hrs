using DG.Tweening;
using Hrs.UI.System;
using UnityEngine;

namespace Hrs.UI
{
    /// <summary>
    /// タイトル画面のアニメーションを管理
    /// </summary>
    public class UITweenTitle : UITweenBase<UITweenTitle.MotionType>
    {
        /// <summary>
        /// タイトルのモーションタイプ
        /// </summary>
        public enum MotionType
        {
            ///<summary>default</summary>
            Default,
            ///<summary>Inモーション</summary>
            In,
            ///<summary>アウトモーション</summary>
            Out
        }

        ///<summary>初期モーション設定</summary>
        [SerializeField]
        private MotionType InitialMotion = MotionType.Default;

        ///<summary>タイトルオブジェクト</summary>
        [SerializeField]
        private RectTransform Title;

        ///<summary>TouchScreenオブジェクト</summary>
        [SerializeField]
        private RectTransform TouchScreen;

        ///<summary>タイトルのInポジション</summary>
        private Vector2 _titleInPos;

        ///<summary>TouchScreenのInポジション</summary>
        private Vector2 _touchScreenInPos;

        ///<summary>タイトルのOutポジション</summary>
        private readonly Vector2 _titleOutPos = new Vector3(0, 500);

        ///<summary>TouchScreenのOutポジション</summary>
        private readonly Vector2 _touchScreenOutPos = new Vector3(0, -500);

        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            _titleInPos = Title.anchoredPosition;
            _touchScreenInPos = TouchScreen.anchoredPosition;

            SetMotion(InitialMotion);
        }

        /// <summary>
        /// アニメーション実行
        /// </summary>
        /// <param name="completeAction">アニメーション終了コールバック</param>
        protected override void RunMotion(TweenCallback completeAction = null)
        {
            base.RunMotion(completeAction);

            switch (MainMotionType)
            {
                case MotionType.Default:
                    Title.anchoredPosition = _titleOutPos;
                    TouchScreen.anchoredPosition = _touchScreenOutPos;
                    break;
                case MotionType.In:
                    MainSequence.AppendInterval(BaseTime);
                    MainSequence.AppendCallback(() =>
                    {
                        Title.anchoredPosition = _titleOutPos;
                        TouchScreen.anchoredPosition = _touchScreenOutPos;

                        Title.DOAnchorPosY(_titleInPos.y, BaseTime).SetEase(Ease.OutBounce);
                        TouchScreen.DOAnchorPosY(_touchScreenInPos.y, BaseTime);
                    });
                    break;
                case MotionType.Out:
                    MainSequence.AppendInterval(BaseTime);
                    Title.DOAnchorPosY(_titleOutPos.y, BaseTime);
                    TouchScreen.DOAnchorPosY(_touchScreenOutPos.y, BaseTime);
                    break;
            }
        }
    }
}