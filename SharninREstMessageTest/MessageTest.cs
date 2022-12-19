using TestToolsToXunitProxy;
using SharpClient;

namespace SharpClient.Test
{
    [TestClass()]
    public class MessageTests
    {
        [TestMethod()]
        public void MessageTest()
        {
            var to = MessageRecipients.MR_BROKER;
            int from = 101;
            var ms = new Message(to, (MessageRecipients)from);
            Assert.AreEqual(MessageTypes.MT_DATA, ms.GetAction());
            Assert.AreEqual("", ms.GetData());
        }

        [TestMethod()]
        public void GetActionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetDataTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetFromTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void sendTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void sendTest1()
        {
            Assert.Fail();
        }
    }
}