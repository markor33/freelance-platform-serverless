using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcJobManagement;
using JobManagement.Application.Queries;

namespace JobManagement.API.GrpcServices
{
    public class ProposalGrpcService : Proposal.ProposalBase
    {
        private readonly IProposalQueries _queries;

        public ProposalGrpcService(IProposalQueries queries)
        {
            _queries = queries;
        }

        public override async Task<GetProposalsByJobIdResponse> GetProposalsByJobId(GetProposalsByJobIdRequest request, ServerCallContext context)
        {
            var proposals = await _queries.GetByJobId(Guid.Parse(request.JobId));

            var response = new GetProposalsByJobIdResponse() { JobId= request.JobId };
            foreach (var proposal in proposals)
                response.Proposals.Add(new ProposalDTO()
                {
                    Id = proposal.Id.ToString(),
                    FreelancerId = proposal.FreelancerId.ToString(),
                    Text = proposal.Text,
                    Payment = new Payment()
                    {
                        Amount = proposal.Payment.Amount,
                        Currency= proposal.Payment.Currency,
                        Type = (int)proposal.Payment.Type
                    },
                    Status = (int)proposal.Status,
                    Created = Timestamp.FromDateTime(proposal.Created),
                });

            return response;
        }
    }
}
