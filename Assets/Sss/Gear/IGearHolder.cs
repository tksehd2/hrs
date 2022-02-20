namespace Hrs.Gear
{
	public interface IGearHolder 
	{
		void InitGear();
		Gear GetGear();
		void AllDisposeGear();
		string GearDILog();
	}
}
