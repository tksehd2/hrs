using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hrs.Core
{
	public class UIEventRegister : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		#region 通常ボタン
		private const float BUTTON_LOCK_SEC = 0.333f;
		private const float BUTTON_PRESSED_SCALE = 1.1f;
		private const float SCALING_CLOSE_RATE = 0.8f; // 1回のUpdateでどれくらい目的のScaleに近づくか

		// 通常ボタンの同時押しは回避しておく
		private static bool underLock = false;

		private Button button;

		private Vector3 startButtonScale;
		private bool isPushed;
		private float scaleRation = 1f;

		[SerializeField]
		private string _buttonId = string.Empty;

		// PointerDown時に一緒に呼んでほしいイベントを入れる
		private Action customPointerDownEvent = null;
		public Action CustomPointerDownEvent
		{ set { customPointerDownEvent = value; } }
		[SerializeField]
		private bool isEnablePushedScaling = true;
		public bool IsEnablePushedScaling
		{ set { isEnablePushedScaling = value; } }

		// 基本的にボタンのScaleは1固定であるが、デバック用にScale調整したいときに、こちらにチェックを入れてScale調整する
		[SerializeField]
		private bool isChangeButtonScale = false;
		#endregion

		#region 長押しボタン関連
		[SerializeField]
		private bool isLongTapButton = false;           // 長押しボタンか？
		private const float LONG_PRESS_INTERVAL_SEC = 0.7f;
		public Action onLongPress;
		private float pressingSecCounter = 0f;          // 
		private bool isEnabledLongPress = true;         // 長押し可能か
		private bool isPressing = false;                // 長押し中か？
		#endregion

		// Use this for initialization
		void Start()
		{
			// GameMainObjectの取得
			GameObject go = GameObject.FindWithTag("MainGameObject");
			MainGameObject mainGameObject = go.GetComponent<MainGameObject>();
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

		// Update is called once per frame
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

		private void UpdateNormalButton()
		{
			if (!isEnablePushedScaling)
			{
				return;
			}

			if (isPushed)
			{
				scaleRation = (scaleRation * (1.0f - SCALING_CLOSE_RATE)) + (BUTTON_PRESSED_SCALE * SCALING_CLOSE_RATE);
			}
			else
			{
				scaleRation = (scaleRation * (1.0f - SCALING_CLOSE_RATE)) + (1.0f * SCALING_CLOSE_RATE);
			}

			if (button != null && button.IsInteractable())
			{
				button.transform.localScale = startButtonScale * scaleRation;
			}
		}

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

		// 同一ボタン連打対策
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

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!isLongTapButton)
			{
				OnPointerExit_NormalButton();
			}
		}

		private void OnPointerDown_NormalButton()
		{
			isPushed = true;
			if (customPointerDownEvent != null)
			{
				customPointerDownEvent();
			}
		}

		private void OnPointerDown_LongTapButton()
		{
			isPressing = true;
		}

		private void OnPointerUp_NormalButton()
		{
			isPushed = false;
		}

		private void OnPointerUp_LongTapButton()
		{
			pressingSecCounter = 0;
			isEnabledLongPress = true;
			isPressing = false;
		}

		private void OnPointerExit_NormalButton()
		{
			isPushed = false;
		}
	}
}
