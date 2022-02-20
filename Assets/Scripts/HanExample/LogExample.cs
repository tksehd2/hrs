using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hrs.Util;

namespace Hrs.Test
{
    public class LogExample : MonoBehaviour
    {
        [SerializeField]
        private bool isDisplayDebug = true;

        [SerializeField]
        private bool isDisplayInfo = true;

        [SerializeField]
        private bool isDisplayWarning = true;

        [SerializeField]
        private bool isDisplayError = true;

        // Start is called before the first frame update
        void OnEnable()
        {
            uint displayLevel = 0;
            if(isDisplayDebug)
            {
                displayLevel = displayLevel | HrsLogLevelDefine.Debug;
            }
            if (isDisplayInfo)
            {
                displayLevel = displayLevel | HrsLogLevelDefine.Info;
            }
            if (isDisplayWarning)
            {
                displayLevel = displayLevel | HrsLogLevelDefine.Warning;
            }
            if (isDisplayError)
            {
                displayLevel = displayLevel | HrsLogLevelDefine.Error;
            }

            HrsLog.Init(displayLevel);

            HrsLog.Debug("Debug");
            HrsLog.Info("Info");
            HrsLog.Warning("Warning");
            HrsLog.Error("Error");
        }
    }
}