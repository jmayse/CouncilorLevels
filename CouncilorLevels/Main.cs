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
            CouncilorLevelManagerExternalMethods.AddOrIncrementCouncilorLevel(__instance);
        }
    }

    [HarmonyPatch(typeof(TICouncilorState), "InitWithTemplate")]
    static class InitWithTemplatePatch
    {

        static void Postfix(TIDataTemplate template, ref bool ___gameStateSubjectCreated, TICouncilorState __instance)
        {
            if (Main.enabled)
            {
                if (!___gameStateSubjectCreated)
                {
                    TICouncilorTemplate ticouncilortemplate = template as TICouncilorTemplate;
                    bool flag = ticouncilortemplate == null;
                    if (!flag)
                    {
                        CouncilorLevelManagerExternalMethods.AddOrIncrementCouncilorLevel(__instance);
                    }
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
