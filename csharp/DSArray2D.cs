using System;
using System.Linq;
using Xunit;

namespace HackerHands.CSharp
{

    public class DSArray2DTest 
    {
        public static int GetHourGlassSum(int hi, int hj, int[][] arr)
        {
            var fst = Enumerable.Range(0, 3).Select (x => arr[hi][hj + x]).Sum();
            var snd = arr[hi+1][hj+1];
            var trd = Enumerable.Range(0, 3).Select (x => arr[hi + 2][hj + x]).Sum();
            return fst + snd + trd;
        }
        public static int hourglassSum(int[][] arr) 
        {
            int maxVal = int.MinValue;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var current= GetHourGlassSum(i, j, arr);
                    Console.WriteLine("i: {0}, j: {1}, sum:{2}", i, j, current);
                    maxVal = Math.Max (maxVal, current);
                }
            }
            return maxVal;
        }



        [Theory]
        [InlineData(@"
-9 -9 -9  1 1 1 
 0 -9  0  4 3 2
-9 -9 -9  1 2 3
 0  0  8  6 6 0
 0  0  0 -2 0 0
 0  0  1  2 4 0
        ", 28)]        
        [InlineData(@"
1 1 1 0 0 0
0 1 0 0 0 0
1 1 1 0 0 0
0 0 2 4 4 0
0 0 0 2 0 0
0 0 1 2 4 0
        ", 19)]
        public void VerificaSomaHourGlassFixa(string strArr, int expected)
        {
            var arr = strArr.Trim().Split('\n').Select((s, i)=> s.Trim().Split(' ').Where(x=> !string.IsNullOrWhiteSpace(x)).Select(x=> int.Parse(x)).ToArray()).ToArray();
            var actual = hourglassSum(arr);
            Assert.Equal(expected, actual);
        }
    }

}