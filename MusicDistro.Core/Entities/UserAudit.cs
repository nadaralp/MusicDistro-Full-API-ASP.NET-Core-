using MusicDistro.Core.Entities.Auth;
using System;
using System.Collections.Generic;

namespace MusicDistro.Core.Entities
{
    public class UserAudit
    {
        public int Id { get; set; }
        public UserActionType ActionType { get; set; }
        public string EventDescription { get; set; }
        public DateTime DateTime { get; set; }
        public virtual User User { get; set; }
        public Guid UserId { get; set; } // user foreign key
    }

    public enum UserActionType
    {
        MusicAdded,
        MusicDeleted,
        ArtistAdded
    }
}
