using System;
using UnityEngine;
using Hrs.Util;
using Hrs.Gear;

namespace Hrs.Example
{
    /// <summary>
    /// �������
    /// </summary>
    public class GearExample : MonoBehaviour
    {
        /// <summary> ��?�ȫ��� </summary>
        GearRoot gearRoot = null;

        /// <summary>
        /// ���
        /// </summary>
        void OnEnable()
        {
            HrsLog.Init(HrsLogLevelDefine.All);
            gearRoot = new GearRoot();
            gearRoot.InitGear();
        }

        /// <summary>
        /// ����
        /// </summary>
        private void OnDisable()
        {
            gearRoot.AllDisposeGear();
            gearRoot = null;
        }
    }

    /// <summary>
    /// ��?�ȫ���
    /// </summary>
    public class GearRoot : GearHolder
    {
        /// <summary> ���A </summary>
        private ChildA childA = null;
        /// <summary> ���B </summary>
        private ChildB childB = null;

        /// <summary>
        /// ���󫹫ȫ髯����
        /// </summary>
        public GearRoot() : base(true)
        {
            childA = new ChildA();
            childB = new ChildB();

            _gear.AddChildGear(childA.GetGear());
            _gear.AddChildGear(childB.GetGear());
        }

        /// <summary>
        /// ?ߤ
        /// </summary>
        protected override void DiffuseGearProcess()
        {
            base.DiffuseGearProcess();
            _gear.Diffuse(this, typeof(GearRoot));
        }

        /// <summary>
        /// ��Ѣ��
        /// </summary>
        protected override void StartGearProcess()
        {
            base.StartGearProcess();
            HrsLog.Debug("Root Gear Start");
        }

        /// <summary>
        /// ���
        /// </summary>
        protected override void EndGearProcess()
        {
            base.EndGearProcess();
            HrsLog.Debug("Root Gear End");
        }
    }

    /// <summary>
    /// ���A
    /// </summary>
    public class ChildA : GearHolder
    {
        /// <summary>
        /// ���󫹫ȫ髯����
        /// </summary>
        public ChildA() : base(false)
        {
        }

        /// <summary>
        /// ?ߤ
        /// </summary>
        protected override void DiffuseGearProcess()
        {
            base.DiffuseGearProcess();
            _gear.Diffuse(this, typeof(ChildA));
        }

        /// <summary>
        /// ��Ѣ��
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

            // ��������
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
    /// ���B
    /// </summary>
    public class ChildB : GearHolder
    {
        /// <summary> ���C </summary>
        private ChildC childC = null;

        /// <summary>
        /// ���󫹫ȫ髯����
        /// </summary>
        public ChildB() : base(false)
        {
            childC = new ChildC();
            _gear.AddChildGear(childC.GetGear());
        }

        /// <summary>
        /// ?ߤ
        /// </summary>
        protected override void DiffuseGearProcess()
        {
            base.DiffuseGearProcess();
            _gear.Diffuse(this, typeof(ChildB));
        }

        /// <summary>
        /// ��Ѣ��
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

            // ��������
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
    /// ���C
    /// </summary>
    public class ChildC : GearHolder
    {
        /// <summary>
        /// ���󫹫ȫ髯����
        /// </summary>
        public ChildC() : base(false)
        {

        }

        /// <summary>
        /// ��Ѣ��
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