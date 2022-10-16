using HarmonyLib;
using UnityModManagerNet;
using System.Reflection;
using PavonisInteractive.TerraInvicta;

namespace CouncilorLevels
{
    static class Main
    {
        public static bool enabled;
        public static UnityModManager.ModEntry mod;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            mod = modEntry;
            modEntry.OnToggle = OnToggle;
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
    }

    [HarmonyPatch(typeof(TICouncilorState), "ApplyAugmentation")]
    static class ApplyAugmentationPatch
    {

        static void Postfix(TICouncilorState __instance)
        {
            CouncilorLevelManagerExternalMethods.IncrementCouncilorLevel(__instance);
        }
    }

    [HarmonyPatch(typeof(TICouncilorState), "InitWithTemplate")]
    static class InitWithTemplatePatch
    {

        static void Postfix(TIDataTemplate template, TICouncilorState __instance)
        {
            if (Main.enabled)
            {
                Log.Info("InitWithTemplatePatch 1");
                TICouncilorTemplate ticouncilortemplate = template as TICouncilorTemplate;
                Log.Info("InitWithTemplatePatch 2");
                bool flag = ticouncilortemplate == null;
                if (!flag)
                {
                    Log.Info("InitWithTemplatePatch 3");
                    CouncilorLevelManagerExternalMethods.AddCouncilorLevel(__instance);
                }
                
            }
        }
    }

    // Checks & Logic

    [HarmonyPatch(typeof(TICouncilorState), "KillCouncilor")]
    static class KillCouncilorPatch
    {
        static void Postfix(ref TICouncilorState __instance)
        {
            CouncilorLevelManagerExternalMethods.RemoveCouncilorLevel(__instance);
        }
    }
}
