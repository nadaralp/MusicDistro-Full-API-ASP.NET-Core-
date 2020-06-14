using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistro.Core.Events.UserActionAudit
{
    public interface IUserEventSubscriber
    {
        void SubscribeToUserEvent(IUserEventService userEventService);
        void AuditEvent(object sender, UserAuditEventArgs e);
    }
}
