using DocumentValidationAPI.Api.Contracts.Requests;
using DocumentValidationAPI.Api.Contracts.Responses;
using DocumentValidationAPI.Api.Mappers;
using DocumentValidationAPI.Application.Abstractions.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace DocumentValidationAPI.Api.Controllers
{
    [ApiController]
    [Route("api/documents")]
    public class DocumentsController : ControllerBase
    {
        private readonly IUploadDocumentService _uploadDocumentService;
        private readonly IGetDownloadUrlService _getDownloadUrlService;
        private readonly IApproveDocumentService _approveDocumentService;
        private readonly IRejectDocumentService _rejectDocumentService;

        public DocumentsController(
            IUploadDocumentService uploadDocumentService, 
            IGetDownloadUrlService getDownloadUrlService,
            IApproveDocumentService approveDocumentService,
            IRejectDocumentService rejectDocumentService)
        {
            _uploadDocumentService = uploadDocumentService;
            _getDownloadUrlService = getDownloadUrlService;
            _approveDocumentService = approveDocumentService;
            _rejectDocumentService = rejectDocumentService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] UploadDocumentRequest request)
        {
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

        [HttpGet("{documentId:guid}/download")]
        public async Task<IActionResult> Download(Guid documentId)
        {
            var result = await _getDownloadUrlService.HandleAsync(documentId, HttpContext.RequestAborted);

            var response = new DownloadDocumentResponse { DownloadUrl = result.DownloadUrl };
            return Ok(response);
        }

        [HttpPost("{documentId:guid}/approve")]
        public async Task<IActionResult> Approve(Guid documentId, [FromBody] ChangeStateDocumentRequest request)
        {
            var command = request.ToChangeStateCommand(documentId);
            var result = await _approveDocumentService.HandleAsync(command);

            var response = new ChangeStateDocumentResponse
            {
                DocumentId = result.DocumentId,
                Message = result.Message
            };
            return Ok(response);
        }

        [HttpPost("{documentId:guid}/reject")]
        public async Task<IActionResult> Reject(Guid documentId, [FromBody] ChangeStateDocumentRequest request)
        {
            var command = request.ToChangeStateCommand(documentId);
            var result = await _rejectDocumentService.HandleAsync(command);

            var response = new ChangeStateDocumentResponse
            {
                DocumentId = result.DocumentId,
                Message = result.Message
            };
            return Ok(response);
        }
    }
}
