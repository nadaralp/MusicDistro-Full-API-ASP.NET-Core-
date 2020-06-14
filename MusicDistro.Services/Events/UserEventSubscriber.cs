using MusicDistro.Core.Entities;
using MusicDistro.Core.Events.UserActionAudit;
using MusicDistro.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistro.Services.Events
{
    class UserEventSubscriber : IUserEventSubscriber
    {
        private readonly IUserAuditRepository _repository;
        public UserEventSubscriber(IUserAuditRepository repository)
        {
            _repository = repository;
        }


        public async void AuditEvent(object sender, UserAuditEventArgs e)
        {
            var userAudit = new UserAudit
            {
                ActionType = e.UserActionType,
                DateTime = e.DateTime,
                EventDescription = e.EventDescription,
                UserId = e.UserId,
            };

            await _repository.AddAsync(userAudit);
            await _repository.Commit();
        }

        public void SubscribeToUserEvent(IUserEventService userEventService)
        {
            userEventService.UserEmittedEvent += AuditEvent;
        }
    }
}
