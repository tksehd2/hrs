using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hrs.Util;

namespace Hrs.Test
{
    /// <summary>
    /// «í«°ÖÇð¹
    /// </summary>
    public class LogExample : MonoBehaviour
    {
        /// <summary> DebugOn </summary>
        [SerializeField]
        private bool isDisplayDebug = true;

        /// <summary> InfoOn </summary>
        [SerializeField]
        private bool isDisplayInfo = true;

        /// <summary> WarinigOn </summary>
        [SerializeField]
        private bool isDisplayWarning = true;

        /// <summary> ErrorOn </summary>
        [SerializeField]
        private bool isDisplayError = true;

        /// <summary>
        /// ËÒã·
        /// </summary>
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