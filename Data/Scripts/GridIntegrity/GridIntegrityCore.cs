using Grid_Integirty___Reborn.GridIntegrity;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI;

namespace WARE.Grid_Integirty_Reborn.GridIntegrity
{
    internal class GridIntegrityCore
    {
        private static bool _IsInit = false;

        public static void Init()
        {
            try
            {
                if (!_IsInit)
                {
                    MyAPIGateway.Session.DamageSystem.RegisterBeforeDamageHandler(5, IntegrityDamageHandler);
                    _IsInit = true;
                }
            }
            catch{}
        }

        public static void IntegrityDamageHandler(object target, ref MyDamageInformation info)
        {
            if (!IsValidDamage(ref info)) return;
            try {
                IMySlimBlock slimBlock = target as IMySlimBlock;
                if (slimBlock == null) return; //Skip if Null

                IMyCubeBlock cubeBlock = slimBlock.FatBlock;
                if(cubeBlock == null) return; //Skip if Null
                if (!(cubeBlock is IMyTerminalBlock)) return; //Skip if not Terminal Block
                GridIntegrity integrityCore = IntegrityUtility.GetComponent<GridIntegrity>(cubeBlock.CubeGrid);
                if(integrityCore == null) return;
                if (!integrityCore.validGrid) return;

                float damageMult = 1f;
                float overrideDamageMult = 1f;

                float damage = info.Amount;

                damage *= damageMult * overrideDamageMult;
                integrityCore.TakeDamage(damage);
            }
            catch { }
        }

        public static bool IsValidDamage(ref MyDamageInformation info)
        {
            return !(info.Type.ToString() == "Grind");
        }
    }
}
