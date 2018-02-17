using System;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;

namespace MemoryProfiling
{
    public class PlayTests
    {
        [Test]
        public void GivenNoInstanceCreated_ThenNoInstanceInMemory()
        {
            dotMemory.Check(memory =>
            {
                Assert.That(GetCount<Play>(memory), Is.EqualTo(0));
            });
        }

        [Test]
        public void GivenOneInstanceCreatedAndReferenced_ThenOneInMemory()
        {
            var play = new Play();

            dotMemory.Check(memory =>
            {
                Assert.That(GetCount<Play>(memory), Is.EqualTo(1));
            });
        }

        [Test]
        public void GivenOneInstanceCreatedAndCollected_ThenNoInstanceInMemory()
        {
            //For debuggable code, JIT extends lifetime for every variable to end of the function.
            CreatePlay();

            GC.Collect();

            dotMemory.Check(memory =>
            {
                Assert.That(GetCount<Play>(memory), Is.EqualTo(0));
            });
        }

        private static void CreatePlay()
        {
            new Play();
        }

        private static int GetCount<T>(Memory memory)
        {
            return memory
                .GetObjects(where => where.Type.Is<T>())
                .ObjectsCount;
        }
    }
}
