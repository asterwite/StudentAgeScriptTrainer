using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZGScriptTrainer.Cheat;

namespace ZGScriptTrainer.Hook
{
    internal class Hook
    {
        public static void HarmonyHook()
        {
            Harmony.CreateAndPatchAll(typeof(MinGameCheat), null);
            Harmony.CreateAndPatchAll(typeof(GetFriendCheat), null);
            Harmony.CreateAndPatchAll(typeof(MapFriendCheat), null);
        }
    }
}
