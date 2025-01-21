using Application.Interfaces.Services.Email;
using Application.Services.Email;
using Domain.Entities;
using Moq;

namespace Tests.Application.ServicesTests.EmailTests
{
    public class SendMailT
    {
        private readonly User nUserReceiver = new()
        {
            Pseudo = "testUserR",
            EmailGoogle = "testr@test.com",
        };
        private readonly User nUserSender = new()
        {
            Pseudo = "testUserS",
            EmailGoogle = "tests@test.com",
        };

        private readonly Mock<IConfigEmail> mockEmailSender = new();
        private readonly Mock<IBodyEmail> mockBodyEmail = new();

        [Fact]
        public async Task MailRegisterAsync_ShouldCallConfigMailWithCorrectParameters()
        {
            var email = "test@test.com";
            var pseudo = "testUser";
            var sendMail = new SendMail(mockEmailSender.Object, mockBodyEmail.Object);

            await sendMail.MailRegisterAsync(email, pseudo);

            mockEmailSender.Verify(sender => sender.configMail(
                        email,
                        mockBodyEmail.Object.subjectRegister,
                        mockBodyEmail.Object._emailBodyRegister(pseudo)),
                        Times.Once);
        }

        [Fact]
        public async Task RequestFriendMailAsync_ShouldCallConfigMailWithCorrectParameters()
        {
            var sendMail = new SendMail(mockEmailSender.Object, mockBodyEmail.Object);

            await sendMail.RequestFriendMailAsync(nUserReceiver, nUserSender);

            mockEmailSender.Verify(sender => sender.configMail(
                        nUserReceiver.EmailGoogle,
                        mockBodyEmail.Object.subjectRequestFriend,
                        mockBodyEmail.Object._requestFriendBodyEmail(nUserReceiver.Pseudo, nUserSender)),
                        Times.Once);
        }

        [Fact]
        public async Task AcceptRequestFriendMailAsync_ShouldCallConfigMailWithCorrectParameters()
        {
            var sendMail = new SendMail(mockEmailSender.Object, mockBodyEmail.Object);

            await sendMail.AcceptRequestFriendMailAsync(nUserReceiver, nUserSender);

            mockEmailSender.Verify(sender => sender.configMail(
                        nUserReceiver.EmailGoogle,
                        nUserSender.Pseudo + mockBodyEmail.Object.subjectAcceptFriend,
                        mockBodyEmail.Object._acceptFriendBodyEmail(nUserReceiver.Pseudo, nUserSender)),
                        Times.Once);
        }
    }
}