using DocumentValidationAPI.Application.UseCases.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentValidationAPI.Application.Abstractions.UseCases
{
    public interface IRejectDocumentService
    {
        Task<ChangeStateDocumentResult> HandleAsync(ChangeStateCommand command, CancellationToken cancellationToken = default);
    }
}
