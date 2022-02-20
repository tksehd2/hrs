namespace Hrs.Gear
{
	public interface IGearHolder 
	{
		/// <summary> ギア初期化 </summary>
		void InitGear();
		/// <summary> ギア取得 </summary>
		Gear GetGear();
		/// <summary> 全ギア解放 </summary>
		void AllDisposeGear();
		/// <summary> ログ出力 </summary>
		string GearDILog();
	}
}
