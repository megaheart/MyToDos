using System;
using System.Collections.Generic;
using System.Text;
//using MyToDos.View.CustomizedControls;
using Xunit;

namespace MyToDos.Test
{
    public class NumbericTextBoxTest
    {
        [Theory]
        [InlineData(3,26,3050,5,7)]
        //[InlineData(1, 2, 1, 2, 1)]
        public void SafeAdd_Test(int min, int max, int value, int i, int trueResult)
        {
            Assert.Equal(trueResult, SafeAdd(min, max, value, i));
        }
        public static int SafeAdd(int min, int max, int value, int i)
        {
            int length = max - min + 1;
            return ((value - min + i) % length + length) % length + min;
        }
    }
}
