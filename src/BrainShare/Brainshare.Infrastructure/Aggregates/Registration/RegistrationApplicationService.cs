using Brainshare.Infrastructure.Aggregates.Registration.Commands;

namespace Brainshare.Infrastructure.Aggregates.Registration
{
    public class RegistrationApplicationService : IMessageHandler
    {
        private readonly IRepository<RegistrationAggregate> _repository;

        public RegistrationApplicationService(IRepository<RegistrationAggregate> repository)
        {
            _repository = repository;
        }

        public void Handle(InitiateUserRegisteration c)
        {
            _repository.Perform(RegistrationState.Id, user => user.InitiateUserRegisteration(c));
        }

        public void Handle(FinalizeUserRegistation c)
        {
            _repository.Perform(RegistrationState.Id, user => user.FinalizeRegistation(c));
        }

        public void Handle(ExpireRegistration c)
        {
            _repository.Perform(RegistrationState.Id, user => user.ExpireRegistration(c));
        }

        public void Handle(DeleteUserRegistation c)
        {
            _repository.Perform(RegistrationState.Id, user => user.DeleteUserRegistration(c));
        }         
            
    }
}
