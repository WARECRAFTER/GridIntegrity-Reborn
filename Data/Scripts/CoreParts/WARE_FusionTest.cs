using System.Collections.Generic;
using Sandbox.Game;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;
using CoreSystems.Api;
using VRage.Input;
using VRage.Game.Entity;
using System;
using Sandbox.Game.Entities;
using VRage.Utils;


namespace WARE.TorpedoWarheadTestHandler
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class prototypePhantom : MySessionComponentBase
    {
        WcApi wcAPi;
        List<MyEntity> allActivePhantoms = new List<MyEntity>();
        int masterTimer = 0;

        //public class MissileMine
        //{
        //    public MyEntity phantomEnt;
        //    public bool tripped;
        //    public float radius;
        //    public BoundingSphereD sphere;
        //    public List<MyEntity> nearbyEnts = new List<MyEntity>();
        //    public string owningFactionTag;
        //    public int endTime;
        //    public MissileMine(MyEntity phantomEnt, float radius, string owningFactionTag)
        //    {
        //        this.phantomEnt = phantomEnt;
        //        this.tripped = false;
        //        this.radius = radius;
        //        this.owningFactionTag = owningFactionTag;
        //        sphere = new BoundingSphereD(phantomEnt.WorldMatrix.Translation, this.radius);
        //    }

        //    public void Update()
        //    {
        //        sphere.Center = phantomEnt.WorldMatrix.Translation;
        //        sphere.Radius = radius;
        //        nearbyEnts.Clear();
        //        MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, nearbyEnts);

        //        foreach (var ent in nearbyEnts)
        //        {
        //            MyCubeGrid grid = ent as MyCubeGrid;
        //            if (grid != null && grid.Physics != null)
        //            {
        //                if (grid.BigOwners.Count > 0)
        //                {
        //                    string testTag = MyVisualScriptLogicProvider.GetPlayersFactionTag(grid.BigOwners[0]);
        //                    if (!string.IsNullOrEmpty(testTag) && testTag != owningFactionTag)
        //                    {
        //                        tripped = true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        
        //}

        //public class TimedPhantom
        //{
        //    public MyEntity phantomEnt;
        //    public int endTime;

        //    public TimedPhantom(MyEntity phantomEnt, int endTime)
        //    {
        //        this.phantomEnt = phantomEnt;
        //        this.endTime = endTime;
        //    }
        //}

        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {

        }

        public override void BeforeStart()
        {
            wcAPi = new WcApi();
            wcAPi.Load();
        }

        public override void UpdateAfterSimulation()
        {
            //var characMat = MatrixD.Identity;
            //if (MyAPIGateway.Session.Player?.Character != null)
            //{
            //    characMat = MyAPIGateway.Session.Player.Character.WorldMatrix;
            //}

            //var forwardMat = characMat;
            //forwardMat.Translation += forwardMat.Forward * 10;

            //if (MyAPIGateway.Input.IsNewKeyPressed(MyKeys.U) && ValidInput())
            //{
            //    if (wcAPi != null && wcAPi.IsReady)
            //    {
            //        List<MyDefinitionId> allPhantoms = new List<MyDefinitionId>();
            //        wcAPi.GetAllCorePhantoms(allPhantoms);
            //        foreach (var phantom in allPhantoms)
            //        {
            //            MyVisualScriptLogicProvider.SendChatMessage("Phantom found: " + phantom.SubtypeName);
            //        }

                 
            //    }
            //}

            //if (MyAPIGateway.Input.IsNewKeyPressed(MyKeys.L) && ValidInput() && wcAPi != null)
            //{
            //    var ent = wcAPi.SpawnPhantom("FusionExplosion", 100, false, 1, "WARE_FusionExplosion", WcApi.TriggerActions.TriggerOn,
            //     null, null, false, false, MyAPIGateway.Session.Player.IdentityId, true);
            //    ent.WorldMatrix = forwardMat;
            //    allActivePhantoms.Add(ent);

            //    MyVisualScriptLogicProvider.SendChatMessage("Spawned phantom");
            //}

            //foreach (var mine in allMines)
            //{
            //    if (!mine.tripped)
            //    {
            //        mine.Update();
            //        if (mine.tripped)
            //        {
            //            wcAPi.SetTriggerState(mine.phantomEnt, WcApi.TriggerActions.TriggerOn);
            //            mine.endTime = masterTimer + 30;
            //        }
            //    }
            //}

            //foreach (var mine in allMines)
            //{
            //    if (mine.tripped)
            //    {
            //        if (masterTimer > mine.endTime)
            //        {
            //            wcAPi.SetTriggerState(mine.phantomEnt, WcApi.TriggerActions.TriggerOff);
            //        }
            //    }
            //}

            //if (MyAPIGateway.Session.IsServer)
            //{
            //    for (int i = allActivePhantoms.Count - 1; i >= 0; i--)
            //    {
            //        if (masterTimer > allActivePhantoms[i].endTime)
            //        {
            //            if (allActivePhantoms[i].phantomEnt != null)
            //            {
            //                using (allActivePhantoms[i].phantomEnt.Pin())
            //                {
            //                    if (!allActivePhantoms[i].phantomEnt.MarkedForClose && !allActivePhantoms[i].phantomEnt.Closed)
            //                        wcAPi.ClosePhantom(allActivePhantoms[i].phantomEnt);
            //                }
            //            }

            //            allActivePhantoms.RemoveAt(i);
            //        }
            //    }
            //}

            //foreach (var phantom in allActivePhantoms)
            //{
            //    phantom.WorldMatrix = forwardMat;
            //}

            //if (MyAPIGateway.Input.IsKeyPress(MyKeys.R) && ValidInput())
            //{
            //    MyVisualScriptLogicProvider.SendChatMessage("Detonate phantom");
            //    foreach (var phantom in allActivePhantoms)
            //    {
            //        wcAPi.SetTriggerState(phantom, WcApi.TriggerActions.TriggerOnce);
                    
            //    }
            //    MyLog.Default.WriteLineAndConsole("Fire2 ");
            //}
            //else
            //{
            //    foreach (var phantom in allActivePhantoms)
            //    {
            //        wcAPi.SetTriggerState(phantom, WcApi.TriggerActions.TriggerOff);
            //    }
            //}
            //masterTimer += 1;
        }

        private bool ValidInput()
        {
            if (MyAPIGateway.Session.CameraController != null && !MyAPIGateway.Gui.ChatEntryVisible && !MyAPIGateway.Gui.IsCursorVisible
                && MyAPIGateway.Gui.GetCurrentScreen == MyTerminalPageEnum.None && !MyAPIGateway.Input.IsAnyAltKeyPressed())
            {
                return true;
            }
            return false;
        }


        protected override void UnloadData()
        {
            if (wcAPi != null)
            {
                wcAPi.Unload();
            }
        }
    }
}