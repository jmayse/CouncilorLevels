using System.Collections.Generic;
using PavonisInteractive.TerraInvicta;
using UnityEngine;

namespace CouncilorLevels
{
    public class CouncilorLevelManager
    {
        /// <summary>
        /// A Dictionary containing all levels keyed to TICouncilorState
        /// </summary>
        public Dictionary<GameStateID, TICouncilorLevelState> CouncilorIDCouncilorLevelList = new Dictionary<GameStateID, TICouncilorLevelState>();

        /// <summary>
        /// Returns the int level associated with the TICouncilorState. Will add the key if it is missing w/ default level.
        /// </summary>
        /// <param name="councilor">The councilor to lookup</param>
        /// <returns></returns>
        public TICouncilorLevelState this[TICouncilorState councilor]
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
        /// Registers a new member in the CouncilorIDCouncilorLevelList keyed by their ID with default level 1
        /// </summary>
        /// <param name="councilor">The instance to register</param>
        public void RegisterList(TICouncilorState councilor, TICouncilorLevelState councilorLevel=null)
        {
            if (!CouncilorIDCouncilorLevelList.ContainsKey(councilor.ID))
            {
                bool flag = (councilorLevel == null);
                if (!flag)
                {
                    CouncilorIDCouncilorLevelList.Add(councilor.ID, councilorLevel);
                }
                else
                {
                    TICouncilorLevelState tiCouncilorLevelState = GameStateManager.CreateNewGameState<TICouncilorLevelState>();
                    tiCouncilorLevelState.InitWithCouncilorState(councilor);
                    CouncilorIDCouncilorLevelList.Add(councilor.ID, tiCouncilorLevelState);
                }
            }

        }

        /// <summary>
        /// Deregisters an existing member from the CouncilorIDCouncilorLevelList
        /// </summary>
        /// <param name="councilor">The instance to deregister</param>
        public void DeRegisterList(TICouncilorState councilor)
        {
            if (!CouncilorIDCouncilorLevelList.ContainsKey(councilor.ID))
            {
                TICouncilorLevelState councilorLevel = this[councilor];
                bool result = GameStateManager.RemoveGameState<TICouncilorLevelState>(councilorLevel.ID, false);
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
                this[councilor].Level = value;
            }
        }

        /// <summary>
        /// Clears the CouncilorIDCouncilorLevelList Dictionary
        /// </summary>
        public void ClearCouncilorIDCouncilorLevelList()
        {
            CouncilorIDCouncilorLevelList.Clear();
        }

        /// <summary>
        /// Increment a councilor's level
        /// </summary>
        /// <param name="councilor"></param>
        public void IncrementCouncilorLevel(TICouncilorState councilor)
        {
            if (CouncilorIDCouncilorLevelList.ContainsKey(councilor.ID))
            {
                this[councilor].increment();
            }
        }

        /// <summary>
        /// Decrement a councilor's level
        /// </summary>
        /// <param name="councilor"></param>
        public void DecrementCouncilorLevel(TICouncilorState councilor)
        {
            if (CouncilorIDCouncilorLevelList.ContainsKey(councilor.ID))
            {
                this[councilor].decrement();
            }
        }
    }
}
