﻿// State Machine interpreted from Adaberto's lecture slides in AI workshops (Interpreter: Tristan Bampton).

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SM
{

    public delegate void Action();

    public class StateMachine
    {

        public List<State> States = new List<State>();
        public State InitialState;
        public State CurrentState;

        private Transition triggeredTransition;

        /// <summary>
        /// Constructor for StateMachine
        /// </summary>
        /// <param name="initialState">The state that the machine should start on, if null will start at the first state passed in.</param>
        /// <param name="states">The states this machine will have.</param>
        public StateMachine(State initialState, List<State> states)
        {
            SetupMachine(initialState, states.ToArray());
        }

        /// <summary>
        /// Constructor for StateMachine
        /// </summary>
        /// <param name="initialState">The state that the machine should start on, if null will start at the first state passed in.</param>
        /// <param name="states">The states this machine will have.</param>
        public StateMachine(State initialState, params State[] states)
        {
            SetupMachine(initialState, states);
        }

        public void InitMachine()
        {
            CurrentState = InitialState;

            foreach (Action action in CurrentState.EntryActions)
            {
                action();
            }
        }

        public void SMUpdate()
        {
            triggeredTransition = null;
            List<Action> ReturnList = new List<Action>();

            // Go through each possible transition until one if found to be triggered.
            foreach (Transition transition in CurrentState.Transitions)
            {
                if (transition.IsTriggered)
                {
                    triggeredTransition = transition;
                    break;
                }
            }

            // If a transition has been triggered queue up the necessary actions.
            if (triggeredTransition != null)
            {
                State targetState = triggeredTransition.TargetState;

                if (CurrentState.ExitActions.Count > 0)
                {
                    ReturnList.AddRange(CurrentState.ExitActions);
                }

                if (triggeredTransition.Actions.Count > 0)
                {
                    ReturnList.AddRange(triggeredTransition.Actions);
                }

                if (targetState.EntryActions.Count > 0)
                {
                    ReturnList.AddRange(targetState.EntryActions);
                }

                CurrentState = targetState;
            }
            else // If no transition has happened continue with this states actions.
            {
                if (CurrentState.Actions.Count > 0)
                {
                    ReturnList.AddRange(CurrentState.Actions);
                }
            }

            foreach (Action a in ReturnList)
            {
                a();
            }
        }

        public string GetCurrentState()
        {
            return CurrentState.Name;
        }

        private void SetupMachine(State initialState, State[] states)
        {
            States.AddRange(states);

            if (initialState != null)
            {
                InitialState = initialState;
            }
            else
            {
                InitialState = States[0];
            }
        }
    }

    public class State
    {
        public string Name;
        public List<Transition> Transitions = new List<Transition>();
        public List<Action> EntryActions = new List<Action>();
        public List<Action> Actions = new List<Action>();
        public List<Action> ExitActions = new List<Action>();

        /// <summary>
        /// Constructor for State
        /// </summary>
        /// <param name="name">The name of the state.</param>
        /// <param name="transitions">A list of transitions that this state has.</param>
        /// <param name="entryActions">A list of this states entry actions.</param>
        /// <param name="actions">A list of this states actions.</param>
        /// <param name="exitActions">A list of this states exit actions.</param>
        public State(string name, List<Transition> transitions, List<Action> entryActions, List<Action> actions, List<Action> exitActions)
        {
            SetupState(name,
                (transitions != null) ? transitions.ToArray() : null,
                (entryActions != null) ? entryActions.ToArray() : null,
                (actions != null) ? actions.ToArray() : null,
                (exitActions != null) ? exitActions.ToArray() : null);
        }

        /// <summary>
        /// Constructor for State
        /// </summary>
        /// <param name="name">The name of the state.</param>
        /// <param name="transitions">A list of transitions that this state has.</param>
        /// <param name="entryActions">A list of this states entry actions.</param>
        /// <param name="actions">A list of this states actions.</param>
        /// <param name="exitActions">A list of this states exit actions.</param>
        public State(string name, Transition[] transitions, Action[] entryActions, Action[] actions, Action[] exitActions)
        {
            SetupState(name, transitions, entryActions, actions, exitActions);
        }

        private void SetupState(string name, Transition[] transitions, Action[] entryActions, Action[] actions, Action[] exitActions)
        {
            Name = name;
            SetupTransitions(transitions);
            SetupEntryActions(entryActions);
            SetupActions(actions);
            SetupExitActions(exitActions);
        }

        private void SetupTransitions(Transition[] transitions)
        {
            if (transitions != null)
            {
                if (transitions.Length > 0)
                {
                    Transitions.AddRange(transitions);
                }
            }
        }

        private void SetupEntryActions(Action[] entryActions)
        {
            if (entryActions != null)
            {
                if (entryActions.Length > 0)
                {
                    EntryActions.AddRange(entryActions);
                }
            }
        }

        private void SetupActions(Action[] actions)
        {
            if (actions != null)
            {
                if (actions.Length > 0)
                {
                    Actions.AddRange(actions);
                }
            }
        }

        private void SetupExitActions(Action[] exitActions)
        {
            if (exitActions != null)
            {
                if (exitActions.Length > 0)
                {
                    ExitActions.AddRange(exitActions);
                }
            }
        }

    }

    public class Transition
    {
        public string Name;
        public List<Action> Actions = new List<Action>();
        public Condition.ICondition TransitionCondition;
        public State TargetState;

        /// <summary>
        /// Constructor for Transition (Don't forget to add the Target state after states have been made)
        /// </summary>
        /// <param name="name">The name of the transition</param>
        /// <param name="condition">The condition for the transition to fire.</param>
        /// <param name="actions">Any actions that should be performed whist transitioning.</param>
        public Transition(string name, Condition.ICondition condition, List<Action> actions)
        {
            SetupTransition(name, condition, actions.ToArray());
        }

        /// <summary>
        /// Constructor for Transition (Don't forget to add the Target state after states have been made)
        /// </summary>
        /// <param name="name">The name of the transition</param>
        /// <param name="condition">The condition for the transition to fire.</param>
        /// <param name="actions">Any actions that should be performed whist transitioning.</param>
        public Transition(string name, Condition.ICondition condition, params Action[] actions)
        {
            SetupTransition(name, condition, actions);
        }

        public void SetTargetState(State targetState)
        {
            TargetState = targetState;
        }

        public bool IsTriggered
        {
            get
            {
                return TransitionCondition.Test();
            }
        }

        private void SetupTransition(string name, Condition.ICondition condition, Action[] actions)
        {
            Name = name;
            TransitionCondition = condition;
            if (actions.Length > 0)
            {
                Actions.AddRange(actions);
            }
        }
    }

}


namespace Condition
{

    public interface ICondition
    {
        bool Test();
    }

    /// <summary>
    /// Test Value between Min and Max values
    /// </summary>
    public class FloatCondition : ICondition
    {
        public float MinValue;
        public float MaxValue;

        public delegate float FloatParam();
        public FloatParam TestValue;

        bool ICondition.Test()
        {
            return (MinValue <= TestValue()) && (TestValue() <= MaxValue);
        }
    }

    /// <summary>
    /// A less than or equal to B
    /// </summary>
    public class LessThanFloatCondition : ICondition
    {
        public delegate float FloatParam();
        public FloatParam A;
        public FloatParam B;

        bool ICondition.Test()
        {
            return A() <= B();
        }
    }

    /// <summary>
    /// A greater than or equal to B
    /// </summary>
    public class GreaterThanFloatCondition : ICondition
    {
        public delegate float FloatParam();
        public FloatParam A;
        public FloatParam B;

        bool ICondition.Test()
        {
            return A() >= B();
        }
    }

    public class BoolCondition : ICondition
    {
        public delegate bool BoolParam();
        public BoolParam Condition;

        bool ICondition.Test()
        {
            return Condition();
        }
    }

    public class AndCondition : ICondition
    {
        public ICondition ConditionA;
        public ICondition ConditionB;

        bool ICondition.Test()
        {
            return ConditionA.Test() && ConditionB.Test();
        }
    }

    public class OrCondition : ICondition
    {
        public ICondition ConditionA;
        public ICondition ConditionB;

        bool ICondition.Test()
        {
            return ConditionA.Test() || ConditionB.Test();
        }
    }

    public class NotCondition : ICondition
    {
        public ICondition Condition;

        bool ICondition.Test()
        {
            return !Condition.Test();
        }
    }

    /// <summary>
    /// True if null
    /// </summary>
    public class NullCondition : ICondition
    {
        public delegate object ObjectParam();
        public ObjectParam Condition;

        bool ICondition.Test()
        {
            return Condition() == null;
        }
    }
}
