using DocumentValidationAPI.Api.Contracts.Requests;
using DocumentValidationAPI.Api.Contracts.Responses;
using DocumentValidationAPI.Api.Mappers;
using DocumentValidationAPI.Application.UseCases.Documents.GetDownloadUrl;
using DocumentValidationAPI.Application.UseCases.Documents.UploadDocument;
using Microsoft.AspNetCore.Mvc;

namespace DocumentValidationAPI.Api.Controllers
{
    [ApiController]
    [Route("api/documents")]
    public class DocumentsController : ControllerBase
    {
        private readonly UploadDocumentService _uploadDocumentService;
        private readonly GetDownloadUrlService _getDownloadUrlService;

        public DocumentsController(UploadDocumentService uploadDocumentService, GetDownloadUrlService getDownloadUrlService)
        {
            _uploadDocumentService = uploadDocumentService;
            _getDownloadUrlService = getDownloadUrlService;
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

        [HttpGet("{documentId}/download")]
        public async Task<IActionResult> Download (Guid documentId)
        {
            var result = await _getDownloadUrlService.HandleAsync(documentId, HttpContext.RequestAborted);

            var response = new DownloadDocumentResponse { DownloadUrl = result.DownloadUrl };
            return Ok(response);
        }
    }
}
