using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using System.Reflection;
using System.Collections.Generic;
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

        static void Postfix(TICouncilorState __instance, CouncilorAugmentationOption augmentation)
        {
            Log.Info("Apply augmentation with cost " + augmentation.XPCost.ToString());
            CouncilorLevelManagerExternalMethods.IncrementCouncilorLevel(__instance, augmentation.XPCost);
        }
    }

    [HarmonyPatch(typeof(TICouncilorState), "InitWithTemplate")]
    static class InitWithTemplatePatch
    {

        static void Postfix(TIDataTemplate template, TICouncilorState __instance)
        {
            if (Main.enabled)
            {
                TICouncilorTemplate ticouncilortemplate = template as TICouncilorTemplate;
                bool flag = ticouncilortemplate == null;
                if (!flag)
                {
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

    // New Templates

    [HarmonyPatch(typeof(TemplateManager), "ValidateAllTemplates")]
    static class UseAlternateTemplateLoadManager
    {
        static void Prefix()
        {
            TIMissionTemplate template = TemplateManager.Find<TIMissionTemplate>("RespecCouncilor");
            if (template == null)
            {
                Log.Info("RespecCouncilor template not found!");
                return;
            }
            TIMissionEffect_RespecCouncilor tiMissionEffect = new TIMissionEffect_RespecCouncilor();
            List<TIMissionEffect> list = new List<TIMissionEffect>() { tiMissionEffect };
            template.councilorEffects = list;

            TemplateManager.Add<TIMissionTemplate>(template, true);

            return;
        }
    }
}
