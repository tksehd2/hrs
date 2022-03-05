using Hrs.Extensions;
using NUnit.Framework;

namespace Hrs.Test.Editor
{
    /// <summary>
    /// ArrayExtensionクラスをテスト
    /// </summary>
    public class ArrayExtensionTest
    {
        ///<summary>テストで使う配列</summary>
        private int[] _testArray;

        /// <summary>
        /// テストを初期化、テストごとに実行される
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _testArray = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        }

        /// <summary>
        /// GetOrDefaultのIndexをテスト
        /// </summary>
        [Test]
        public void GetOrDefault_IndexTest()
        {
            var expected = 3;
            var result = _testArray.GetOrDefault(2);

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// GetOrDefaultのデフォルト値をテスト
        /// </summary>
        [Test]
        public void GetOrDefault_DefaultValueTest()
        {
            var expected = 100;
            var result = _testArray.GetOrDefault(-999, expected);

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// FirstOrDefaultのIndexテスト
        /// </summary>
        [Test]
        public void FirstOrDefault_IndexTest()
        {
            var expected = 1;
            var result = _testArray.FirstOrDefault();

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// FirstOrDefaultのデファクト値テスト
        /// </summary>
        [Test]
        public void FirstOrDefault_DefaultValueTest()
        {
            var expected = 100;
            _testArray = null;
            var result = _testArray.FirstOrDefault(expected);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// LastOrDefaultのIndexテスト
        /// </summary>
        [Test]
        public void LastOrDefault_IndexTest()
        {
            var expected = 10;
            var result = _testArray.LastOrDefault();
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// LastOrDefaultのデフォルト値テスト
        /// </summary>
        [Test]
        public void LastOrDefault_DefaultValueTest()
        {
            var expected = 100;
            _testArray = null;
            var result = _testArray.LastOrDefault(expected);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// ForEachテスト
        /// </summary>
        [Test]
        public void ForEachTest()
        {
            var expected = 55;
            var result = 0;
            _testArray.ForEach(val => result += val);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// ForEachIndexedテスト
        /// </summary>
        [Test]
        public void ForEachIndexedTest()
        {
            var expected = 9;
            var result = 0;

            _testArray.ForEachIndexed((val, idx) => result = idx);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// FindIndexテスト
        /// </summary>
        [Test]
        public void FindIndexTest()
        {
            for (var expected = 0; expected < _testArray.Length; ++expected)
            {
                var result = _testArray.FindIndex(_testArray[expected]);
                Assert.AreEqual(expected, result);
            }
        }
    }
}
