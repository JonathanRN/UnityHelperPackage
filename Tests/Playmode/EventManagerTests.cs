using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace Jroynoel.Tests
{
    public class EventManagerTests
    {
        [Test]
        public void SubscribeTo_Sender_IsValid()
        {
            string eventName = "event";
            object sender = new object();

            object receiver = new object();
            receiver.SubscribeTo(eventName, (s, a) =>
            {
                Assert.AreEqual(sender, s);
                Assert.IsNull(a);
            });

            sender.Emit(eventName);

            receiver.UnsubscribeFrom(eventName);
        }

        [TestCase("string")]
        [TestCase(1)]
        [TestCase('c')]
        [TestCase(3.1416)]
        [TestCase(3.1416f)]
        [TestCase(true)]
        [TestCase(null)]
        public void SubscribeTo_Args_AreValid(object args)
        {
            string eventName = "event";
            object sender = new object();

            object receiver = new object();
            receiver.SubscribeTo(eventName, (s, a) =>
            {
                Assert.AreEqual(args, a);
            });

            sender.Emit(eventName, args);

            receiver.UnsubscribeFrom(eventName);
        }

        [Test]
        public void UnsubscribeFrom_ExistingEvent_IsValid()
		{
            string eventName = "event";
            object sender = new object();

            object receiver1 = new object();
            receiver1.SubscribeTo(eventName, (s, a) =>
            {
                UnityEngine.Debug.LogError("Action should not have been called");
            });

            object receiver2 = new object();
            receiver2.SubscribeTo(eventName, (s, a) =>
            {
                Assert.AreEqual(sender, s);
            });

            receiver1.UnsubscribeFrom(eventName);

            sender.Emit(eventName);

            receiver2.UnsubscribeFrom(eventName);
        }

        [Test]
        public void UnsubscribeFrom_NonExistingEvent_Throws()
        {
            string eventName = "event";
            object receiver = new object();

            Assert.Throws<System.NullReferenceException>(() => receiver.UnsubscribeFrom(eventName));
        }

        [Test]
        public void Emit_NonExistingEvent_Throws()
        {
            string eventName = "event";
            object sender = new object();

            Assert.Throws<System.NullReferenceException>(() => sender.Emit(eventName));
        }

        [UnityTest]
        public IEnumerator WaitForEvent()
		{
            string eventName = "event";
            object sender = new object();

            bool done = false;
            System.Threading.Tasks.Task.Run(async () =>
            {
                await System.Threading.Tasks.Task.Delay(1000);
                done = true;
                sender.Emit(eventName);
            });

            object receiver = new object();
            yield return receiver.WaitForEvent(eventName);
            Assert.IsTrue(done);
        }
    }
}
