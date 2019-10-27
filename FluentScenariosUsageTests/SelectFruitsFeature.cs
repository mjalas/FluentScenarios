using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using FluentScenarios;
using Xunit.Abstractions;

namespace FluentScenariosUsageTests
{
    public class SelectFruitsFeature
    {
        private readonly ITestOutputHelper _output;

        public SelectFruitsFeature(ITestOutputHelper output)
        {
            _output = output;
        }

        [Scenario]
        public void SelectObjectFromListScenario()
        {
            var storedFruits = new List<Fruit>();
            Fruit selectedFruit = null;
            var expectedFruitName = "Apple";
            
            new Scenario(_output)
                .Given("I have the following fruits",
                    new Table()
                        .Headers("Name")
                        .Row("Apple")
                        .Row("Orange")
                        .Row("Kiwi"),
                    (table) =>
                    {
                        foreach (var fruit in table.Rows)
                        {
                            storedFruits.Add(new Fruit { Name = fruit.Name });
                        }
                    })
                .When("selecting the Apple", () =>
                {
                    foreach (var fruit in storedFruits)
                    {
                        if (fruit.Name == "Apple")
                        {
                            selectedFruit = fruit;
                            break;
                        }
                    }
                })
                .Then("I should have the Apple selected", () => { selectedFruit.Name.Should().Be(expectedFruitName); })
                .Run();
        }
        
        
        private class Fruit
        {
            public string Name { get; set; }
        }
    }

    
}