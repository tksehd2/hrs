using UnityEngine;
using Hrs.Gear;

namespace Hrs.Core
{
	/// <summary> 
	/// フレームマネージャー
	/// 前回のゲームループとの差分を計算する
	/// </summary>
	public class FrameManager : GearHolder
	{
		/// <summary> 時間倍率 </summary>
		private float _speedRate = 1.0f;
		/// <summary> Frame per sencod (1秒に回るFrame数) </summary>
		public readonly int FPS = 60;
		/// <summary> Sencond per frame (１Frameかかる時間（秒）) </summary>
		public readonly float SPF = 1.0f;
		/// <summary> 前回のupdate()の最後のタイミングの、開始時刻からの秒数 </summary>
		private float _lastUpdateSeconds = 0;
		/// <summary> 経過時間を蓄積する変数 </summary>
		private float _elapsedSeconds = 0.0f;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="fps"> FPS </param>
		public FrameManager(int fps) : base(false)
		{
			FPS = fps;
			SPF = 1.0f / FPS;
		}

		/// <summary>
		/// 最後に更新した時間（秒）を記録する
		/// </summary>
		public void RecordLastUpdateSeconds()
		{
			_lastUpdateSeconds = Time.time;
		}

		/// <summary>
		/// Frameを計算する
		/// </summary>
		/// <returns>`フレーム差分 </returns>
		public int CalcDeltaFrame()
		{
			int deltaFrame = 0;
			// 経過時間を蓄積しておく
			_elapsedSeconds += ((Time.time - _lastUpdateSeconds) * _speedRate);
			// Frameを数える
			while (_elapsedSeconds >= SPF)
			{
				_elapsedSeconds -= SPF;
				++deltaFrame;
			}

			return deltaFrame;
		}
	}

}