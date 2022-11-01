using PavonisInteractive.TerraInvicta;
using PavonisInteractive.TerraInvicta.Actions;
using PavonisInteractive.TerraInvicta.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace CouncilorLevels
{
    public class TICouncilorLevelState : TIGameState
    {
        [SerializeField]
        public int Level { get; set; } = 1;

        [SerializeField]
        public int CurrentLevel { get; set; } = 1;

        [SerializeField]
        public int TotalXP { get; set; } = 0;

        new public TICouncilorState ref_councilor;

        public bool AlreadyInit { get; set; } = false;
        public int? Persuasion { get; set; } = null;
        public int? Investigation { get; set; } = null;
        public int? Espionage { get; set; } = null;
        public int? Command { get; set; } = null;
        public int? Administration { get; set; } = null;
        public int? Science { get; set; } = null;
        public int? Security { get; set; } = null;

        public List<string> traitTemplateNames { get; set; }

        public void increment()
        {
            CurrentLevel += 1;
            if (Level <= CurrentLevel)
            {
                Level = CurrentLevel+0;
            }
            // TotalXP += levelCost;
            // Log.Info("Total XP is now " + TotalXP.ToString());
            return;
        }

        public void decrement()
        {
            if (CurrentLevel > 0)
            {
                CurrentLevel -= 1;
            }
        }

        public TICouncilorLevelState ref_councilorLevelState
        {
            get
            {
                return this;
            }
        }

        private void AssignAttributes(KeyValuePair<CouncilorAttribute, int> attribute)
        {
            // Log.Info("AssignAttrs");
            switch (attribute.Key)
            {
                case CouncilorAttribute.Persuasion:
                    this.Persuasion = attribute.Value;
                    break;
                case CouncilorAttribute.Investigation:
                    this.Investigation = attribute.Value;
                    break;
                case CouncilorAttribute.Espionage:
                    this.Espionage = attribute.Value;
                    break;
                case CouncilorAttribute.Command:
                    this.Command = attribute.Value;
                    break;
                case CouncilorAttribute.Administration:
                    this.Administration = attribute.Value;
                    break;
                case CouncilorAttribute.Science:
                    this.Science = attribute.Value;
                    break;
                case CouncilorAttribute.Security:
                    this.Security = attribute.Value;
                    break;
                default:
                    break;
            }

        }

        public void InitWithCouncilorState(TICouncilorState councilor)
        {
            bool flag = councilor.template == null;
            if (!flag)
            {
                this.ref_councilor = councilor;
                this.templateName = councilor.template.dataName;
                this.CurrentLevel = CurrentLevel;
                this.Level = Level;
                this.TotalXP = TotalXP;
                this.Persuasion = Persuasion;
                this.Investigation = Investigation;
                this.Espionage = Espionage;
                this.Command = Command;
                this.Administration = Administration;
                this.Science = Science;
                this.Security = Security;
                this.AlreadyInit = AlreadyInit;
                this.traitTemplateNames = traitTemplateNames;
            }
        }

        override public void PostInitializationInit_4()
        {
            try
            {
                // Log.Info("Init here");
                // Log.Info(this.ref_councilor.ID.ToString());
                if (!this.AlreadyInit)
                {
                    // Set default values from councilor at init
                    // Log.Info("Init here - inside bool check");
                    if (this.ref_councilor != null)
                    {
                        if (this.ref_councilor.attributes != null)
                        {
                            foreach (KeyValuePair<CouncilorAttribute, int> attribute in this.ref_councilor.attributes)
                            {
                                // Log.Info("Inside foreach");
                                this.AssignAttributes(attribute);
                            }
                            // Log.Info("Done with foreach");
                            this.traitTemplateNames = new List<string>(this.ref_councilor.traitTemplateNames);

                            // Flag that this step has been done once & does not need to be repeated
                            this.AlreadyInit = true;
                        }
                        else { // Log.Info("Attributes are null!");
                             }

                    }
                }

                // Log.Info("Outside bool check");
                CouncilorLevelManagerExternalMethods.AddCouncilorLevel(this.ref_councilor, this);
            }
            catch (System.Exception e)
            {
                // Log.Info(e.Message);
                throw;
            }

        }

        private int AdjustAttribute(int? input, CouncilorAttribute attribute)
        {
            return input - this.ref_councilor.attributes[attribute] ?? 0;
        }

        private void RemoveTraits()
        {
            // Log.Info("Remove Traits Here");
            for (int i = this.ref_councilor.traits.Count - 1; i >= 0; i--)
            {
                TITraitTemplate traitTemplate = this.ref_councilor.traits[i];
                // Log.Info("Removing trait " + traitTemplate.displayName);
                this.ref_councilor.RemoveTrait(traitTemplate);
                // Log.Info("Removed trait " + traitTemplate.displayName);
            }
            // Log.Info("Remove Traits Done");
        }

        private void RemoveOrgs()
        {
            // Log.Info("Remove orgs");
            TIFactionState faction = this.ref_councilor.faction;
            Player playerControl = faction.playerControl;
            List<TransferOrgToFactionPoolAction> list = new List<TransferOrgToFactionPoolAction>();
            foreach (TIOrgState tiorgState in this.ref_councilor.orgs)
            {
                list.Add(new TransferOrgToFactionPoolAction(tiorgState, this.ref_councilor));
            }
            foreach (TransferOrgToFactionPoolAction transferOrgToFactionPoolAction in list)
            {
                playerControl.StartAction(transferOrgToFactionPoolAction);
            }
            // Log.Info("Remove Orgs done");
        }

        private void ResetXP()
        {
            int total_xp = this.TotalXP;
            this.ref_councilor.ChangeXP(this.TotalXP);
            this.TotalXP = total_xp;
            this.CurrentLevel = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetTraits()
        {
            foreach (string traitName in this.traitTemplateNames)
            {
                // Log.Info("Adding trait " + traitName);
                this.ref_councilor.AddTrait(traitName);
                // Log.Info("Added trait " + traitName);
            }
            // Log.Info("Add traits done");
        }

        /// <summary>
        /// This is implemented in TICouncilorState.AssignStatsFromTemplate but in the TI instance includes setting loyalty, and it is private.
        /// </summary>
        private void AssignStatsFromTemplate()
        {
            // Log.Info("Setting stats");
            this.ref_councilor.ModifyAttribute(CouncilorAttribute.Persuasion, this.AdjustAttribute(this.Persuasion, CouncilorAttribute.Persuasion));
            this.ref_councilor.ModifyAttribute(CouncilorAttribute.Espionage, this.AdjustAttribute(this.Espionage, CouncilorAttribute.Espionage));
            this.ref_councilor.ModifyAttribute(CouncilorAttribute.Command, this.AdjustAttribute(this.Command, CouncilorAttribute.Command));
            this.ref_councilor.ModifyAttribute(CouncilorAttribute.Investigation, this.AdjustAttribute(this.Investigation, CouncilorAttribute.Investigation));
            this.ref_councilor.ModifyAttribute(CouncilorAttribute.Science, this.AdjustAttribute(this.Science, CouncilorAttribute.Science));
            this.ref_councilor.ModifyAttribute(CouncilorAttribute.Administration, this.AdjustAttribute(this.Administration, CouncilorAttribute.Administration));
            this.ref_councilor.ModifyAttribute(CouncilorAttribute.Security, this.AdjustAttribute(this.Security, CouncilorAttribute.Security));
            // Log.Info("Done setting stats");
        }

        public void Respec()
        {
            // Log.Info("Remove all orgs");
            this.RemoveOrgs();

            // Log.Info("Remove all attrs & add all attrs from template");
            this.AssignStatsFromTemplate();

            // Log.Info("Remove all traits");
            this.RemoveTraits();

            // Log.Info("Set all traits to default");
            this.SetTraits();

            // Log.Info("Change XP");
            this.ResetXP();

            // Log.Info("Done");
        }
    }
}
