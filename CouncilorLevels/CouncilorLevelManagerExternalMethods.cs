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
        public static int? GetCouncilorLevel(this TICouncilorState councilor)
        {
            return Manager[councilor];
        }

        /// <summary>
        /// Removes a councilor from the register
        /// </summary>
        /// <param name="councilor">The TICouncilorState instance</param>
        public static void RemoveCouncilorLevel(this TICouncilorState councilor)
        {
 
            Manager.DeRegisterList(councilor);
        }

        /// <summary>
        /// Increments or adds a councilor to the register
        /// </summary>
        /// <param name="councilor">The TICouncilorState instance</param>
        public static void AddOrIncrementCouncilorLevel(this TICouncilorState councilor)
        {
            Manager.RegisterList(councilor);
        }

        /// <summary>
        /// Decrements the councilor level
        /// </summary>
        /// <param name="councilor">The TICouncilorState instance</param>
        public static void DecrementCouncilorLevel(this TICouncilorState councilor)
        {
            int? currentLevel = Manager[councilor];
            if (currentLevel != null)
            {
                Manager.UpdateRegisterList(councilor, (int)currentLevel-1);
            }
        }
    }
}
