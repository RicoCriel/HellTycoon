using System.Collections;
using System.Collections.Generic;
using Economy;
using TinnyStudios.AIUtility;
using UnityEngine;
using static FreeBuild.FreeBuildUI;

namespace Tycoons
{
    public class ChangeToStatAction : UtilityAction
    {
        [SerializeField] private StatType _statType;


        public override EActionStatus Perform(Agent agent)
        {
            return PerformByDuration(agent);
        }

        public override bool IsAvailable()
        {
            var context = Agent.GetContext<TycoonDataContext>();

            if (context != null)
            {
                return context.CurrentProduction != _statType && context.CanChangeProduction;
            }

            return true;
        }

        protected override void OnPerformByDurationCompleted(Agent agent)
        {
            var context = agent.GetContext<TycoonDataContext>();

            if (context != null)
            {
                context.EconomyManager.BuyObject(1000f, context.TycoonType);
                context.CurrentProduction = _statType;
                context.CanChangeProduction = false;
                context.OnChangeProduction?.Invoke();
            }
        }
    }
}

