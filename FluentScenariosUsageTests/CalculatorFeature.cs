using FluentAssertions;
using FluentScenarios;
using Xunit;
using Xunit.Abstractions;

namespace FluentScenariosUsageTests
{
    public class CalculatorFeature
    {
        private readonly ITestOutputHelper _output;

        public CalculatorFeature(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [ExamplesScenario]
        [InlineData(1, 1, 2)]
        [InlineData(4, 6, 10)]
        public void AddTwoIntegersScenario(int first, int second, int expected)
        {
            var data = new {first = first, second = second, expected = expected};

            int[] integers = new int[2];
            int result = 0;
            new ScenarioWithExamples(_output, data)
                .Given("I have integer @first", values => integers[0] = (int) values.first)
                .And("I have integer @second", values => integers[1] = (int) values.second)
                .When("adding them together", values => result = integers[0] + integers[1])
                .Then("the sum should be @expected", values =>
                {
                    result.Should().Be((int) values.expected);
                })
                .Run();
        }
    }
}