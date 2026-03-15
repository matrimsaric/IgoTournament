using CompetitionDomain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoneLedger.Services.Api.Interfaces
{
    public interface ISgfService
    {
        Task CreateSgfRecord(SgfRecord sgfRecord);
    }
}
