using System;
using System.Collections.Generic;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Domain.Communication;
using QuantumCryptoCram.Domain.Protocol;

namespace QuantumCryptoCram.Application.Tests.Communication
{
    [TestFixture]
    public class PublicNetworkTest
    {
        [TestCase(ProtocolRoleType.Alice)]
        [TestCase(ProtocolRoleType.Bob)]
        public void Test_PublicNetwork_ReceiversForDifferentMessageTypes(ProtocolRoleType roleType)
        {
            // Arrange
            bool receiver1Received = false;
            bool receiver2Received = false;
            var network = new PublicNetwork();
            object sender = new object();
            object receiver1 = new object();
            object receiver2 = new object();
            var message = new PolarisationMatchesMessage(roleType, new List<int>());

            network.Subscribe<CipherMessage>(receiver1, msg => receiver1Received = true);
            network.Subscribe<PolarisationMatchesMessage>(receiver2, msg => receiver2Received = true);

            // Act
            network.PublishMessage(sender, message);

            // Assert
            receiver1Received.Should().BeFalse();
            receiver2Received.Should().BeTrue();
        }

        [Test]
        public void Test_PublicNetwork_SenderShouldNotReceiveMessage()
        {
            // Arrange
            bool senderReceived = false;
            bool receiverReceived = false;
            var network = new PublicNetwork();
            object sender = new object();
            object receiver = new object();
            PublicMessage message = Substitute.For<PublicMessage>();

            network.Subscribe<PublicMessage>(sender, msg => senderReceived = true);
            network.Subscribe<PublicMessage>(receiver, msg => receiverReceived = true);

            // Act
            network.PublishMessage(sender, message);

            // Assert
            senderReceived.Should().BeFalse();
            receiverReceived.Should().BeTrue();
        }

        [TestCase(ProtocolRoleType.Alice)]
        [TestCase(ProtocolRoleType.Bob)]
        public void Test_PublicNetwork_AllReceiversShouldReceiveMessage(ProtocolRoleType roleType)
        {
            // Arrange
            int receiveCount = 0;
            var network = new PublicNetwork();
            object sender = new object();
            object receiver1 = new object();
            object receiver2 = new object();
            object receiver3 = new object();

            var message = new PolarisationMatchesMessage(roleType, null);

            network.Subscribe<PolarisationMatchesMessage>(receiver1, msg => receiveCount++);
            network.Subscribe<PolarisationMatchesMessage>(receiver2, msg => receiveCount++);
            network.Subscribe<PolarisationMatchesMessage>(receiver3, msg => receiveCount++);

            // Act
            network.PublishMessage(sender, message);

            // Assert
            receiveCount.Should().Be(3);
        }

        [TestCase(ProtocolRoleType.Alice)]
        [TestCase(ProtocolRoleType.Bob)]
        public void Test_PublicNetwork_ShouldNotReceiveAfterUnsubscribing(ProtocolRoleType roleType)
        {
            // Arrange
            bool handlerCalled = false;
            var network = new PublicNetwork();
            object sender = new object();
            object receiver = new object();

            var message = new PolarisationMatchesMessage(roleType, null);

            Action<PolarisationMatchesMessage> messageHandler = _ =>
            {
                handlerCalled = true;
            };

            network.Subscribe(receiver, messageHandler);
            network.Unsubscribe(receiver, messageHandler);

            // Act
            network.PublishMessage(sender, message);

            // Assert
            handlerCalled.Should().BeFalse();
        }

        [Test]
        public void Test_PublicNetwork_ShouldThrowArgumentExcepetion_WhenSubscribedWithNoMessageHandler()
        {
            // Arrange
            var network = new PublicNetwork();
            object receiver = new object();

            // Act
            Action testDelegate = () => network.Subscribe<PolarisationMatchesMessage>(receiver, null);

            // Assert
            testDelegate.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test, Description("Bug #69")]
        public void Test_PublicNetwork_ShouldThrowNoExcepetion_WhenObserverIsSubscribingMultipleTimes()
        {
            // Arrange
            var network = new PublicNetwork();
            object receiver = new object();

            // Act
            Action firstSubscription1 = () => network.Subscribe<PolarisationMatchesMessage>(receiver, msg => { });
            Action firstSubscription2 = () => network.Subscribe<PolarisationMatchesMessage>(receiver, msg => { });

            // Assert
            firstSubscription1.Should().NotThrow<Exception>();
            firstSubscription2.Should().NotThrow<Exception>();
        }
    }
}