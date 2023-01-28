using System;
using LiveClinic.Shared.Common;
using NUnit.Framework;

namespace LiveClinic.Shared.Tests.Custom
{
    [TestFixture]
    public class LiveUtilsTests
    {
        [Test]
        public void should_GenerateId()
        {
            long testId = LiveUtils.GenerateId();
            Assert.That(testId,Is.GreaterThan(0));
            Console.WriteLine(testId);
        }
        
        [TestCase(8009)]
        [TestCase(1234)]
        public void should_GenerateMemberNo(long id)
        {
            var memberNo=id.GenerateMemberNo();
            Assert.That(memberNo,Is.EqualTo($"KE-UHC-{id}"));
            Console.WriteLine(memberNo);
        }
    }
}