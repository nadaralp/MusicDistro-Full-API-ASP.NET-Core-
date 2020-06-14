using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDistro.Core.Events.UserActionAudit
{
    public interface IUserActionEventEmitter
    {
        public event EventHandler<UserAuditEventArgs> UserEmittedEvent;
    }
}
