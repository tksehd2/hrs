using System.Diagnostics;

namespace Hrs.Util
{
	/// <summary>
	/// 位置確認用クラス
	/// 生成された時のクラス名、コード位置等を覚える
	/// </summary>
	public class PosInfos
	{
		/// <summary> クラス名 </summary>
		public readonly string _className = string.Empty;
		/// <summary> ファイル名 </summary>
		public readonly string _fileName = string.Empty;
		/// <summary> ライン </summary>
		public readonly int _lineNumber = -1;
		/// <summary> メソッド名 </summary>
		public readonly string _methodName = string.Empty;

		/// <summary>
		/// エラーログを出す
		/// <param name="callerFrameIndex"> 呼ぶフレームIndex </param>
		/// </summary>
		public PosInfos(int callerFrameIndex = 1)
		{
#if HRS_DEBUG
			var sf = new StackFrame(callerFrameIndex, true);
			_className = sf.GetMethod().ReflectedType.FullName;
			_fileName = sf.GetFileName();
			_lineNumber = sf.GetFileLineNumber();
			_methodName = sf.GetMethod().Name;
#endif
		}

		/// <summary> 
		/// 文字列変換
		/// </summary>
		public override string ToString()
		{
#if HRS_DEBUG
			return string.Format("F:{0} L:{1} / {2}.{3}", _fileName, _lineNumber.ToString(), _className, _methodName);
#else
			return String.Empty;
#endif
		}
	}
}
