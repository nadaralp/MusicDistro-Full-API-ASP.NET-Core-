using MusicDistro.Core.Entities;
using System.Security.Claims;

namespace MusicDistro.Core.Events.UserActionAudit
{
    public interface IUserEventService : IUserActionEventEmitter
    {
        void EmitUserEvent(ClaimsPrincipal claimsPrincipal, UserActionType userAction);
    }
}
