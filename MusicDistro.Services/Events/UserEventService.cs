using MusicDistro.Core.Events.UserActionAudit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace MusicDistro.Services.Events
{
    public class UserEventService : IUserEventService
    {
        public event EventHandler<UserAuditEventArgs> UserEmittedEvent;

        public void EmitUserEvent(ClaimsPrincipal claimsPrincipal, Core.Entities.UserActionType userAction)
        {
            Guid userId = new Guid(claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            string userFirstName = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "FirstName").Value;

            string eventDescription = $"{userFirstName} Has Performed - {userAction}";

            UserEmittedEvent(this, new UserAuditEventArgs
            {
                DateTime = DateTime.Now,
                UserActionType = userAction,
                EventDescription = eventDescription,
                UserId = userId
            });
        }
    }
}
