using Hrs.Util;

namespace Hrs.Gear
{
	/// <summary>
	/// diffuse, absorbを使うクラスの基底クラス
	/// このクラスを継承しておくことで必要な処理は基本的に自動で行われる
	/// GearHolderBehavior がほぼコピペなので、修正は両方に入れるように注意
	/// </summary>
	public class GearHolder : IGearHolder 
	{
		protected readonly bool _isRoot;
		protected Gear _gear = null;

		/// <summary>
		/// コンストラクタ
		/// Gearを必ず保持する
		/// 大親の時はisrootをtrueにすること
		/// </summary>
		public GearHolder(bool isRoot) 
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

		/// Gearの外部出し用（子が親のGearを見るために必要）
		public Gear GetGear()
		{
			return _gear;
		}

		/// インスタンスが一通り揃い、Gearの親子関係ができた後の最初の処理
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
