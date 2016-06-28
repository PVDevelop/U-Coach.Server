using NUnit.Framework;
using PVDevelop.UCoach.Server.Core.Domain;
using PVDevelop.UCoach.Server.Core.Domain.Exceptions;
using System;

namespace Core.Domain.Tests
{
    [TestFixture]
    public class CoreUserTests
    {
        [Test]
        public void Confirm_ValidKeyAndWaitingForConfirm_SetsConfirmed()
        {
            var key = Guid.NewGuid().ToString();
            var user = CoreUserFactory.CreateUCoachUser("1", key);
            user.Confirm(key);

            Assert.AreEqual(CoreUserState.Confirmed, user.State);
        }

        [Test]
        public void Confirm_InvalidKeyAndWaitingForConfirm_ThrowsException()
        {
            var key = Guid.NewGuid().ToString();
            var user = CoreUserFactory.CreateUCoachUser("2", key);
            Assert.Throws<InvalidConfirmationKeyException>(() => user.Confirm(Guid.NewGuid().ToString()));
        }
    }
}
