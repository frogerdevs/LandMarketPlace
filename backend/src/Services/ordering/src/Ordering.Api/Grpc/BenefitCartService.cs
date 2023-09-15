using Grpc.Core;
using GrpcOrdering;
using Mediator;

namespace Ordering.Api.Grpc
{
    public class BenefitCartService : BenefitCartGrpc.BenefitCartGrpcBase
    {
        readonly IMediator _mediator;
        public BenefitCartService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override Task<GrpcBenefitCartResponse> GetItemsByUser(GrpcByIdRequest request, ServerCallContext context)
        {
            return base.GetItemsByUser(request, context);
        }
    }
}
