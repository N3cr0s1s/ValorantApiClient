using MediatR;

namespace ValorantClient.Lib.API.PreGame.QuitMatch
{
    public class QuitMatchCommand : IRequest
    {
        public string MatchId { get; set; }
    }
}
