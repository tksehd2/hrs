using System;
using UnityEngine;
using Hrs.Util;
using Hrs.Gear;

namespace Hrs.Example
{
    public class GearExample : MonoBehaviour
    {
        GearRoot gearRoot = null;
        // Start is called before the first frame update
        void OnEnable()
        {
            HrsLog.Init(HrsLogLevelDefine.All);
            gearRoot = new GearRoot();
            gearRoot.InitGear();
        }

        private void OnDisable()
        {
            gearRoot.AllDisposeGear();
            gearRoot = null;
        }
    }

    public class GearRoot : GearHolder
    {
        private ChildA childA = null;
        private ChildB childB = null;

        public GearRoot() : base(true)
        {
            childA = new ChildA();
            childB = new ChildB();

            _gear.AddChildGear(childA.GetGear());
            _gear.AddChildGear(childB.GetGear());
        }

        protected override void DiffuseGearProcess()
        {
            base.DiffuseGearProcess();
            _gear.Diffuse(this, typeof(GearRoot));
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! ôøÑ¢ûù
        protected override void StartGearProcess()
        {
            base.StartGearProcess();
            HrsLog.Debug("Root Gear Start");
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! ú°ð¶
        protected override void EndGearProcess()
        {
            base.EndGearProcess();
            HrsLog.Debug("Root Gear End");
        }
    }

    public class ChildA : GearHolder
    {
        public ChildA() : base(false)
        {
        }

        protected override void DiffuseGearProcess()
        {
            base.DiffuseGearProcess();
            _gear.Diffuse(this, typeof(ChildA));
        }

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

    public class ChildB : GearHolder
    {
        private ChildC childC = null;

        public ChildB() : base(false)
        {
            childC = new ChildC();
            _gear.AddChildGear(childC.GetGear());
        }

        protected override void DiffuseGearProcess()
        {
            base.DiffuseGearProcess();
            _gear.Diffuse(this, typeof(ChildB));
        }

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

    public class ChildC : GearHolder
    {
        public ChildC() : base(false)
        {

        }

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