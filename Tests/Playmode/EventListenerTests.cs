using NUnit.Framework;
using UnityEngine;

namespace Jroynoel.Tests
{
    public class EventListenerTests
    {
        private EventListener eventListener;

        [SetUp]
        public void SetUp()
		{
			if (eventListener == null)
			{
                eventListener = new GameObject(nameof(EventListenerTests)).AddComponent<EventListener>();
			}
		}

        [TearDown]
        public void TearDown()
		{
			if (eventListener != null)
			{
                Object.DestroyImmediate(eventListener.gameObject);
			}
		}

        [Test]
        [TestCase("Test")]
        [TestCase("Test1")]
        [TestCase("1234")]
        [TestCase("fgdfgdfgf")]
        [TestCase(" ")]
        public void SubscribeTo_SimpleEventNames(string eventName)
        {
            bool eventWasCalled = false;

            eventListener.SubscribeTo(eventName, (obj) =>
            {
                eventWasCalled = true;
            });

            // Call the event
            eventListener.Emit(eventName, null);

            Assert.IsTrue(eventWasCalled);
        }

        [Test]
        [TestCase("Test")]
        [TestCase("Test1")]
        [TestCase("1234")]
        [TestCase("fgdfgdfgf")]
        [TestCase(" ")]
        public void SubscribeTo_MultipleListeners(string eventName)
        {
            bool eventWasCalled1 = false;
            bool eventWasCalled2 = false;

            eventListener.SubscribeTo(eventName, (obj) =>
            {
                eventWasCalled1 = true;
            });

            var eventListener2 = new GameObject().AddComponent<EventListener>();
            eventListener2.SubscribeTo(eventName, (obj) =>
            {
                eventWasCalled2 = true;
            });

            // If only 1 emits the same event name, both should receive
            eventListener2.Emit(eventName, null);

            Assert.IsTrue(eventWasCalled1);
            Assert.IsTrue(eventWasCalled2);
        }

        [Test]
        [TestCase("test")]
        [TestCase('c')]
        [TestCase(1)]
        [TestCase(32.5652f)]
        [TestCase(true)]
        public void SubscribeTo_WithParameters(object arg)
		{
            object receivedParam = null;

            eventListener.SubscribeTo("test", (obj) =>
            {
                receivedParam = obj;
                Debug.Log(receivedParam);
            });

            // Call the event
            eventListener.Emit("test", arg);

            Assert.AreEqual(receivedParam, arg);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void SubscribeTo_InvalidEventName(string eventName)
		{
            Assert.Throws<System.ArgumentException>(() => eventListener.SubscribeTo(eventName, null));
		}

        [Test]
        [TestCase("Test")]
        [TestCase("Test1")]
        [TestCase("1234")]
        [TestCase("fgdfgdfgf")]
        [TestCase(" ")]
        public void UnsubscribeFrom(string eventName)
        {
            bool eventWasCalled = false;

            eventListener.SubscribeTo(eventName, (obj) =>
            {
                eventWasCalled = true;
            });

            eventListener.UnsubscribeFrom(eventName);

            // Call the event
            eventListener.Emit(eventName, null);

            Assert.IsFalse(eventWasCalled);
        }

        [Test]
        [TestCase("test")]
        [TestCase('c')]
        [TestCase(1)]
        [TestCase(32.5652f)]
        [TestCase(true)]
        public void GetLastEvent_SyncLastEvent(object arg)
		{
            bool eventWasCalled = false;
            object received = null;
            eventListener.SyncLastEvent = true;

            // Emit the event before subscribing
            eventListener.Emit("test", arg);

            // Should still trigger with Sync
            eventListener.SubscribeTo("test", (obj) =>
            {
                eventWasCalled = true;
                received = obj;
            });

            Assert.IsTrue(eventWasCalled);
            Assert.AreEqual(arg, received);
        }

        [Test]
        [TestCase("test")]
        [TestCase('c')]
        [TestCase(1)]
        [TestCase(32.5652f)]
        [TestCase(true)]
        public void GetLastEvent_Unsynced(object arg)
        {
            bool eventWasCalled = false;
            object received = null;
            eventListener.SyncLastEvent = false;

            // Emit the event before subscribing
            eventListener.Emit("test");

            eventListener.SubscribeTo("test", (obj) =>
            {
                eventWasCalled = true;
                received = obj;
            });

            Assert.IsFalse(eventWasCalled);
            Assert.IsNull(received);
        }
    }
}
