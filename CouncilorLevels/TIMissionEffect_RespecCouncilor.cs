using PavonisInteractive.TerraInvicta;

namespace CouncilorLevels
{
    class TIMissionEffect_RespecCouncilor : TIMissionEffect
    {
        public override string ApplyEffect(TIMissionState mission, TIGameState target, TIMissionOutcome outcome = TIMissionOutcome.Success)
        {
            Log.Info("ApplyEffect 1");
            CouncilorLevelManagerExternalMethods.Respec(mission.councilor);
            Log.Info("ApplyEffect 2");
            return string.Empty;
        }
    }
}
