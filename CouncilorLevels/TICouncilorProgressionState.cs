using System.Collections.Generic;
using PavonisInteractive.TerraInvicta;
using UnityEngine;

namespace CouncilorLevels
{
    public class TICouncilorProgressionState : TIGameState
    {
        [SerializeField]
        public int Level { get; set; } = 1;

        new public TICouncilorState ref_councilor;
        public Dictionary<int, TICouncilorLevelState> progression = new Dictionary<int, TICouncilorLevelState>();

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
                this.ref_councilor = councilor;
                this.templateName = councilor.template.dataName;
                this.Level = Level;
            }
        }

        override public void PostInitializationInit_4()
        {
            CouncilorLevelManagerExternalMethods.AddCouncilorLevel(this.ref_councilor, this);
        }

        public void Respec()
        {
            // Remove all orgs
            // Set all attributes & traits to their default values
            // Refund all XP

        }
    }
}
