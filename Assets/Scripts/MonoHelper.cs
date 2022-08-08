using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FSMFrame
{
    public class MonoHelper : MonoBehaviour
    {
        class StateUpdateModule
        {
            public Action<object[]> updateAction;
            public object[] updateParameters;
            public StateUpdateModule(Action<object[]> action,object[] parameters)
            {
                updateAction = action;
                updateParameters = parameters;
            }
        }
        public static MonoHelper instance;
        public float updateInterval = 0f;

        private Dictionary<string, StateUpdateModule> stateUpdateModuleDict;
        private StateUpdateModule[] stateUpdateModules;

        private void Awake()
        {
            instance = this;
            stateUpdateModuleDict = new Dictionary<string, StateUpdateModule>();
        }

        public void AddUpdateEvent(string stateName,Action<object[]> action,object[] parameters)
        {
            if (!stateUpdateModuleDict.ContainsKey(stateName))
            {
                stateUpdateModuleDict.Add(stateName,new StateUpdateModule(action,parameters));
            }
            DictToArray();
        }

        public void RemoveUpdateEvent(string stateName)
        {
            if (stateUpdateModuleDict.ContainsKey(stateName))
            {
                stateUpdateModuleDict.Remove(stateName);
            }
            DictToArray();
        }

        private void DictToArray()
        {
            stateUpdateModules = new StateUpdateModule[stateUpdateModuleDict.Count];
            int interval = 0;
            foreach(var item in stateUpdateModuleDict)
            {
                stateUpdateModules[interval] = item.Value;
                interval++;
            }
        }

        private IEnumerator Start()
        {
            while (true)
            {
                if(updateInterval <0)
                {
                    yield return 0;
                }
                else
                {
                    yield return new WaitForSeconds(updateInterval);
                }

                for(int i = 0; i < stateUpdateModules.Length; i++)
                {
                    stateUpdateModules[i].updateAction(stateUpdateModules[i].updateParameters);
                }

            }
        }
    }
}
