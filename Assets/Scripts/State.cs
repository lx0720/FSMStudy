using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMFrame
{
    public class State
    {
        public string StateName { private set; get; }
        public bool StateStatus { private set; get; }

        public State(string stateName) 
        {
            StateName = stateName;
            transitionStates = new Dictionary<string, Func<bool>>();
            StateStatusBindEvent();
        }
        /// <summary>
        /// ״̬��ת��
        /// </summary>
        private Dictionary<string, Func<bool>> transitionStates;
        /// <summary>
        /// ״̬�����ں���
        /// </summary>
        public event Action<object[]> OnStateEnter;
        public event Action<object[]> OnStateUpdate;
        public event Action<object[]> OnStateExit;

        public void StateStatusBindEvent() 
        {
            OnStateEnter += objects => { StateStatus = true; };
            OnStateExit += objects => { StateStatus = false; };
        }


        /// <summary>
        /// ���ת������
        /// </summary>
        /// <param name="stateName">״̬������</param>
        /// <param name="condition">ת��������</param>
        public void AddTransitionCondition(string stateName, Func<bool> condition)
        {
            if (transitionStates.ContainsKey(stateName))
                transitionStates[stateName] = condition;
            else
                transitionStates.Add(stateName,condition);
        }
        /// <summary>
        /// �Ƴ�ת������
        /// </summary>
        /// <param name="stateName">��Ҫ�Ƴ�ת��������������</param>
        public void RemoveTransitionState(string stateName)
        {
            if (transitionStates.ContainsKey(stateName))
            {
                transitionStates.Remove(stateName);
            }
        }
        /// <summary>
        /// ����״̬
        /// </summary>
        /// <param name="enterParameters">����Ĳ���</param>
        /// <param name="updateParameters">���µĲ���</param>
        public virtual void EnterState(object[] enterParameters,object[] updateParameters)
        {
            if (OnStateEnter != null)
            {
                OnStateEnter(enterParameters);
            }
            //update
            MonoHelper.instance.AddUpdateEvent(StateName, OnStateUpdate, updateParameters);
        }
        /// <summary>
        /// �˳�״̬
        /// </summary>
        /// <param name="parameters">�˳�״̬�Ĳ���</param>
        public virtual void ExitState(object[] parameters)
        {
            //update
            MonoHelper.instance.RemoveUpdateEvent(StateName);
            if (OnStateExit != null)
                OnStateExit(parameters);
        }

        public string GetWillTransitionState() 
        {
            foreach(var item in transitionStates)
            {
                if (item.Value())
                {
                    return item.Key;
                }
            }
            return null;
        }
    }
}