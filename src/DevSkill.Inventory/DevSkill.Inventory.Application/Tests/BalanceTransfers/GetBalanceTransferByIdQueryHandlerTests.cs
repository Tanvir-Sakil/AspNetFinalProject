using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Handler;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Inventory.Application.Tests.BalanceTransfers
{
    [ExcludeFromCodeCoverage]
    public class GetBalanceTransferByIdQueryHandlerTests
    {
        private AutoMock _moq;
        private GetBalanceTranferByIdQueryHandler _handler;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;

        [OneTimeSetUp]
        public void OneTimeSetup() => _moq = AutoMock.GetLoose();

        [OneTimeTearDown]
        public void OneTimeTearDown() => _moq?.Dispose();

        [SetUp]
        public void Setup()
        {
            _handler = _moq.Create<GetBalanceTranferByIdQueryHandler>();
            _unitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        }

        [TearDown]
        public void Teardown() => _unitOfWorkMock?.Reset();

        [Test]
        public async Task Handle_ShouldReturnBalanceTransfer_WhenFound()
        {
            var transferId = Guid.Parse("B93DE9DC-8622-430A-8B2B-87978530978F");
            var expected = new BalanceTransfer { Id = transferId };

            _unitOfWorkMock
                .Setup(x => x.BalanceTransferRepository.GetByIdAsync(transferId))
                .ReturnsAsync(expected);

            var query = new GetBalanceTransferByIdQuery { Id = transferId };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.ShouldBe(expected);
        }
    }
}
