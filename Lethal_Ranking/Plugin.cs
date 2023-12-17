using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Lethal_Ranking.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lethal_Ranking
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Lethal_Ranking_Base : BaseUnityPlugin
    {
        private const string modGUID = "claymor_wan.Lethal_Ranking";
        private const string modName = "Lethal Ranking";
        private const string modVersion = "0.6.0";
        private const string modAuthor = "claymor_wan";
        private readonly Harmony harmony = new Harmony(modGUID);
        private static Lethal_Ranking_Base Instance;
        internal ManualLogSource mls;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("The femboy's mod as awaken");
            harmony.PatchAll(typeof(Lethal_Ranking_Base));
            harmony.PatchAll(typeof(QuickMenuManagerPatch));
        }
    }
}
