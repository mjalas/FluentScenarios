using System;
using FluentAssertions;
using FluentScenarios;
using Xunit;

namespace FluentScenariosTests
{
    public class TableTests
    {
        [Fact]
        public void TestCreateTable()
        {
            var table = new Table()
                .Headers("Fruit", "Price")
                .Row("Apple", 1)
                .Row("Orange", 2)
                .Row("Kiwi", 1);

            (table.Rows[0].Fruit as string).Should().Be("Apple");
            (table.Rows[1].Fruit as string).Should().Be("Orange");
            ((int)table.Rows[1].Price).Should().Be(2);
        }

        [Fact]
        public void TestAddNotEnoughItemsToARow()
        {
            Action action = () => new Table()
                .Headers("Fruit", "Price")
                .Row("Apple");

            action.Should().Throw<RowItemCountMissMatch>()
                .WithMessage("Not enough items in the row!");

        }
        
        [Fact]
        public void TestAddTooManyItemsToARow()
        {
            Action action = () => new Table()
                .Headers("Fruit", "Price")
                .Row("Apple", 1, 2);

            action.Should().Throw<RowItemCountMissMatch>()
                .WithMessage("Too many items in the row!");

        }
    }
}