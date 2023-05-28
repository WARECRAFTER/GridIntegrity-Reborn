using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Xml.Serialization;

using Sandbox;
using Sandbox.Common;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Weapons;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage;
using VRageMath;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using System.Drawing;

namespace WARE.Grid_Integirty_Reborn.GridIntegrity
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_CubeGrid))]
    public class GridIntegrity : MyGameLogicComponent
    {
        private const float StandardHealthFloat = 1f;
        private const float _HealtRation = 1f;
        private const float _DestructionRation = .4f;
        private bool _isExploding = false;
        private const float _RegenRatioPer100 = 0.00001f;

        public float Integrity { get; set; }
        public float MaxIntegrity { get; set; }
        public float MinIntegrity { get; set; }
        public bool validGrid { get; set; }

        private MyObjectBuilder_EntityBase objectBuilder;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            validGrid= false;
            ResetIntegrity();

            this.objectBuilder = objectBuilder;

            IMyCubeGrid grid = Entity as IMyCubeGrid;
            if (grid.GridSizeEnum == MyCubeSize.Small)
                validGrid = false;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME | MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;

            base.Init(objectBuilder);
        }


        public override void Close()
        {
            objectBuilder = null;

        }

        public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
        {
            return copy ? (MyObjectBuilder_EntityBase)objectBuilder.Clone() : objectBuilder;
        }

        public override void UpdateBeforeSimulation100()
        {
            GridIntegrityCore.Init();
            CalculateMinMaxIntegrity();
            if (validGrid)
            {
                //MyVisualScriptLogicProvider.SendChatMessage(Integrity + "(" + MinIntegrity + "->" + MaxIntegrity + ")" + Integrity/MaxIntegrity);
                Integrity += _RegenRatioPer100 * MaxIntegrity;
                if ((!_isExploding) && (Integrity <= MinIntegrity))
                {
                    StartReactorExplosion();
                }
                //RewriteTextPanels(GetValidTextPanels());
            }
            RewriteTextPanels(GetValidTextPanels());
            base.UpdateBeforeSimulation100();
        }

        public void TakeDamage(float damage)
        {
            if (!validGrid) return;
            Integrity -= damage;
            if(Integrity < MinIntegrity ) StartReactorExplosion();
            if(Integrity < MinIntegrity) Integrity = MinIntegrity;
            IMyCubeGrid grid = Entity as IMyCubeGrid;
        }

        public void StartReactorExplosion()
        {


            _isExploding = true;
            MyVisualScriptLogicProvider.SendChatMessage(Integrity + "(" + MinIntegrity + " --> " +MaxIntegrity + ")");
            IMyCubeGrid grid = Entity as IMyCubeGrid;
            MyVisualScriptLogicProvider.SendChatMessage("Ship " + grid.DisplayName + " blew up", "Ship Computer", 0,  "Red");
        }

        public void ResetIntegrity()
        {
            Integrity = 0f;
            MinIntegrity = 0f;
            MaxIntegrity = 0f;

            CalculateMinMaxIntegrity(false);
            Integrity = MaxIntegrity;
        }

        public void CalculateMinMaxIntegrity(bool recalculateIntegrity = true)
        {
            float MaxBefore = MaxIntegrity;
            IMyCubeGrid grid = Entity as IMyCubeGrid;
            List<IMySlimBlock> slimBlocks = new List<IMySlimBlock>();
            grid.GetBlocks(slimBlocks);
            ISet<IMySlimBlock> terminalSet = new HashSet<IMySlimBlock>();
            foreach(IMySlimBlock block in slimBlocks)
            {
                if(block.FatBlock is IMyTerminalBlock)
                {
                    terminalSet.Add(block);
                }
            }

            MinIntegrity = 0f;
            MaxIntegrity = 0f;

            if(terminalSet.Count < 10)
            {
                validGrid = false;
                return;
            }
            validGrid = true;
            foreach(IMySlimBlock slimBlock in terminalSet)
            {
                MaxIntegrity += slimBlock.MaxIntegrity * _HealtRation;
            }

            MinIntegrity = _DestructionRation * MaxIntegrity;

            if(recalculateIntegrity)
            {
                Integrity += MaxIntegrity - MaxBefore;
                if (Integrity > MaxIntegrity) Integrity = MaxIntegrity;
            }
            if (Math.Abs(MaxBefore) < double.Epsilon) Integrity = MaxIntegrity;
        }

        private ISet<IMyTextPanel> GetValidTextPanels()
        {

            IMyCubeGrid grid = Entity as IMyCubeGrid;
            List<IMySlimBlock> slimBlocks = new List<IMySlimBlock>();
            grid.GetBlocks(slimBlocks);
            HashSet<IMyTextPanel> textPanelSet = new HashSet<IMyTextPanel>();
            foreach (IMySlimBlock slimBlock in slimBlocks)
            {
                var panel = slimBlock.FatBlock as IMyTextPanel;
                if (panel != null)
                {
                    IMyTextPanel textPanel = panel;
                    if (textPanel.CustomName.Contains("IntegrityStatus")) textPanelSet.Add(textPanel);
                }
            }
            return textPanelSet;
        }

        private void RewriteTextPanels(ISet<IMyTextPanel> panels)
        {
            foreach (var panel in panels)
            {
                double currentStatus = (Integrity / MaxIntegrity) * 100;
                panel.CustomData = currentStatus.ToString();
                panel.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
                panel.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.CENTER;
                panel.WriteText("");
                if (validGrid)
                {
                    panel.WriteText("\n\n\n\n\n\n\nCurrent Grid Integrity : " + currentStatus.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "%\n", true);
                    panel.WriteText("GridIntegrityStatus: " + currentStatus.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "%", true);
                }
                else
                {
                    panel.WriteText("\n\n\n\n\n\n\n       Current Grid Integrity : Disabled!", true);

                    panel.WriteText("GridIntegrityStatus: Disabled", true);
                }
                if (_isExploding)
                {
                    panel.WriteText("");

                    panel.WriteText("\n\n\n\n\n\n\nWARNING! \n FATAL REACTOR EXPLOSION IMMINENT\nEVACUATE IMMEDIATELY!\n", true);
                }
            }
        }
    }
}
