using System;
using System.Linq;
using Xunit;
using FsCheck.Xunit;
using FsCheck;

namespace HackerHands.CSharp
{
    public class LeftRotation
    {
        static int[] rotLeft(int[] a, int d)
        {
            var result = Enumerable.Range(0, int.MaxValue).SelectMany(x => a).Skip(d).Take(a.Length).ToArray();
            return result;
        }


        [Theory]
        [InlineData(new object[] { new[] { 1, 2, 3, 4, 5 }, 2, new[] { 3, 4, 5, 1, 2} })]
        [InlineData(new object[] { new[] { 1, 2, 3, 4, 5 }, 4, new[] { 5, 1, 2, 3, 4} })]
        public void VerifyRotation(int[] a, int d, int[] expected)
        {
            var actual = rotLeft(a, d);
            Assert.Equal(expected, actual);
        }


        [LeftRotationProperty]
        public Property RotatedArrayHasSameLengthAsOriginal((int[] a, int d) input)
        {
            var actual = rotLeft(input.a, input.d);
            return (actual.Length == input.a.Length).ToProperty();
        }

        [LeftRotationProperty]
        public Property RotatedArrayContainsSameNumbersAsOriginal((int[] a, int d) input)
            => rotLeft(input.a, input.d).All(x => input.a.Contains(x)).ToProperty();

        [LeftRotationProperty]
        public Property RotatedArraySumsEqualToOriginalSum((int[] a, int d) input)
            => (rotLeft(input.a, input.d).Sum() == input.a.Sum()).ToProperty();


        [LeftRotationProperty]
        public Property WhenRotatingCountEqualsToLengthSameArrayIsReturned(int[] a)
            => rotLeft(a, a.Length).Select((x, i) => a[i] == x).All(x=> x).ToProperty();

        
        [LeftRotationProperty]
        public Property AfterRotationIndexPlusDEqualsCurrent((int[] a, int d) input)
        {
            var a = input.a;
            var d = input.d;
            bool Verify()
            {
                var actual = rotLeft(a, d);
                for (int i = 0; i < a.Length; i++)
                {
                    var nextIndex = i + d;
                    if (nextIndex >= a.Length)
                        nextIndex -= a.Length;

                    if (actual[i] != a[nextIndex])
                        return false;
                }

                return true;
            }

            return new Func<bool>(Verify).When(d <= a.Length);
        }




        class LeftRotationProperty : PropertyAttribute
        {
            public LeftRotationProperty()
            {
                Arbitrary = new[] {typeof(LeftRotationGenerator)};
            }
        }

        private static class LeftRotationGenerator
        {

            public static Arbitrary<int[]> A()
            {
                var len = Gen.Choose(1, 100);
                var aValue = Gen.Choose(1, 1000);
                var a = len.SelectMany(l => Gen.ArrayOf(l, aValue));
                return a.ToArbitrary();
            }

            public static Arbitrary<(int[], int d)> Input()
            {
                var a = A().Generator;
                var d = a.SelectMany(av => Gen.Choose(1, av.Length));
                var result = a.SelectMany(ax => d.Select(dx => (ax, dx)));
                return result.ToArbitrary();
            }
        }

    }
}