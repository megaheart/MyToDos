using Storage.QueryMethods;
using System.Collections.Generic;
using Xunit;

namespace MyToDos.Test
{
    public class BinarySearchTest
    {
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
            //Assert.Equal(list.IndexOf(value), Extensions.BinarySearch(list, value, (a, b) => a - b));
            //Assert.Equal(list.FindIndex(i => i.ID == id), index);
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
            //Assert.Equal(list.IndexOf(value), Extensions.BinarySearch(list, value, (a, b) => a - b));
            //Assert.Equal(list.FindIndex(i => i.ID == id), index);
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void IndexOf_Test3(int value)
        {
            List<int> list = new List<int>()
            {
                -5,-3,-1/*,0,0*/,0,1,3,5,7
            };
            //Assert.Equal(list.IndexOf(value), Extensions.BinarySearch(list, value, (a, b) => a - b));
            //Assert.Equal(list.FindIndex(i => i.ID == id), index);
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
            //Assert.Equal(list.IndexOf(value), Extensions.BinarySearch(list, value, (a, b) => a - b));
            //Assert.Equal(list.FindIndex(i => i.ID == id), index);
        }
    }
}
