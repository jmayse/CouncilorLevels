using System.Collections.Generic;
using PavonisInteractive.TerraInvicta;
using UnityEngine;

namespace CouncilorLevels
{
    public class TICouncilorLevelState : TICouncilorState
    {
        [SerializeField]
        public int Level { get; set; } = 0;

        // new public TICouncilorState ref_councilor;

        public void increment()
        {
            Level += 1;
        }

        public void decrement()
        {
            Level -= 1;
        }

        public TICouncilorLevelState ref_councilorLevelState
        {
            get
            {
                return this;
            }
        }

        public void InitWithCouncilorState(TICouncilorState councilor)
        {
            bool flag = councilor.template == null;
            if (!flag)
            {
                this.templateName = councilor.template.dataName;
                this.Level = Level;               
            }
        }

    }

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
        public void RegisterList(TICouncilorState councilor)
        {
            Log.Info("RegisterList 1");
            if (!CouncilorIDCouncilorLevelList.ContainsKey(councilor.ID))
            {
                Log.Info("RegisterList 2"); // Game crashes after logging this line
                TICouncilorLevelState tiCouncilorLevelState = GameStateManager.CreateNewGameState<TICouncilorLevelState>();
                Log.Info("RegisterList 3"); // This line is never logged
                tiCouncilorLevelState.InitWithCouncilorState(councilor);
                Log.Info("RegisterList 4");
                CouncilorIDCouncilorLevelList.Add(councilor.ID, tiCouncilorLevelState);
                Log.Info("RegisterList 5");
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
