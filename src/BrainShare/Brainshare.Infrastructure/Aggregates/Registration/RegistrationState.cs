using System;
using System.Collections.Generic;
using Brainshare.Infrastructure.Aggregates.Registration.Events;

namespace Brainshare.Infrastructure.Aggregates.Registration
{
    public sealed class RegistrationState : AggregateState
    {
        public RegistrationState()
        {
            InitiatedRegistrations = new Dictionary<string, InitiatedRegistation>();
            DeletedRegistrations = new List<string>();
            RegistredTenants = new Dictionary<string, string>();

            On((UserRegistrationInitiated message) =>
                InitiatedRegistrations.Add(message.Email.ToLowerInvariant(), new InitiatedRegistation()
                {
                    Email = message.Email,
                    UserId = message.Id,
                    CreatedOn = message.CreatedOn,
                    EmailVerificationCode = message.EmailVerificationCode,
                    InviteFromDomain = message.InviteFromDomain,
                    ExpirationDate = message.ExpirationDate,
                })
            );

            On((RegistrationTenantCreated message) => RegistredTenants.Add(message.SubDomain, message.TenantId));

            On((RegistrationExpired message) => InitiatedRegistrations.Remove(message.Email));

            On((RegistrationUserCreated message) => InitiatedRegistrations.Remove(message.Email));

            On((UserRegistrationDeleted message) => DeletedRegistrations.Add(message.Email.ToLowerInvariant()));
        }

        public static string Id = "registration";

        /// <summary>
        /// user email, initiated registration
        /// </summary>
        public Dictionary<string, InitiatedRegistation> InitiatedRegistrations { get; set; }

        public List<string> DeletedRegistrations { get; set; }

        //lowercased subdomain/ tenant id
        public Dictionary<string, string> RegistredTenants { get; set; }
    }

    public class InitiatedRegistation
    {
        public string Email { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string EmailVerificationCode { get; set; }

        public string InviteFromDomain { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
