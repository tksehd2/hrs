using Hrs.Core;

namespace Hrs.Main
{
	public class HrsMainGameObject : MainGameObject
	{
		// Use this for initialization
		protected override void Start()
		{
			Initialize(new HrsSetting());
		}
	}
}