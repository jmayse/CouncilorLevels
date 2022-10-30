using HarmonyLib;
using PavonisInteractive.TerraInvicta;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace CouncilorLevels
{
    static class Main
    {
        public static bool enabled;
        public static UnityModManager.ModEntry mod;
        public static Settings settings;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            settings = Settings.Load<Settings>(modEntry);
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            mod = modEntry;
            modEntry.OnToggle = OnToggle;
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
    }

    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        

        [Header("Enabled Fuctionality"), Space(5)]
        [Draw("Org Capacity from Councilor Level", Collapsible = true)] public static bool orgCapacityEnabled = true;
        [Draw("Councilor Respecs", Collapsible = true)] public static bool councilorRespecs = true;

        public Dictionary<string, bool> ModsEnabled = new Dictionary<string, bool>()
        {
            {"OrgCapacityFromLevel", orgCapacityEnabled },
            {"CouncilorRespecs", councilorRespecs },
        };

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            base.Save(modEntry);
        }

        public void OnChange()
        {
            ModsEnabled["OrgCapacityFromLevel"] = orgCapacityEnabled;
            ModsEnabled["CouncilorRespecs"] = councilorRespecs;
        }

        public bool IsEnabled(string ModName)
        {
            if (ModsEnabled.TryGetValue(ModName, out bool value))
            {
                return value;
            }
            return false;
        }
    }

    // Core

    [HarmonyPatch(typeof(TICouncilorState), "ChangeXP")]
    static class ChangeXPPatch
    {
        static void Postfix(TICouncilorState __instance, int value)
        {
            if (value > 0)
            {
                CouncilorLevelManagerExternalMethods.AddXPToCouncilorTotalXP(__instance, value);
            }
        }
    }

    [HarmonyPatch(typeof(TICouncilorState), "ApplyAugmentation")]
    static class ApplyAugmentationPatch
    {

        static void Postfix(TICouncilorState __instance, CouncilorAugmentationOption augmentation)
        {
            Log.Info("Apply augmentation with cost " + augmentation.XPCost.ToString());
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
                TICouncilorTemplate ticouncilortemplate = template as TICouncilorTemplate;
                bool flag = ticouncilortemplate == null;
                if (!flag)
                {
                    CouncilorLevelManagerExternalMethods.AddCouncilorLevel(__instance);
                }

            }
        }
    }

    // UI

    [HarmonyPatch(typeof(CouncilGridController), "SetCouncilorInfo")]
    class SetCouncilorInfoPatch
    {
        static void Postfix(CouncilGridController __instance)
        {
            __instance.XP.SetText(Loc.T("UI.Councilor.XP", new object[]
            {
                    CouncilorLevelManagerExternalMethods.GetCouncilorLevel(__instance.currentCouncilor).ToString(),
                    __instance.currentCouncilor.XP.ToString(),
                    CouncilorLevelManagerExternalMethods.GetCouncilorTotalXP(__instance.currentCouncilor).ToString(),
                    
            }), true);

        }
    }

    [HarmonyPatch(typeof(CouncilGridController), "SetAugmentationPanel")]
    class SetAugmentationPanelPatch
    {
        static void Postfix(CouncilGridController __instance)
        {
            __instance.augmentScreenCurrentXPValue.SetText(Loc.T("UI.CouncilorAugmentation.XP", new object[] { __instance.currentCouncilor.XP.ToString() }), true);
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
        static bool Prefix()
        {
            if (Main.settings.IsEnabled("CouncilorRespecs"))
            {
                TIMissionTemplate template = TemplateManager.Find<TIMissionTemplate>("RespecCouncilor");
                if (template == null)
                {
                    // Log.Info("RespecCouncilor template not found!");
                    return true;
                }
                TIMissionEffect_RespecCouncilor tiMissionEffect = new TIMissionEffect_RespecCouncilor();
                List<TIMissionEffect> list = new List<TIMissionEffect>() { tiMissionEffect };
                template.councilorEffects = list;

                TemplateManager.Add<TIMissionTemplate>(template, true);

                return true;
            }
            // Passthrough if not enabled
            return true;
        }
    }
}
