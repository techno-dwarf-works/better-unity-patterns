namespace Better.UnityPatterns.Runtime.StateMachine.States
{
    public abstract class BaseState
    {
        public abstract void Enter();
        public abstract void Tick(float tickTime);

        public abstract void Exit();
    }
}