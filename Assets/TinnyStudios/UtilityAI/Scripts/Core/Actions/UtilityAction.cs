using System;
using System.Collections.Generic;
using TinnyStudios.AIUtility.Core.Properties;
using UnityEngine;
using UnityEngine.Events;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// The base implementation for Utility Action to be used by the planner.
    /// See the interface <see cref="IUtilityAction"/> for detailed comments.
    /// </summary>
    public abstract class UtilityAction : MonoBehaviour, IUtilityAction
    {
        public string Name;

        [Tooltip("The action score is multiplied by the weight. This determines its max score output and can be use for classifying priorities.")]
        [Range(0,1)]
        public float Weight = 1;
        
        [Range(0, 1)]
        public float MinScore;

        public List<Consideration> Considerations;

        public ActionPerformData PerformData;
        public ActionMoveData MoveData;
        public PropertySetRuntime PropertySet;
        public ActionEvents Events;

        public EActionStatus State;

        public Agent Agent { get; private set; }
        public float Score { get; private set; }
        public bool Initialized { get; set; }

        #region Interface Pointers
        float IUtilityAction.Weight => Weight;
        ActionMoveData IUtilityAction.MoveData => MoveData;
        string IUtilityAction.Name => Name;
        EActionStatus IUtilityAction.State => State;

        ActionEvents IUtilityAction.Events => Events;
        List<Consideration> IUtilityAction.Considerations => Considerations;
        PropertySetRuntime IUtilityAction.PropertySet => PropertySet;

        #endregion

        /// <summary>
        /// A set of Unity Events that the action can go through. Useful for quick prototypes.
        /// </summary>
        [Serializable]
        public class ActionEvents
        {
            public UnityEvent OnSetUp;
            public UnityEvent OnMoveBegin;
            public UnityEvent OnMoving;
            public UnityEvent OnPerformBegin;
            public UnityEvent OnPerform;
            public UnityEvent OnCompleted;
            public UnityEvent OnAbort;
        }

        public virtual void SetScore(float score)
        {
            Score = Mathf.Clamp(score, MinScore, 1);
        }

        public abstract EActionStatus Perform(Agent agent);

        public virtual bool IsAvailable() => true;

        public virtual void OnMove(MoveSystemBase moveSystem)
        {
            if (MoveData.Required)
            {
                moveSystem.SetDestination(MoveData.DestinationTransform);
                moveSystem.SetProperties(MoveData);
            }
        }

        public virtual void Setup(Agent agent)
        {
            Agent = agent;
            PropertySet.Initialize();
            InitializeDynamicConsideration();
        }

        public virtual void InitializeDynamicConsideration(){}

        public void AddConsideration(Consideration consideration)
        {
            Considerations.Add(consideration);
        }

        public void RemoveConsideration(Consideration consideration)
        {
            Considerations.Remove(consideration);
        }

        public virtual void OnConsiderationCheck(Consideration consideration)
        {
        }

        public virtual void OnMoveStarted(MoveSystemBase moveSystem) {}
        public virtual TimeWatcher TimeWatch { get; } = new StopwatchWatcher();
        public bool FoldOutEnabled { get; set; }
        public virtual void OnReachedDestination(){}
        public void SetState(EActionStatus state)
        {
            State = state;
        }

        public bool ReachedPerformDuration => TimeWatch.GetTotalSeconds() >= PerformData.Properties.Duration;

        /// <summary>
        /// While the running elapsed has not reached action perform duration, it'll return Running and keep the action continuing.
        /// Pass in any method group to be called on complete.
        /// </summary>
        /// <returns></returns>
        protected EActionStatus PerformByDuration(Agent agent)
        {
            if (!ReachedPerformDuration) 
                return EActionStatus.Running;

            OnPerformByDurationCompleted(agent);
            return EActionStatus.Completed;
        }
        
        /// <summary>
        /// Called when Perform By Duration is completed.
        /// Override this to execute code after it completes.
        /// </summary>
        /// <param name="agent"></param>
        protected virtual void OnPerformByDurationCompleted(Agent agent){}
    }

    /// <summary>
    /// The state the action is in. This is returned by <see cref="UtilityAction"/> Perform method.
    /// </summary>
    public enum EActionStatus
    {
        ///<summary>Has not been chosen</summary>
        NotStarted,
        ///<summary>Is moving towards destination</summary>
        Moving,
        ///<summary>The action is performing, return this in Perform method to keep going</summary>
        Running,
        ///<summary>Return Failed to cancel your action perform. i.e in the process, you realized it's not a success. </summary>
        Failed, 
        ///<summary>Return Completed so the action ends. </summary>
        Completed, 
    }
}