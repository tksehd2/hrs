using System.Diagnostics;
using System;

namespace Hrs.Util
{
	public class PosInfos
	{
		public readonly string _className;
		public readonly string _fileName;
		public readonly int _lineNumber;
		public readonly string _methodName;

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
