using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FSMFrame
{
    public class StateMachine : State
    {
        private State defaultState;
        private State currentState;

        public StateMachine(string stateName) : base(stateName)
        {
            manageredStates = new Dictionary<string, State>();
            FSMMachineUpdateEventBind();
        }

        private void FSMMachineUpdateEventBind()
        {
            OnStateUpdate += CheckAllTransitionCondition;
        }

        private Dictionary<string, State> manageredStates;
        /// <summary>
        /// 通过状态名添加状态并返回该状态
        /// </summary>
        /// <param name="stateName">需要添加的状态的状态名</param>
        /// <returns></returns>
        public State AddManageredState(string stateName)
        {
            if (!StateStatus)
            {
                if (manageredStates.ContainsKey(stateName))
                    return manageredStates[stateName];
                State state = new State(stateName);
                manageredStates.Add(stateName, state);
                if (manageredStates.Count == 1)
                    defaultState = state;
                return state;
            }
            return null;
        }
        /// <summary>
        /// 通过状态添加状态
        /// </summary>
        /// <param name="state">需要添加的状态</param>
        public void AddManageredState(State state)
        {
            if (!StateStatus)
            {
                if (manageredStates.ContainsKey(state.StateName))
                    return;
                manageredStates.Add(state.StateName, state);
                if (manageredStates.Count == 1)
                    defaultState = state;
            }
        }
        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="stateName">需要移除状态的名字</param>
        public void RemoveManageredState(string stateName)
        {
            if(manageredStates.ContainsKey(stateName) && !StateStatus)
            {
                manageredStates.Remove(stateName);
            }
        }

        public override void EnterState( object[] enterParameters,object[] updateParameters)
        {
            base.EnterState( enterParameters,updateParameters);

            if (defaultState == null)
                return;
            currentState = defaultState;
            currentState.EnterState(enterParameters, updateParameters);
        }
        public override void ExitState(object[] exitParameters)
        {
            if (currentState != null)
                currentState.ExitState(exitParameters);
            base.ExitState(exitParameters);
        }

        /// <summary>
        /// 检查具备转换条件的状态
        /// </summary>
        /// <param name="exitParameters">状态退出的参数</param>
        /// <param name="enterParameters">新状态进入的参数</param>
        /// <param name="updateParameters">新状态更新的参数</param>
        public void CheckAllTransitionCondition(object[] updateParameters) 
        {
            string transitionStateName = currentState.GetWillTransitionState();
            if (transitionStateName == null)
                return;
            TransitionToNewState(transitionStateName);
        }

        private void TransitionToNewState(string newStateName)
        {
            if (!manageredStates.ContainsKey(newStateName))
                return;
            currentState.ExitState(null);
            currentState = manageredStates[newStateName];
            manageredStates[newStateName].EnterState(null,null);
        }
    }
}
