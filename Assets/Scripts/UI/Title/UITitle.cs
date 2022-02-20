using UnityEngine;

namespace Hrs.UI
{
    /// <summary>
    /// タイトル画面の制御 
    /// </summary>
    public class UITitle : MonoBehaviour
    {
        /// <summary>
        /// タイトルの状態
        /// </summary>
        private enum TitleState
        {
            ///<summary>Inモーションの再生中</summary>
            PlayInMotion,
            ///<summary>待機</summary>
            Idle,
            ///<summary>Outモーションの再生中</summary>
            PlayOutMotion,
        }
        ///<summary>タイトルのアニメーション</summary>
        [SerializeField]
        private UITweenTitle TweenTitle;

        ///<summary>タイトル画面終了中</summary>
        private TitleState _state;

        /// <summary>
        /// 開始
        /// </summary>
        private void Start()
        {
            _state = TitleState.PlayInMotion;
            TweenTitle.SetMotion(UITweenTitle.MotionType.In, () => _state = TitleState.Idle);
        }

        /// <summary>
        /// スクリーンをタッチしたときのコールバック
        /// </summary>
        private void OnTouchScreen()
        {
            _state = TitleState.PlayOutMotion;
            TweenTitle.SetMotion(UITweenTitle.MotionType.Out, OnTitleOut);
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            if (_state != TitleState.Idle)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                OnTouchScreen();
            }
        }

        /// <summary>
        /// Outモーション終了のコールバック
        /// </summary>
        private void OnTitleOut()
        {
            gameObject.SetActive(false);
        }
    }
}