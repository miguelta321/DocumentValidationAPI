using DocumentValidationAPI.Api.Contracts.Requests;
using DocumentValidationAPI.Api.Contracts.Responses;
using DocumentValidationAPI.Api.Mappers;
using DocumentValidationAPI.Application.UseCases.Documents.UploadDocument;
using Microsoft.AspNetCore.Mvc;

namespace DocumentValidationAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly UploadDocumentService _uploadDocumentService;

        public DocumentsController(UploadDocumentService uploadDocumentService)
        {
            _uploadDocumentService = uploadDocumentService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] UploadDocumentRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var command = request.ToUploadDocumentCommand();
            var result = await _uploadDocumentService.HandleAsync(command, HttpContext.RequestAborted);

            var response = new UploadDocumentResponse
            {
                DocumentId = result.DocumentId,
                UploadUrl = result.UploadUrl,
                BucketKey = result.BucketKey
            };

            return Ok(response);
        }
    }
}
