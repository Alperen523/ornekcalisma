using System;

namespace Vakko.CrmService.Application.Abstractions.Service.BatchApprovalList.Model
{
    public class BatchApprovalListProcessStatusRequestDto
    {
        public Guid? BatchApprovalListId { get; set; } = null;

        public int? ProcessStatus { get; set; } = null;
    }
}
