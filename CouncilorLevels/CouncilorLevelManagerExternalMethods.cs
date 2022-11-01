using UnityEngine;
using PavonisInteractive.TerraInvicta;

namespace CouncilorLevels
{
    public static class CouncilorLevelManagerExternalMethods
    {
        /// <summary>
        /// ExternalMethods manages the content with an instance of CouncilorLevelManager
        /// </summary>
        public static CouncilorLevelManager Manager = new CouncilorLevelManager();

        /// <summary>
        /// Returns the level of a councilor by its instance
        /// </summary>
        /// <param name="councilor">The TICouncilorState instance</param>
        /// <returns></returns>
        public static int? GetCouncilorLevel(TICouncilorState councilor)
        {
            return Manager[councilor].CurrentLevel;
        }

        public static int? GetCouncilorTotalXP(TICouncilorState councilor)
        {
            return Manager[councilor].TotalXP;
        }

        public static void AddXPToCouncilorTotalXP(TICouncilorState councilor, int value)
        {
            // Manager[councilor].TotalXP = Mathf.Max(Manager[councilor].TotalXP, councilor.XP);
            Manager[councilor].TotalXP += value;
        }

        /// <summary>
        /// Removes a councilor from the register
        /// </summary>
        /// <param name="councilor">The TICouncilorState instance</param>
        public static void RemoveCouncilorLevel(TICouncilorState councilor)
        {
            Manager.DeRegisterList(councilor);
        }

        /// <summary>
        /// Increments or adds a councilor to the register
        /// </summary>
        /// <param name="councilor">The TICouncilorState instance</param>
        public static void AddCouncilorLevel(TICouncilorState councilor, TICouncilorLevelState councilorLevel = null)
        {
            Manager.RegisterList(councilor, councilorLevel);
        }

        /// <summary>
        /// Increments the councilor level
        /// </summary>
        /// <param name="councilor">The TICouncilorState instance</param>
        public static void IncrementCouncilorLevel(TICouncilorState councilor)
        {
            // Log.Info("Adding to Councilor level with XP " + levelCost.ToString());
            Manager.IncrementCouncilorLevel(councilor);
        }

        /// <summary>
        /// Decrements the councilor level
        /// </summary>
        /// <param name="councilor">The TICouncilorState instance</param>
        public static void DecrementCouncilorLevel(TICouncilorState councilor)
        {
            Manager.DecrementCouncilorLevel(councilor);
        }

        public static void Respec(TICouncilorState councilor)
        {
            Manager.Respec(councilor);
        }
    }
}
