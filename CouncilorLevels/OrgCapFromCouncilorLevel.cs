using HarmonyLib;
using PavonisInteractive.TerraInvicta;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CouncilorLevels
{

    class OrgCapFromCouncilorLevel
    {
        /// <summary>
        /// 
        /// </summary>
        [HarmonyPatch(typeof(TICouncilorState), "SufficientCapacityForOrg")]
        static class SufficientCapacityForOrgPatch
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="org"></param>
            static bool Prefix(TIOrgState org, TICouncilorState __instance, ref bool __result)
            {
                if (Main.settings.IsEnabled("OrgCapacityFromLevel"))
                {
                    __result = __instance.orgs.Count < 15 && __instance.orgsWeight + org.tier <= CouncilorLevelManagerExternalMethods.GetCouncilorLevel(__instance);
                    return false;
                }
                // Log.Info("Reached a bad spot");
                // Disabled mod passthrough
                return true;
            }
        }

        [HarmonyPatch(typeof(TICouncilorState), "CanRemoveOrg_Admin")]
        static class CanRemoveOrg_AdminPatch
        {
            static bool Prefix(TIOrgState org, ref TICouncilorState __instance, ref bool __result)
            {
                if (Main.settings.IsEnabled("OrgCapacityFromLevel"))
                {
                    // Because org capacity is driven by level we no longer need this check. You can always remove orgs.
                    __result = true;
                    return true;
                }
                // Disabled mod passthrough
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [HarmonyPatch(typeof(TIFactionState), "ValidateAllOrgs")]
        static class ValidateAllOrgsPatch
        {
            static bool Prefix(bool suppressReporting, TIFactionState __instance, ref List<TIOrgState> __result)
            {
                if (Main.settings.IsEnabled("OrgCapacityFromLevel"))
                {
                    List<TIOrgState> badOrgs = new List<TIOrgState>();
                    new List<int>();
                    Func<TICouncilorState, TIOrgState> func = (TICouncilorState councilor) => (from x in councilor.orgs.Except(badOrgs)
                                                                                                   // orderby x.administration
                                                                                               orderby x.tier
                                                                                               select x).First<TIOrgState>();
                    foreach (TICouncilorState ticouncilorState in __instance.councilors)
                    {
                        int? availableAdministration = CouncilorLevelManagerExternalMethods.GetCouncilorLevel(ticouncilorState) - ticouncilorState.orgsWeight;
                        Action<TIOrgState> action = delegate (TIOrgState badOrg)
                        {
                            badOrgs.Add(badOrg);
                            // availableAdministration += badOrg.tier;
                        };
                        using (List<TIOrgState>.Enumerator enumerator2 = ticouncilorState.orgs.GetEnumerator())
                        {
                            while (enumerator2.MoveNext())
                            {
                                TIOrgState tiorgState = enumerator2.Current;
                                if (!tiorgState.IsEligibleForCouncilor(ticouncilorState))
                                {
                                    action(tiorgState);
                                }
                            }
                            goto IL_BE;
                        }
                        goto IL_AC;
                    IL_BE:
                        if (availableAdministration >= 0 || !ticouncilorState.orgs.Except(badOrgs).Any<TIOrgState>())
                        {
                            continue;
                        }
                    IL_AC:
                        TIOrgState tiorgState2 = func(ticouncilorState);
                        action(tiorgState2);
                        goto IL_BE;
                    }
                    foreach (TIOrgState tiorgState3 in badOrgs)
                    {
                        __instance.AddOrgToFactionPool(tiorgState3, tiorgState3.assignedCouncilor);
                    }
                    if (!suppressReporting && badOrgs.Count > 0)
                    {
                        TINotificationQueueState.LogOrgsForcedToPool(__instance, badOrgs);
                    }
                    __result = badOrgs;
                    return false;
                }
                // Disabled mod passthrough
                return true;
            }
        }

        // UI

        /// <summary>
        /// 
        /// </summary>
        [HarmonyPatch(typeof(CouncilGridController), "SetCouncilorInfo")]
        class SetCouncilorInfoPatch
        {
            static void Postfix(CouncilGridController __instance)
            {
                if (Main.settings.IsEnabled("OrgCapacityFromLevel"))
                {
                    __instance.councilorOrgGridTitle.SetText(Loc.T("UI.Councilor.OrgGridTitle", new object[]
                    {
                    __instance.currentCouncilor.orgsWeight.ToString(),
                    CouncilorLevelManagerExternalMethods.GetCouncilorLevel(__instance.currentCouncilor),
                    __instance.currentCouncilor.orgs.Count.ToString(), 15.ToString()
                    }), true);
                }
            }
        }

        // AI 

        /// <summary>
        /// 
        /// </summary>
        [HarmonyPatch(typeof(AIEvaluators), "EvaluateStatIncreaseUtility")]
        class AdjustAIAdminPreference
        {

            static void Postfix(TICouncilorState councilor, TIFactionState faction, CouncilorAttribute attribute, float __result, List<TIMissionTemplate> requiredMissions)
            {
                if (Main.settings.IsEnabled("OrgCapacityFromLevel") & attribute == CouncilorAttribute.Administration)
                {
                    // The admin value is weighted by 20!! We reduce that to 2f
                    __result /= 10f;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [HarmonyPatch(typeof(AIEvaluators), "EvaluateOrgForCouncilor")]
        class EvaluateOrgForCouncilorPatch
        {

            static bool Prefix(TIOrgState org, TICouncilorState councilor, List<TIMissionTemplate> requiredMissions, List<TIMissionTemplate> missingRequiredMissions, float __result)
            {
                if (Main.settings.IsEnabled("OrgCapacityFromLevel"))
                {
                    if (org.tier - org.administration > CouncilorLevelManagerExternalMethods.GetCouncilorLevel(councilor))
                    {
                        __result = 0f;
                        return false;
                    }
                }
                // Disabled mod passthrough
                return true;
            }

        }
    }
}
