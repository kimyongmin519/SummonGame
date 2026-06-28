using Agents;

namespace Player.FSM.States
{
    public abstract class AbstractPlayerState
    {
        protected Player _player;
        protected IMover _mover;
        protected IRenderer _renderer;
        protected const float INPUT_DEADLINE = 0.1f;
        protected readonly int _stateClipHash;

        public AbstractPlayerState(Player player, int clipHash)
        {
            _player = player;
            _stateClipHash = clipHash;
            _mover = player.GetModule<IMover>();
            _renderer = player.GetModule<IRenderer>();
        }

        public virtual void Enter(float transition = 0.2f, int layerIndex = 0)
        {
            _renderer.PlayClip(_stateClipHash, transition, layerIndex);
        }
        
        public virtual void Update() {}

        public virtual void Exit() {}
    }
}