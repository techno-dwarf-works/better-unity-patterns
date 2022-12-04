using Better.UnityPatterns.Runtime.StateMachine;
using Samples.StateMachineSamples.Models;
using UnityEngine;

namespace Samples.StateMachineSamples
{
    public class StateMachineTester : MonoBehaviour
    {
        private StateMachine<TestBase, TestHandler<TestBase>> _machine =
            new StateMachine<TestBase, TestHandler<TestBase>>();

        private void Start()
        {
            var testMove = new TestMove();
            var testJump = new TestJump();
            _machine.AddTransition(testMove, testJump, () => Time.time < 15f);
            _machine.AddTransition(testJump, testMove, () => Time.time > 15f);
            _machine.ChangeState(testMove);
        }

        // Update is called once per frame
        private void Update()
        {
            _machine.Tick(Time.deltaTime);
        }
    }
}