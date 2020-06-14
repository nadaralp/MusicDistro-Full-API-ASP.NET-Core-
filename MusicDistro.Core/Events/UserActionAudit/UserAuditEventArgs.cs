using MusicDistro.Core.Entities;
using System;

namespace MusicDistro.Core.Events.UserActionAudit
{
    public class UserAuditEventArgs : EventArgs
    {
        public Guid UserId { get; set; }
        public UserActionType UserActionType { get; set; }
        public string EventDescription { get; set; }
        public DateTime DateTime { get; set; }
    }
}
