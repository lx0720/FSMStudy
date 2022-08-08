using System;
using UnityEngine;
namespace FSMFrame
{
    public class FSM : MonoBehaviour
    {
        [Range(0,6)]
        public float speed;

        private void Start()
        {
            StateMachine mainMachine = new StateMachine("Main");
            State idleState = new State("Idle");
            mainMachine.AddManageredState(idleState);
            StateMachine movementMachine = new StateMachine("Movement");
            mainMachine.AddManageredState(movementMachine);
            State walkState = new State("Walk");
            State runState = new State("Run");
            movementMachine.AddManageredState(walkState);
            movementMachine.AddManageredState(runState);

            idleState.AddTransitionCondition(movementMachine.StateName,()=> { return speed > 1.0f; });
            idleState.AddTransitionCondition(walkState.StateName, () => { return speed > 1.0f; });
            walkState.AddTransitionCondition(runState.StateName, () => { return speed > 4.0f; });
            movementMachine.AddTransitionCondition(idleState.StateName, () => { return speed < 1.0f; });
            runState.AddTransitionCondition(walkState.StateName, () => { return speed < 4.0f; });

            mainMachine.OnStateEnter += objects => { Debug.Log("mainMachine Enter!"); };
            mainMachine.OnStateUpdate += objects => { Debug.Log("mainMachine Update!"); };
            mainMachine.OnStateExit += objects => { Debug.Log("mainMachine Exit!"); };

            idleState.OnStateEnter+= objects => { Debug.Log("Idle Enter!"); };
            idleState.OnStateUpdate += objects => { Debug.Log("Idle Update!"); };
            idleState.OnStateExit += objects => { Debug.Log("Idle Exit!"); };

            movementMachine.OnStateEnter += objects => { Debug.Log("movementMachine Enter!"); };
            movementMachine.OnStateUpdate += objects => { Debug.Log("movementMachine Update!"); };
            movementMachine.OnStateExit += objects => { Debug.Log("movementMachine Exit!"); };

            walkState.OnStateEnter += objects => { Debug.Log("walkState Enter!"); };
            walkState.OnStateUpdate += objects => { Debug.Log("walkState Update!"); };
            walkState.OnStateExit += objects => { Debug.Log("walkState Exit!"); };

            runState.OnStateEnter += objects => { Debug.Log("runState Enter!"); };
            runState.OnStateUpdate += objects => { Debug.Log("runState Update!"); };
            runState.OnStateExit += objects => { Debug.Log("runState Exit!"); };

            mainMachine.EnterState(null,null);
        }

      
    }
}
