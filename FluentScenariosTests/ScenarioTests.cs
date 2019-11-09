using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using FluentScenarios;
using FluentAssertions;
using NSubstitute;
using Xunit.Abstractions;

namespace FluentScenariosTests
{
    public class ExampleData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {new {original = 1, add = 1, expected = 2}};
            yield return new object[] {new {original = 3, add = 4, expected = 7}};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    public class ScenarioTests
    {
        private readonly ITestOutputHelper _output ;

        public ScenarioTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestBuildAndRunScenario()
        {
            var callCount = 0;
            var expectedCallCount = 3;
            var action = Substitute.For<Action>();
            action.When(x => x())
                .Do(x => callCount++);
            new Scenario(_output)
                .Given("this step is executed", action)
                .When("running the scenario", action)
                .Then("I expect mocked function has been executed", action)
                .Run();

            callCount.Should().Be(expectedCallCount, $"the action should have been called {expectedCallCount} times during the scenario execution");
        }

        [Fact]
        public void TestScenarioFails()
        {
            new Scenario(_output)
                .Given("this does nothing", () => {})
                .When("running the scenario", () => {})
                .Then("scenario should fail when assertion fails", () => { true.Should().Be(false); })
                .Invoking(s => s.Run()).Should().Throw<ScenarioFailed>();
        }

        [Fact]
        public void TestGivenWithAndStep()
        {
            var callCount = 0;
            var expectedCallCount = 4;
            var action = Substitute.For<Action>();
            action.When(x => x())
                .Do(x => callCount++);
            new Scenario(_output)
                .Given("this step is executed", action)
                .And("this step is also executed", action)
                .When("running the scenario", action)
                .Then("I expect mocked function has been executed", action)
                .Run();

            callCount.Should().Be(expectedCallCount, $"the action should have been called {expectedCallCount} times during the scenario execution");
        }
        
        [Theory]
        [ClassData(typeof(ExampleData))]
        public void TestScenarioWithExampleDataV3(object dataRow)
        {
            int apples = 0;
            new ScenarioWithExamples(_output, dataRow)
                .Given("I have @original apple(s) in my basket", (values) => { apples = (int)values.original; })
                .Run();

            apples.Should().Be((int) dataRow.GetType().GetProperty("original").GetValue(dataRow, null));
            _output.WriteLine($"{apples}");
        }
    }
}
