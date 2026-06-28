using System;
using System.Collections.Generic;
using Agents;
using Player.FSM.States;
using UnityEngine;

namespace Player.FSM
{
    public class PlayerStateMachine
    {
        public AbstractPlayerState CurrentState { get; private set; }
        private Dictionary<int, AbstractPlayerState> _stateDict;

        public PlayerStateMachine(Agent agent, PlayerStateSO[] stateList)
        {
            _stateDict = new Dictionary<int, AbstractPlayerState>();
            foreach (PlayerStateSO stateData in stateList)
            {
                Type type = Type.GetType(stateData.className);
                Debug.Assert(type != null, $"찾고자 하는 타입이 없습니다. : {stateData.className}");
                int paramHash = stateData.stateParam != null ? stateData.stateParam.ParamHash : 0;
                AbstractPlayerState state = (AbstractPlayerState)Activator.CreateInstance(type, agent,  paramHash);
                _stateDict.Add(stateData.stateIndex, state);
            }
        }

        public void ChangeState(int newStateIndex)
        {
            CurrentState?.Exit();
            AbstractPlayerState newState = _stateDict.GetValueOrDefault(newStateIndex);
            Debug.Assert(newState != null, $"new State is null {newStateIndex}");
            
            CurrentState = newState;
            CurrentState.Enter();
        }
        
        public void UpdateMachine() => CurrentState?.Update();

        public AbstractPlayerState GetCurrentState() => CurrentState;
        public AbstractPlayerState GetState(int stateIndex)
        {
            return _stateDict.GetValueOrDefault(stateIndex);
        }
    }
}