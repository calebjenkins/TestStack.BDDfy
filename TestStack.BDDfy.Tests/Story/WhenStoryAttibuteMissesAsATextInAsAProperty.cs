using NUnit.Framework;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Tests.Story
{
    [TestFixture]
    [Story(
        AsA = "programmer",
        IWant = "I want the missing 'As a' to be added to story metadata",
        SoThat = "So that I don't have to duplicate it on the string")]
    public class WhenStoryAttibuteMissesAsATextInAsAProperty
    {
        [Test]
        public void Then_it_is_injected_by_BDDfy()
        {
            var story = new DummyScenario().BDDfy<WhenStoryAttibuteMissesAsATextInAsAProperty>();

            Assert.That(story.MetaData.AsA, Is.EqualTo("As a programmer"));
            Assert.That(story.MetaData.IWant, Is.EqualTo("I want the missing 'As a' to be added to story metadata"));
            Assert.That(story.MetaData.SoThat, Is.EqualTo("So that I don't have to duplicate it on the string"));
        }
    }
}