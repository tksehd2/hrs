using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hrs.Core
{
	/// <summary> 
	/// Uiのイベント登録クラス
	/// </summary>
	public class UIEventRegister : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		// Todo:このあたり今後UniRx対応する。
		#region 通常ボタン
		/// <summary> 連打防止 </summary>
		private const float BUTTON_LOCK_SEC = 0.333f;
		/// <summary> 押したときの大きさ </summary>
		private const float BUTTON_PRESSED_SCALE = 1.1f;
		/// <summary> 1回のUpdateでどれくらい目的のScaleに近づくか </summary>
		private const float SCALING_CLOSE_RATE = 0.8f;

		/// <summary> 通常ボタンの同時押しは回避しておく </summary>
		private static bool underLock = false;

		/// <summary> ボタン </summary>
		private Button button = null;

		/// <summary> 開始時のボタンスケール </summary>
		private Vector3 startButtonScale = Vector3.one;
		/// <summary> 押したか </summary>
		private bool isPushed = false;
		/// <summary> スケール比率 </summary>
		private float scaleRate = 1f;

		/// <summary> ボタンId </summary>
		[SerializeField]
		private string _buttonId = string.Empty;

		/// <summary> PointerDown時に一緒に呼んでほしいイベントを入れる </summary>
		private Action customPointerDownEvent = null;
		/// <summary> PointerDown時に一緒に呼んでほしいイベントを入れる </summary>
		public Action CustomPointerDownEvent { set { customPointerDownEvent = value; } }

		/// <summary> 押されてスケール中かどうか </summary>
		[SerializeField]
		private bool isEnablePushedScaling = true;
		/// <summary> 押されてスケール中かどうか </summary>
		public bool IsEnablePushedScaling { set { isEnablePushedScaling = value; } }

		/// <summary> 基本的にボタンのScaleは1固定であるが、デバック用にScale調整したいときに、こちらにチェックを入れてScale調整する </summary>
		[SerializeField]
		private bool isChangeButtonScale = false;
		#endregion

		#region 長押しボタン関連
		/// <summary> 長押し </summary>
		[SerializeField]
		private bool isLongTapButton = false;           // 長押しボタンか？

		/// <summary> 長押し判定するための時間 </summary>
		private const float LONG_PRESS_INTERVAL_SEC = 0.7f;
		/// <summary> 長押し用のAction </summary>
		public Action onLongPress;
		/// <summary> 押した時間数えるためのカウンター </summary>
		private float pressingSecCounter = 0f;
		/// <summary> 長押し可能か </summary>
		private bool isEnabledLongPress = true;
		/// <summary> 押しているか </summary>
		private bool isPressing = false;
		#endregion

		/// <summary> 
		/// 開始
		/// </summary>
		void Start()
		{
			// GameMainObjectの取得
			GameObject go = GameObject.FindWithTag("MainGameObject");
			EntryGameObject mainGameObject = go.GetComponent<EntryGameObject>();
			IBaseSceneView_ForUIEventRegister sceneView =  mainGameObject.GetCurrentSceneView();
			// Buttonにイベントセット
			button = this.GetComponent<Button>();

			if (isChangeButtonScale)
			{
				startButtonScale = button.transform.localScale;
			}
			else
			{
				startButtonScale = Vector3.one;
			}

			if (button != null)
			{
				if (isLongTapButton)
				{
					// 特になし
					onLongPress = () =>
					{
						//gameMainObject.OnUILongTapEvent(eUIAction.UILongTapEvent, this.gameObject);
					};
				}
				// タップボタン機能
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(
					() =>
					{
						isPushed = true;
						if (!underLock)
						{
							AutoLock(BUTTON_LOCK_SEC);

							ICommand command = new ButtonCommand(_buttonId, EButtonTouchKind.Tap);
							sceneView.NotifyCommand(command);
						}
					}
				);
				isPushed = false;
			}
		}

		/// <summary> 
		/// 更新
		/// </summary>
		void Update()
		{
			if (isLongTapButton)
			{
				UpdateLongTapButton();
			}
			else
			{
				UpdateNormalButton();
			}
		}

		/// <summary> 
		/// 更新（通常ボタン）
		/// </summary>
		private void UpdateNormalButton()
		{
			if (!isEnablePushedScaling)
			{
				return;
			}

			if (isPushed)
			{
				scaleRate = (scaleRate * (1.0f - SCALING_CLOSE_RATE)) + (BUTTON_PRESSED_SCALE * SCALING_CLOSE_RATE);
			}
			else
			{
				scaleRate = (scaleRate * (1.0f - SCALING_CLOSE_RATE)) + (1.0f * SCALING_CLOSE_RATE);
			}

			if (button != null && button.IsInteractable())
			{
				button.transform.localScale = startButtonScale * scaleRate;
			}
		}

		/// <summary>  
		/// 更新（タップボタン）
		/// </summary>
		private void UpdateLongTapButton()
		{
			if (isPressing && isEnabledLongPress)
			{
				pressingSecCounter += Time.deltaTime;
				if (LONG_PRESS_INTERVAL_SEC <= pressingSecCounter)
				{
					isEnabledLongPress = false;
					onLongPress.Invoke();
				}
			}
		}

		/// <summary> 
		/// ロック
		/// </summary>
		private IEnumerator AutoLock(float sec)
		{
			// ロック
			underLock = true;
			button.enabled = false;

			// 待ち
			yield return new WaitForSeconds(sec);

			// アンロック
			button.enabled = true;
			underLock = false;
		}

		/// <summary> 
		/// 押した時の処理
		/// </summary>
		public void OnPointerDown(PointerEventData eventData)
		{
			if (isLongTapButton)
			{
				OnPointerDown_LongTapButton();
			}
			else
			{
				OnPointerDown_NormalButton();
			}
		}

		/// <summary> 
		/// 離した時の処理
		/// </summary>
		public void OnPointerUp(PointerEventData eventData)
		{
			if (isLongTapButton)
			{
				OnPointerUp_LongTapButton();
			}
			else
			{
				OnPointerUp_NormalButton();
			}
		}

		/// <summary> 
		/// ポインターが領域を離れた時の処理
		/// </summary>
		public void OnPointerExit(PointerEventData eventData)
		{
			if (!isLongTapButton)
			{
				OnPointerExit_NormalButton();
			}
		}

		/// <summary> 
		/// 押したときの処理（通常）
		/// </summary>
		private void OnPointerDown_NormalButton()
		{
			isPushed = true;
			if (customPointerDownEvent != null)
			{
				customPointerDownEvent();
			}
		}

		/// <summary> 
		/// 押したときの処理（長押し）
		/// </summary>
		private void OnPointerDown_LongTapButton()
		{
			isPressing = true;
		}

		/// <summary> 
		/// 離した時の処理（通常）
		/// </summary>
		private void OnPointerUp_NormalButton()
		{
			isPushed = false;
		}

		/// <summary> 
		/// 離した時の処理（長押し）
		/// </summary>
		private void OnPointerUp_LongTapButton()
		{
			pressingSecCounter = 0;
			isEnabledLongPress = true;
			isPressing = false;
		}

		/// <summary> 
		/// ポインターが離れた時の処理
		/// </summary>
		private void OnPointerExit_NormalButton()
		{
			isPushed = false;
		}
	}
}
