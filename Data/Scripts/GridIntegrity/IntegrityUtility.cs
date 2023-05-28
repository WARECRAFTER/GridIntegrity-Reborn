using Sandbox.Game;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;

namespace Grid_Integirty___Reborn.GridIntegrity
{
    public static class IntegrityUtility
    {
        public static H GetComponent<H>(VRage.ModAPI.IMyEntity entity) where H : MyGameLogicComponent
        {
            try
            {

                if (entity == null)
                {
                    return null;
                }

                MyEntityComponentContainer componentContainer = entity.Components;
                if (componentContainer == null)
                {
                    return null;
                }

                H hook = null;
                foreach (MyComponentBase comp in componentContainer)
                {
                    if (comp is H)
                    {
                        hook = comp as H;

                    }
                    if (comp is MyCompositeGameLogicComponent)
                    {
                        hook = comp.GetAs<H>();
                    }
                }
                if (hook == null)
                {
                    return null;
                }

                return hook;
            }
            catch (Exception e)
            {

                return null;
            }
        }
    }
}