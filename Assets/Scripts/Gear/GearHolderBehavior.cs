using UnityEngine;
using Hrs.Util;

namespace Hrs.Gear
{
	/// <summary>
	/// diffuse, absorbを使うクラスの基底クラスのMonoBehavior対応版
	/// コンストラクタは使えないし、StartとAwakeは実行タイミングが怪しいので
	/// 初期化は共通化していない。
	/// 
	/// なので以下の感じで手動で叩いて下さい。
	///   GearHolderBehaviorTest ghb = uiPanelManager.GetComponent<GearHolderBehaviorTest>("GHB");
	///	  ghb.InitDI(gear);
	///	Start() 内でInitDI するなら、参照時に Initialized を確認したほうが良いかもです。
	///	
	///	ちなみに ghb.InitDI(); とすれば、大親としてGearのツリーを作れます。
	/// </summary>
	public class GearHolderBehavior : MonoBehaviour, IGearHolder
	{
		/// <summary> ルートかどうか </summary>
		protected bool _isRoot = false;
		/// <summary> ギア </summary>
		protected Gear _gear = null;

		/// <summary>
		/// コンストラクタは使えないので、使うときは手動でこの関数をた叩いて下さい。
		/// <param name="isRoot"> ルートかどうか </param>
		/// </summary>
		public void InitDI(bool isRoot)
		{
			_gear = new Gear(this);

			_isRoot = isRoot;

			//（Gearのセットアップ後）初期化時に行いたいこと（Action）を追加
			// Diffuseをする
			_gear.AddPreparationProcess(DiffuseGearProcess, new PosInfos());

			// prepare後に行いたいこと（Action）を追加
			// 開始処理
			// absorb可能
			_gear.AddStartProcess(StartGearProcess, new PosInfos());

			// dispose時に行いたいこと（Action）を追加
			// 修了処理
			_gear.AddEndProcess(EndGearProcess, new PosInfos());
		}

		/// <summary>
		/// Gearの外部出し用（子が親のGearを見るために必要）
		/// </summary>
		public Gear GetGear()
		{
			return _gear;
		}

		/// <summary>
		/// インスタンスが一通り揃い、Gearの親子関係ができた後の最初の処理
		/// </summary>
		public virtual void InitGear()
		{
			_gear.Initialize();
		}

		/// <summary>
		/// GearおよびDiffuseはIDisposableなので、明示的に破棄が必要
		/// このクラスのインスタンスを破棄する場合は必ず呼ぶこと
		/// 親→子に向けて一斉にDisposeされるので注意（大親だけ呼べばいいということ）
		/// </summary>
		public void AllDisposeGear()
		{
			_gear.Dispose();
		}

		/// <summary>
		/// コンストラクタ直後に行いたいこと（Action）を追加
		/// </summary>
		protected virtual void DiffuseGearProcess()
		{
			//UnityEngine.HrsLog.Debug("ProcessBase(" + this + ")::prepare");
		}

		/// <summary>
		/// 開始処理
		/// absorb可能
		/// </summary>
		protected virtual void StartGearProcess()
		{
		}

		/// <summary>
		/// 修了処理
		/// </summary>
		protected virtual void EndGearProcess()
		{
			//UnityEngine.HrsLog.Debug("ProcessBase(" + this + ")::disposeProcess");
		}

		/// <summary>
		/// DIコンテナのデバッグ用
		/// </summary>
		public string GearDILog()
		{
			return _gear.DILog();
		}

	}
}
