using NUnit.Framework;
using PVDevelop.UCoach.Server.Core.Domain;
using PVDevelop.UCoach.Server.Core.Domain.Exceptions;
using System;

namespace Core.Domain.Tests
{
    [TestFixture]
    public class SportsmanConfirmationTests
    {
        [Test]
        public void Confirm_ValidKeyAndWaitingForConfirm_SetsConfirmed()
        {
            var key = Guid.NewGuid().ToString();
            var user = SportsmanConfirmationFactory.CreateSportsmanConfirmation("1", key);
            user.Confirm(key);

            Assert.AreEqual(SportsmanConfirmationState.Confirmed, user.State);
        }

        [Test]
        public void Confirm_InvalidKeyAndWaitingForConfirm_ThrowsException()
        {
            var key = Guid.NewGuid().ToString();
            var user = SportsmanConfirmationFactory.CreateSportsmanConfirmation("2", key);
            Assert.Throws<InvalidConfirmationKeyException>(() => user.Confirm(Guid.NewGuid().ToString()));
        }
    }
}
