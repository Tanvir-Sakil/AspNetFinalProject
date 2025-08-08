using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Command;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Handler;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Inventory.Application.Tests.BalanceTransfers
{
    [ExcludeFromCodeCoverage]
    public class GetAllBalanceTransfersQueryHandlerTests
    {
        private AutoMock _moq;
        private GetAllBalanceTransfersQueryHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;

        [OneTimeSetUp]
        public void OneTimeSetup() => _moq = AutoMock.GetLoose();

        [OneTimeTearDown]
        public void OneTimeTearDown() => _moq?.Dispose();

        [SetUp]
        public void Setup()
        {
            _handler = _moq.Create<GetAllBalanceTransfersQueryHandler>();
            _unitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        }

        [TearDown]
        public void Teardown() => _unitOfWorkMock?.Reset();

        [Test]
        public async Task Handle_ShouldReturnTransfersWithAccountNames()
        {
            var fromAccountId = Guid.Parse("EF1FA70B-558F-4FF5-8E89-4166C4C18F04");
            var toAccountId = Guid.Parse("53E6D6C1-9D67-4BAA-889E-898CFA372EF6");

            var transfers = new List<BalanceTransfer>
    {
        new BalanceTransfer
        {
            Id = Guid.NewGuid(),
            FromAccountId = fromAccountId,
            ToAccountId = toAccountId,
            FromAccountType = "Cash",
            ToAccountType = "Bank",
            Amount = 250,
            TransferDate = DateTime.UtcNow
        }
    };

            _unitOfWorkMock.Setup(x => x.BalanceTransferRepository.GetAllBalanceTransferAsync(It.IsAny<GetAllBalanceTransferQuery>()))
                .ReturnsAsync((transfers, 1, 1));

            // Mocks for Cash and Bank repositories
            var cashAccountRepoMock = new Mock<ICashAccountRepository>();
            cashAccountRepoMock.Setup(x => x.GetByIdAsync(fromAccountId))
                .ReturnsAsync(new CashAccount { Id = fromAccountId, AccountName = "Cash Account" });

            var bankAccountRepoMock = new Mock<IBankAccountRepository>();
            bankAccountRepoMock.Setup(x => x.GetByIdAsync(toAccountId))
                .ReturnsAsync(new BankAccount { Id = toAccountId, AccountName = "Bank Account" });

            _unitOfWorkMock.Setup(x => x.CashAccountRepository).Returns(cashAccountRepoMock.Object);
            _unitOfWorkMock.Setup(x => x.BankAccountRepository).Returns(bankAccountRepoMock.Object);

            var handler = new GetAllBalanceTransfersQueryHandler(_unitOfWorkMock.Object);
            var result = await handler.Handle(new GetAllBalanceTransferQuery(), CancellationToken.None);

            result.Item1.ShouldNotBeEmpty();
            result.Item1[0].FromAccountName.ShouldBe("Cash Account");
            result.Item1[0].ToAccountName.ShouldBe("Bank Account");
        }

    }

}
