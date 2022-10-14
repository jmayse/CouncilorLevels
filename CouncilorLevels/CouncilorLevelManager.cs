using System.Collections.Generic;
using PavonisInteractive.TerraInvicta;

namespace CouncilorLevels
{
    public class CouncilorLevelManager
    {
        /// <summary>
        /// A Dictionary containing all levels keyed to TICouncilorState
        /// </summary>
        private Dictionary<GameStateID, int> CouncilorIDCouncilorLevelList = new Dictionary<GameStateID, int>();

        /// <summary>
        /// Registers a new member in the CouncilorIDCouncilorLevelList keyed by their ID with default level 1 or else increments existing councilor
        /// </summary>
        /// <param name="councilor">The instance to register</param>
        public void RegisterList(TICouncilorState councilor)
        {
            // Default for TryGetValue is 0 so if the councilor isn't in the Dictionary it's added with level 1, otherwise it's incremented
            CouncilorIDCouncilorLevelList.TryGetValue(councilor.ID, out var currentLevel);
            CouncilorIDCouncilorLevelList[councilor.ID] = currentLevel + 1;
        }

        /// <summary>
        /// Returns the int level associated with the TICouncilorState. Will add the key if it is missing w/ default level.
        /// </summary>
        /// <param name="councilor">The councilor to lookup</param>
        /// <returns></returns>
        public int? this[TICouncilorState councilor]
        {
            get
            {
                if (councilor == null)
                {
                    return null;
                }
                if (!CouncilorIDCouncilorLevelList.ContainsKey(councilor.ID))
                {
                    this.RegisterList(councilor);
                }
                if (CouncilorIDCouncilorLevelList.TryGetValue(councilor.ID, out var value)) return value;
                return null;
            }
        }

        /// <summary>
        /// Deregisters an existing member from the CouncilorIDCouncilorLevelList
        /// </summary>
        /// <param name="councilor">The instance to deregister</param>
        public void DeRegisterList(TICouncilorState councilor)
        {
            if (CouncilorIDCouncilorLevelList.ContainsKey(councilor.ID))
            {
                CouncilorIDCouncilorLevelList.Remove(councilor.ID);
            }
        }

        /// <summary>
        /// Sets the value of TICouncilorState councilor to int value
        /// </summary>
        /// <param name="councilor">The instance to update</param>
        /// <param name="value">The value to insert</param>
        public void UpdateRegisterList(TICouncilorState councilor, int value)
        {
            if (CouncilorIDCouncilorLevelList.ContainsKey(councilor.ID))
            {
                CouncilorIDCouncilorLevelList[councilor.ID] = value;
            }
        }

        /// <summary>
        /// Clears the CouncilorIDCouncilorLevelList Dictionary
        /// </summary>
        public void ClearCouncilorIDCouncilorLevelList()
        {
            CouncilorIDCouncilorLevelList.Clear();
        }

    }
}
