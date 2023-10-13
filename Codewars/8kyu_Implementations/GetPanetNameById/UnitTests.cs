namespace _8kyu_Implementations.GetPanetNameById
{
    using NUnit.Framework;

    [TestFixture]
    public class SolutionTest
    {
        [Test]
        public void Test()
        {
            Assert.AreEqual("Venus", Kata.GetPlanetName(2));
            Assert.AreEqual("Jupiter", Kata.GetPlanetName(5));
        }
    }
}
