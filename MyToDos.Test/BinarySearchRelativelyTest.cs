using System;
using System.Collections.Generic;
using System.Text;
using Storage.QueryMethods;
using Xunit;
using Xunit.Abstractions;

namespace MyToDos.Test
{
    public class BinarySearchRelativelyTest
    {
        private readonly ITestOutputHelper output;
        public BinarySearchRelativelyTest(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        public void IndexOf_Test1(int value)
        {
            List<int> list = new List<int>()
            {
                0,1,2,5,6,8,9,10
            };
            var index = Extensions.BinarySearchRelatively(list, value, (a, b) => a - b);
            output.WriteLine(index.ToString());
            Assert.True((index > 0 && index < list.Count && value >= list[index-1] && value <= list[index]) 
                || (index == 0 && (list.Count == 0 || list[0] >= value))
                || (index == list.Count  && (list.Count == 0 || list[list.Count - 1] <= value)));
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void IndexOf_Test2(int value)
        {
            List<int> list = new List<int>()
            {
                0
            };
            var index = Extensions.BinarySearchRelatively(list, value, (a, b) => a - b);
            Assert.True((index > 0 && index < list.Count && value >= list[index - 1] && value <= list[index])
                || (index == 0 && (list.Count == 0 || list[0] >= value))
                || (index == list.Count && (list.Count == 0 || list[list.Count - 1] <= value)));
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void IndexOf_Test3(int value)
        {
            List<int> list = new List<int>()
            {
                -5,-3,-1,0,0,0,1,3,5,7
            };
            var index = Extensions.BinarySearchRelatively(list, value, (a, b) => a - b);
            Assert.True((index > 0 && index < list.Count && value >= list[index - 1] && value <= list[index])
                || (index == 0 && (list.Count == 0 || list[0] >= value))
                || (index == list.Count && (list.Count == 0 || list[list.Count - 1] <= value)));
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void IndexOf_Test4(int value)
        {
            List<int> list = new List<int>()
            {

            };
            var index = Extensions.BinarySearchRelatively(list, value, (a, b) => a - b);
            Assert.True((index > 0 && index < list.Count && value >= list[index - 1] && value <= list[index])
                || (index == 0 && (list.Count == 0 || list[0] >= value))
                || (index == list.Count && (list.Count == 0 || list[list.Count - 1] <= value)));
        }
    }
}
