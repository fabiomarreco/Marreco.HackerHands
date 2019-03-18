using System;
using System.Linq;
using Xunit;

namespace HackerHands.CSharp
{

    public class UnitTest1
    {
        public static int CountValleys (string str)
        {
            var result = 
                str
                .Select(x=> 
                {
                    switch (x)
                    {
                        case 'U' : return 1;
                        case 'D' : return -1;
                        default: throw new InvalidOperationException("letra nao é valida");
                    }
                })
                .Aggregate(new { Lvl = 0, Valleys = 0}, (acc, n) => (((acc.Lvl + n) == 0) && (acc.Lvl < 0))? new { Lvl = 0, Valleys = acc.Valleys + 1 } 
                : new { Lvl = acc.Lvl + n, Valleys = acc.Valleys });
            return result.Valleys;
        }


        [Theory]
        [InlineData("DDUUUUDD", 1)]
        [InlineData("UDDDUDUU", 1)]
        [InlineData("DUDUDUUUDDUDDDUU", 4)]
        public void NumeroDeValesIgualEsperado(string path, int expected)
        {
            var actual = CountValleys(path);
            Assert.Equal(expected, actual);
        }
    }
}
