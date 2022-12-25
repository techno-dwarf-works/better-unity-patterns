using UnityEngine;

namespace Samples.StateMachineSamples.Models
{
    public class TestMove : TestBase
    {
        public override void Exit()
        {
            Debug.Log($"Exit {typeof(TestMove)}");
        }

        public override void Tick(float tickTime)
        {
            Debug.Log($"Tick {typeof(TestMove)}");
        }

        public override void Enter()
        {
            
            Debug.Log($"Enter {typeof(TestMove)}");
        }
    }
}