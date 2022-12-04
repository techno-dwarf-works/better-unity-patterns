using UnityEngine;

namespace Samples.StateMachineSamples.Models
{
    public class TestJump : TestBase
    {
        public override void Exit()
        {
            Debug.Log($"Exit {typeof(TestJump)}");
        }

        public override void Tick(float tickTime)
        {
            Debug.Log($"Tick {typeof(TestJump)}");
        }

        public override void Enter()
        {
            
            Debug.Log($"Enter {typeof(TestJump)}");
        }
    }
}