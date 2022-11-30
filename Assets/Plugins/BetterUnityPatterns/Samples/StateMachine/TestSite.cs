using Better.UnityPatterns.Runtime.StateMachine.States;
using Samples.StateMachine;

namespace Better.UnityPatterns.Runtime.StateMachine
{
    public abstract class TestBase : BaseState
    {
    
    }

    public class TestMove : TestBase
    {
        public override void Exit()
        {
            
        }

        public override void Tick(float tickTime)
        {
            
        }

        public override void Enter()
        {
            
        }
    }
    
    public class TestJump : TestBase
    {
        public override void Exit()
        {
            
        }

        public override void Tick(float tickTime)
        {
            
        }

        public override void Enter()
        {
            
        }
    }
    
    public class TestSite
    {
        private StateMachine<TestBase, TestHandler<TestBase>> _machine = new StateMachine<TestBase, TestHandler<TestBase>>();

        public void Test()
        {
            var testMove = new TestMove();
            var testJump = new TestJump();
            _machine.AddTransition(testMove, testJump, () => true);
            _machine.EventHandler.RegisterJumpState(testJump);
            _machine.EventHandler.TryJump();
        }
    }
}