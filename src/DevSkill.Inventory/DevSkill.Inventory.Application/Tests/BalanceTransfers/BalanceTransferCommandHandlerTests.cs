using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Command;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Handler;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Inventory.Application.Tests.BalanceTransfers
{
    [ExcludeFromCodeCoverage]
    public class BalanceTransferCommandHandlerTests
    {
        private AutoMock _mock;
        private BalanceTransferCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;

        [OneTimeSetUp]
        public void GlobalSetup() => _mock = AutoMock.GetLoose();

        [OneTimeTearDown]
        public void GlobalTeardown() => _mock?.Dispose();

        [SetUp]
        public void TestSetup()
        {
            _mock = AutoMock.GetLoose();
            _unitOfWorkMock = _mock.Mock<IApplicationUnitOfWork>();
            _handler = new BalanceTransferCommandHandler(_unitOfWorkMock.Object);
        }

        [TearDown]
        public void TestTeardown() => _mock?.Dispose();

        [Test]
        public async Task Handle_ShouldTransferBalance_WhenAccountsAreValid()
        {
            var fromAccountId = Guid.NewGuid();
            var toAccountId = Guid.NewGuid();

            var fromAccount = new CashAccount { CurrentBalance = 2000 };
            var toAccount = new CashAccount { CurrentBalance = 1000 };

            var cashRepoMock = new Mock<ICashAccountRepository>();
            cashRepoMock.Setup(r => r.GetByIdAsync(fromAccountId)).ReturnsAsync(fromAccount);
            cashRepoMock.Setup(r => r.GetByIdAsync(toAccountId)).ReturnsAsync(toAccount);

            var transferRepoMock = new Mock<IBalanceTransferRepository>();
            transferRepoMock.Setup(r => r.AddAsync(It.IsAny<BalanceTransfer>())).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.CashAccountRepository).Returns(cashRepoMock.Object);
            _unitOfWorkMock.Setup(x => x.BalanceTransferRepository).Returns(transferRepoMock.Object);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

            var command = new AddBalanceTransferCommand
            {
                FromAccountId = fromAccountId,
                FromAccountType = "Cash",
                ToAccountId = toAccountId,
                ToAccountType = "Cash",
                Amount = 500,
                Note = "Test Transfer"
            };

            var result = await _handler.Handle(command, CancellationToken.None);


            result.ShouldBeTrue();
            fromAccount.CurrentBalance.ShouldBe(1500);
            toAccount.CurrentBalance.ShouldBe(1500);

            _unitOfWorkMock.Verify(u => u.BalanceTransferRepository.AddAsync(It.IsAny<BalanceTransfer>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldThrowException_WhenInsufficientBalance()
        {
            var fromAccountId = Guid.NewGuid();
            var toAccountId = Guid.NewGuid();

            var fromAccount = new CashAccount { CurrentBalance = 50 }; 
            var toAccount = new BankAccount { CurrentBalance = 1000 };

            var cashRepoMock = new Mock<ICashAccountRepository>();
            var bankRepoMock = new Mock<IBankAccountRepository>();

            cashRepoMock.Setup(r => r.GetByIdAsync(fromAccountId)).ReturnsAsync(fromAccount);
            bankRepoMock.Setup(r => r.GetByIdAsync(toAccountId)).ReturnsAsync(toAccount);

            _unitOfWorkMock.Setup(x => x.CashAccountRepository).Returns(cashRepoMock.Object);
            _unitOfWorkMock.Setup(x => x.BankAccountRepository).Returns(bankRepoMock.Object);

            var command = new AddBalanceTransferCommand
            {
                FromAccountId = fromAccountId,
                FromAccountType = "Cash",
                ToAccountId = toAccountId,
                ToAccountType = "Bank",
                Amount = 100,
                Note = "Insufficient balance case"
            };
            var exception = await Should.ThrowAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            exception.Message.ShouldBe("Insufficient balance");
        }
    }
}
