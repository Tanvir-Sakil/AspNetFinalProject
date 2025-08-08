﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Commands
{
    public class BankAccountDeleteCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
