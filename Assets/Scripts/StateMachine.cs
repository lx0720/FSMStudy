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
        /// ͨ��״̬�����״̬�����ظ�״̬
        /// </summary>
        /// <param name="stateName">��Ҫ��ӵ�״̬��״̬��</param>
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
        /// ͨ��״̬���״̬
        /// </summary>
        /// <param name="state">��Ҫ��ӵ�״̬</param>
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
        /// �Ƴ�״̬
        /// </summary>
        /// <param name="stateName">��Ҫ�Ƴ�״̬������</param>
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
        /// ���߱�ת��������״̬
        /// </summary>
        /// <param name="exitParameters">״̬�˳��Ĳ���</param>
        /// <param name="enterParameters">��״̬����Ĳ���</param>
        /// <param name="updateParameters">��״̬���µĲ���</param>
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
