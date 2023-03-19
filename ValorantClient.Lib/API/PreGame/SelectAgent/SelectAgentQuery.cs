using MediatR;

namespace ValorantClient.Lib.API.PreGame.SelectAgent
{
    public class SelectAgentQuery : IRequest<SelectAgentResponse>
    {
        public string MatchId { get; set; }

        public string AgentId { get; set; }

    }
}
