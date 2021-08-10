using NUnit.Framework;
using UnityEngine;

namespace Jroynoel.Tests
{
    public class SingletonTests
    {
        class SingletonTest : Singleton<SingletonTest> { }

		[Test]
        public void AlreadyExisting()
		{
            string name = "test";

            new GameObject(name).AddComponent<SingletonTest>();

            string instanceName = SingletonTest.Instance.name;
            string foundName = Object.FindObjectOfType<SingletonTest>().name;

            Assert.AreEqual(name, instanceName);
            Assert.AreEqual(name, foundName);
		}
    }
}
