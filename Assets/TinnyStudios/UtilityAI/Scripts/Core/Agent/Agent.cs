using System;
using System.Collections;
using System.Collections.Generic;
using TinnyStudios.AIUtility.Core.Properties;
using UnityEngine;
using UnityEngine.Events;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// The brain of the Utility AI. It controls the  states and uses the <see cref="UtilityPlanner"/> to find the best action.
    /// </summary>
    [RequireComponent(typeof(UtilityPlanner))]
    public class Agent : MonoBehaviour
    {
        public UtilityPlanner UtilityPlanner;
        public EAgentState State;
        public MoveSystemBase MoveSystem;

        public List<IUtilityAction> Actions = new List<IUtilityAction>();

        public IUtilityAction CurrentAction { get; private set; }

        public IAgentDataContext Context { get; private set; }

        public AgentEvents Events;

        public PropertySetRuntime PropertySet;

        /// <summary>
        /// A set of Unity Events of all agent's changes. Useful for quickly prototyping.
        /// </summary>
        [Serializable]
        public class AgentEvents
        {
            public UnityEvent<EAgentState, EAgentState> OnStateChanged;
            public UnityEvent OnActionSkipped;
            public UnityEvent<IUtilityAction> OnPerform;
            public UnityEvent<IUtilityAction> OnPerformBegin;
            public UnityEvent<IUtilityAction> OnMoveBegin;
            public UnityEvent<IUtilityAction> OnMove;
            public UnityEvent<IUtilityAction> OnPerformCompleted;
            public UnityEvent<IUtilityAction> OnPlanFound;
            public UnityEvent OnNoPlanFound;
            public UnityEvent OnIdle;

            public UnityEvent<IUtilityAction> OnAbort;
            public UnityEvent<IUtilityAction> OnFailed;
        }

        private void OnValidate()
        {
            if (UtilityPlanner == null)
                UtilityPlanner = GetComponent<UtilityPlanner>();
        }

        protected virtual void Awake()
        {
            Initialize();

            if (UtilityPlanner.StartPlanOnAwake)
                StartPlan();
        }

        protected virtual Coroutine StartPlan()
        {
            return StartCoroutine(StartPlanRoutine());

            IEnumerator StartPlanRoutine()
            {
                yield return new WaitForSeconds(UtilityPlanner.StartPlanDelay);
                GoToState(EAgentState.Plan);
            }
        }

        public virtual void Initialize()
        {
            Context = GetComponentInChildren<IAgentDataContext>();
            PropertySet?.Initialize();

            SearchForActionsInParent();
            SetupActions();
        }

        public virtual void SetupActions()
        {
            foreach (var utilityAction in Actions)
                SetupAction(utilityAction);
        }

        public virtual void SetupAction(IUtilityAction utilityAction)
        {
            if (utilityAction.Initialized)
                return;

            utilityAction.Setup(this);
            utilityAction.Initialized = true;
            utilityAction.Events.OnSetUp?.Invoke();
        }

        /// <summary>
        /// Searches for actions available on the Agent. 
        /// </summary>
        public virtual void SearchForActionsInParent()
        {
            if(UtilityPlanner.FindActionsOnAwake)
                GetComponentsInChildren<IUtilityAction>(UtilityPlanner.IncludeInActiveActions, Actions);
        }

        protected virtual void Update()
        {
            if (UtilityPlanner.UpdateType == EUpdateType.Update)
                ExecutePlan();
        }

        protected virtual void FixedUpdate()
        {
            if (UtilityPlanner.UpdateType == EUpdateType.FixedUpdate)
                ExecutePlan();
        }

        protected virtual void LateUpdate()
        {
            if (UtilityPlanner.UpdateType == EUpdateType.LateUpdate)
                ExecutePlan();
        }

        public void ExecutePlan()
        {
            if (UtilityPlanner.Paused)
                return;

            switch (State)
            {
                case EAgentState.Plan:
                    FindPlan();

                    if (IsCurrentActionValid())
                        GoToState(CurrentAction.MoveData.Required ? EAgentState.Moving : EAgentState.Performing);

                    break;
                case EAgentState.Moving:

                    Events.OnMove?.Invoke(CurrentAction);
                    CurrentAction.Events.OnMoving?.Invoke();
                    CurrentAction.OnMove(MoveSystem);

                    if (MoveSystem.ReachedDestination())
                    {
                        CurrentAction.OnReachedDestination();
                        GoToState(EAgentState.Performing);
                    }

                    break;
                case EAgentState.Performing:
                    if (IsCurrentActionValid())
                        Perform();
                    break;
                case EAgentState.Aborting:
                    // We want to do it here as we want to give aborting 1 frame. 
                    // If it happens in GoTo, the agent will never really have a frame in aborting.
                    CurrentAction = null;
                    GoToState(EAgentState.Idle);
                    break;
            }
        }

        public void ResetCurrentAction()
        {
            if (CurrentAction == null) 
                return;

            CurrentAction.SetState(EActionStatus.NotStarted);
            CurrentAction.TimeWatch.Stop();
        }

        /// <summary>
        /// Is the current action valid to use?
        /// </summary>
        /// <returns></returns>
        public virtual bool IsCurrentActionValid()
        {
            if (CurrentAction == null)
                return false;

            if (!UtilityPlanner.ExecuteEvenZeroScoreActions && CurrentAction.Score <= 0.0f)
            {
                this.Log(CurrentAction.Name, "Skipped as Score is 0.", EAgentLogType.Skip);
                Events.OnActionSkipped?.Invoke();
                return false;
            }

            return true;
        }

        public virtual void Perform()
        {
            this.Log(CurrentAction.Name, "Performed", EAgentLogType.Perform);
            Events.OnPerform?.Invoke(CurrentAction);
            CurrentAction.Events.OnPerform?.Invoke();

            var result = CurrentAction.Perform(this);
            CurrentAction.SetState(result);

            switch (CurrentAction.State)
            {
                case EActionStatus.Completed:
                    this.Log(CurrentAction.Name, "Completed", EAgentLogType.Completed);
                    Events.OnPerformCompleted?.Invoke(CurrentAction);
                    CurrentAction.Events.OnCompleted?.Invoke();
                    GoToState(EAgentState.Idle);
                    break;
                case EActionStatus.Failed:
                    Events.OnFailed?.Invoke(CurrentAction);
                    AbortPlan();
                    break;
            }
        }

        public Dictionary<EAgentState, string> StateNames = new Dictionary<EAgentState, string>()
        {
            {EAgentState.Aborting, "Aborting"},
            {EAgentState.Idle, "Idle"},
            {EAgentState.Moving, "Moving"},
            {EAgentState.Performing, "Performing"},
            {EAgentState.Plan, "Plan"},
        };

        public void GoToState(EAgentState state)
        {
            if (State == state) 
                return;

            this.Log("Transition to State", StateNames[state], EAgentLogType.Transition);
            Events.OnStateChanged?.Invoke(State, state);

            State = state;

            if (State != EAgentState.Moving && MoveSystem != null)
                MoveSystem.Stop();

            switch (state)
            {
                case EAgentState.Idle:
                    Events.OnIdle?.Invoke();
                    PauseFor(UtilityPlanner.IdleDuration);
                    break;
                case EAgentState.Moving:
                    CurrentAction.SetState(EActionStatus.Moving);
                    CurrentAction.OnMoveStarted(MoveSystem);
                    Events.OnMoveBegin?.Invoke(CurrentAction);
                    CurrentAction.Events.OnMoveBegin?.Invoke();
                    break;
                case EAgentState.Performing:
                    CurrentAction.TimeWatch.Restart();
                    Events.OnPerformBegin?.Invoke(CurrentAction);
                    CurrentAction.Events.OnPerformBegin?.Invoke();
                    break;
                case EAgentState.Aborting:

                    if (CurrentAction != null)
                    {
                        CurrentAction.SetState(EActionStatus.NotStarted);
                        CurrentAction.TimeWatch.Stop();
                    }

                    break;
            }
        }

        /// <summary>
        /// Find a plan and returns true if it exists.
        /// </summary>
        /// <returns></returns>
        public bool FindPlan()
        {
            ResetCurrentAction();
            CurrentAction = UtilityPlanner.GetBestAction(this);

            if (CurrentAction != null)
            {
                this.Log("Plan Found", CurrentAction.Name, EAgentLogType.Plan);
                Events.OnPlanFound?.Invoke(CurrentAction);

                return true;
            }
            else
            {
                Events.OnNoPlanFound?.Invoke();
                return false;
            }
        }

        public T GetContext<T>() where T : IAgentDataContext
        {
            if (Context == null)
                Context = GetComponentInChildren<IAgentDataContext>();

            var context = (T)Context;
            return context;
        }

        public void AbortPlan()
        {
            if (CurrentAction == null)
                return;

            this.Log($"Cancelled Action", CurrentAction.Name, EAgentLogType.Abort);
            Events.OnAbort?.Invoke(CurrentAction);
            CurrentAction.Events.OnAbort?.Invoke();
            GoToState(EAgentState.Aborting);
        }

        /// <summary>
        /// Set UtilityPlanner pause to true and transition  to idle for duration and then transition to planning. 
        /// </summary>
        public virtual Coroutine PauseFor(float duration, EAgentState postState = EAgentState.Plan, Action postAction = null)
        {
            if (PauseRoutine != null)
                StopCoroutine(PauseRoutine);

            if (duration == 0)
            {
                Resume();
                return null;
            }

            return PauseRoutine = StartCoroutine(Routine());

            IEnumerator Routine()
            {
                GoToState(EAgentState.Idle);
                UtilityPlanner.Paused = true;
                yield return new WaitForSeconds(duration);
                Resume();
            }

            void Resume()
            {
                UtilityPlanner.Paused = false;
                GoToState(postState);
                postAction?.Invoke();
            }
        }

        public void ReplaceActions(List<IUtilityAction> actions)
        {
            Actions = actions;
            SetupActions();
        }

        public Coroutine PauseRoutine;

        /// <summary>
        /// Add and set up the action.
        /// </summary>
        /// <param name="utilityAction"></param>
        public void AddAction(UtilityAction utilityAction)
        {
            Actions.Add(utilityAction);
            SetupAction(utilityAction);
        }
    }

    /// <summary>
    /// An interface for inferring and finding agent data context.
    /// The agent data context can be any data, it just gets cast and cached to its concrete type.
    /// </summary>
    public interface IAgentDataContext
    {
    }
}
