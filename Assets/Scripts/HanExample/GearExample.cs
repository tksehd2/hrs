using System;
using UnityEngine;
using Hrs.Util;
using Hrs.Gear;

namespace Hrs.Example
{
    /// <summary>
    /// «®«¢ÖÇð¹
    /// </summary>
    public class GearExample : MonoBehaviour
    {
        /// <summary> «ë?«È«®«¢ </summary>
        GearRoot gearRoot = null;

        /// <summary>
        /// ËÒã·
        /// </summary>
        void OnEnable()
        {
            HrsLog.Init(HrsLogLevelDefine.All);
            gearRoot = new GearRoot();
            gearRoot.InitGear();
        }

        /// <summary>
        /// áóÖõ
        /// </summary>
        private void OnDisable()
        {
            gearRoot.AllDisposeGear();
            gearRoot = null;
        }
    }

    /// <summary>
    /// «ë?«È«®«¢
    /// </summary>
    public class GearRoot : GearHolder
    {
        /// <summary> í­ÍêA </summary>
        private ChildA childA = null;
        /// <summary> í­ÍêB </summary>
        private ChildB childB = null;

        /// <summary>
        /// «³«ó«¹«È«é«¯«¿¡ª
        /// </summary>
        public GearRoot() : base(true)
        {
            childA = new ChildA();
            childB = new ChildB();

            _gear.AddChildGear(childA.GetGear());
            _gear.AddChildGear(childB.GetGear());
        }

        /// <summary>
        /// ?ß¤
        /// </summary>
        protected override void DiffuseGearProcess()
        {
            base.DiffuseGearProcess();
            _gear.Diffuse(this, typeof(GearRoot));
        }

        /// <summary>
        /// ôøÑ¢ûù
        /// </summary>
        protected override void StartGearProcess()
        {
            base.StartGearProcess();
            HrsLog.Debug("Root Gear Start");
        }

        /// <summary>
        /// ú°ð¶
        /// </summary>
        protected override void EndGearProcess()
        {
            base.EndGearProcess();
            HrsLog.Debug("Root Gear End");
        }
    }

    /// <summary>
    /// í­ÍêA
    /// </summary>
    public class ChildA : GearHolder
    {
        /// <summary>
        /// «³«ó«¹«È«é«¯«¿¡ª
        /// </summary>
        public ChildA() : base(false)
        {
        }

        /// <summary>
        /// ?ß¤
        /// </summary>
        protected override void DiffuseGearProcess()
        {
            base.DiffuseGearProcess();
            _gear.Diffuse(this, typeof(ChildA));
        }

        /// <summary>
        /// ôøÑ¢ûù
        /// </summary>
        protected override void StartGearProcess()
        {
            base.StartGearProcess();

            HrsLog.Debug("!!! Child A !!!");
            var root = _gear.Absorb<GearRoot>(new PosInfos());
            if(root != null)
            {
                HrsLog.Debug("root get !!");
            }

            // ã÷ø¨ª¹ªë
            try
            {
                var childB = _gear.Absorb<ChildB>(new PosInfos());
            }
            catch(Exception e)
            {
                HrsLog.Error(e.Message);
            }
        }
    }

    /// <summary>
    /// í­ÍêB
    /// </summary>
    public class ChildB : GearHolder
    {
        /// <summary> í­ÍêC </summary>
        private ChildC childC = null;

        /// <summary>
        /// «³«ó«¹«È«é«¯«¿¡ª
        /// </summary>
        public ChildB() : base(false)
        {
            childC = new ChildC();
            _gear.AddChildGear(childC.GetGear());
        }

        /// <summary>
        /// ?ß¤
        /// </summary>
        protected override void DiffuseGearProcess()
        {
            base.DiffuseGearProcess();
            _gear.Diffuse(this, typeof(ChildB));
        }

        /// <summary>
        /// ôøÑ¢ûù
        /// </summary>
        protected override void StartGearProcess()
        {
            base.StartGearProcess();

            HrsLog.Debug("!!! Child B !!!");
            var root = _gear.Absorb<GearRoot>(new PosInfos());
            if (root != null)
            {
                HrsLog.Debug("root get !!");
            }

            // ã÷ø¨ª¹ªë
            try
            {
                var childA = _gear.Absorb<ChildA>(new PosInfos());
            }
            catch (Exception e)
            {
                HrsLog.Error(e.Message);
            }
        }
    }

    /// <summary>
    /// í­ÍêC
    /// </summary>
    public class ChildC : GearHolder
    {
        /// <summary>
        /// «³«ó«¹«È«é«¯«¿¡ª
        /// </summary>
        public ChildC() : base(false)
        {

        }

        /// <summary>
        /// ôøÑ¢ûù
        /// </summary>
        protected override void StartGearProcess()
        {
            base.StartGearProcess();

            HrsLog.Debug("!!! Child C !!!");
            var root = _gear.Absorb<GearRoot>(new PosInfos());
            if (root != null)
            {
                HrsLog.Debug("root get !!");
            }

            var childB = _gear.Absorb<ChildB>(new PosInfos());
            if (childB != null)
            {
                HrsLog.Debug("Child B get !!");
            }
        }
    }
}