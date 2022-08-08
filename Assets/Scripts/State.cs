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
        /// 状态的转换
        /// </summary>
        private Dictionary<string, Func<bool>> transitionStates;
        /// <summary>
        /// 状态的周期函数
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
        /// 添加转换条件
        /// </summary>
        /// <param name="stateName">状态的名字</param>
        /// <param name="condition">转换的条件</param>
        public void AddTransitionCondition(string stateName, Func<bool> condition)
        {
            if (transitionStates.ContainsKey(stateName))
                transitionStates[stateName] = condition;
            else
                transitionStates.Add(stateName,condition);
        }
        /// <summary>
        /// 移除转换条件
        /// </summary>
        /// <param name="stateName">需要移除转换条件的条件名</param>
        public void RemoveTransitionState(string stateName)
        {
            if (transitionStates.ContainsKey(stateName))
            {
                transitionStates.Remove(stateName);
            }
        }
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="enterParameters">进入的参数</param>
        /// <param name="updateParameters">更新的参数</param>
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
        /// 退出状态
        /// </summary>
        /// <param name="parameters">退出状态的参数</param>
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