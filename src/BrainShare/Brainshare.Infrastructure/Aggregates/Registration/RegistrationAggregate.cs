using System;
using System.Text.RegularExpressions;
using Brainshare.Infrastructure.Aggregates.Registration.Commands;
using Brainshare.Infrastructure.Aggregates.Registration.Events;
using Brainshare.Infrastructure.Infrastructure;
using Brainshare.Infrastructure.Platform.Domain;
using Brainshare.Infrastructure.Platform.Mongo;

namespace Brainshare.Infrastructure.Aggregates.Registration
{
    public class RegistrationAggregate : Aggregate<RegistrationState>
    {
        private readonly CryptographicHelper _cryptoHelper;
        public RegistrationAggregate()
        {
            _cryptoHelper = new CryptographicHelper();
        }

        public void InitiateUserRegisteration(InitiateUserRegisteration c)
        {
            var email = c.Email.ToLowerInvariant();

            if (State.DeletedRegistrations.Contains(email))
                return;

            var emailDomain = Email.GetDomain(email);
            var subDomain = ExtractTenantName(emailDomain);

            var tenantId = c.TenantId;

            if (State.RegistredTenants.ContainsKey(subDomain))
                tenantId = State.RegistredTenants[subDomain];

            if (State.InitiatedRegistrations.ContainsKey(email))
            {
                if (DateTime.UtcNow > State.InitiatedRegistrations[email].ExpirationDate )
                {
                    Apply(new RegistrationExpired()
                    {
                        Email = email,
                        Id = RegistrationState.Id,
                        TenantId = c.TenantId,
                        UserId = c.Id
                    });
                }
            }

            if (State.InitiatedRegistrations.ContainsKey(email))
            {
                Apply(new UserRegistrationReInitiated()
                {
                    Id = c.UserId,
                    Email = email,
                    TenantId = tenantId,
                    UserId = c.UserId,
                    TenantName = subDomain,
                    IsInitiatedFromInvite = c.IsInitiatedFromInvite,
                    InviteOptionalMessage = c.InviteOptionalMessage,
                    EmailVerificationCode = State.InitiatedRegistrations[email].EmailVerificationCode,
                });
            }
            else
            {
                var emailVerificationCode = new IdGenerator().Generate();
                Apply(new UserRegistrationInitiated()
                {
                    Id = RegistrationState.Id,
                    Email = email,
                    EmailVerificationCode = emailVerificationCode,
                    TenantId = tenantId,
                    UserId = c.UserId,
                    IsInitiatedFromInvite = c.IsInitiatedFromInvite,
                    InviteOptionalMessage = c.InviteOptionalMessage,
                    InviteFromDomain = c.InviteFromDomain,
                    TenantName = subDomain,
                    CreatedOn = DateTime.UtcNow,
                    ExpirationDate = DateTime.UtcNow.AddHours(AppConstants.RegistrationVerificationCodeExpirationHours)
                });   
            }
        }

        public void FinalizeRegistation(FinalizeUserRegistation c)
        {
            //for example: andrew@org.com
            var email = c.UserEmail.ToLowerInvariant();

            if (State.DeletedRegistrations.Contains(email))
                return;

            if (!State.InitiatedRegistrations.ContainsKey(email))
                return;

            var registration = State.InitiatedRegistrations[email];

            //should be org.com
            var emailDomain = String.Empty;
            var subDomain = String.Empty;

            if (!string.IsNullOrEmpty(registration.InviteFromDomain))
            {
                emailDomain = registration.InviteFromDomain;
            }
            else
            {
                emailDomain = c.IsPublicEmail ? c.TenantName.ToLowerInvariant() : Email.GetDomain(email);
            }
            subDomain = ExtractTenantName(emailDomain);

            // if such tenant with such subdomain is not registred -- we need to register it 
            // user will be created in Tenant workflow

            if (!State.RegistredTenants.ContainsKey(subDomain))
            {
                var tenantId = c.TenantId;
                Apply(new RegistrationTenantCreated()
                {
                    Id = RegistrationState.Id,
                    TenantId = tenantId,
                    EmailDomain = emailDomain,
                    UserEmail = email,
                    UserFirstName = c.FirstName,
                    UserLastName = c.LastName,
                    UserPasswordHash = c.PasswordHash,
                    UserPasswordSalt = c.PasswordSalt,
                    SubDomain = subDomain,
                    UserExpertises = c.Expertises,
                    UserId = c.UserId,
                    IsPublicEmail = c.IsPublicEmail
                });
            }
            else
            {
                var tenantId = State.RegistredTenants[subDomain];
                Apply(new RegistrationUserCreated()
                {
                    Id = RegistrationState.Id,
                    Email = registration.Email,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    TenantId = tenantId,
                    PasswordHash = c.PasswordHash,
                    PasswordSalt = c.PasswordSalt,
                    Expertises = c.Expertises,
                    UserId = c.UserId
                });
            }
            Apply(new RegistrationExpired()
            {
                Email = email,
                Id = RegistrationState.Id,
                TenantId = c.TenantId,
                UserId = c.Id
            });
        }

        private static string ExtractTenantName(string emailDomain)
        {
            var indexOfFirstLevelDomain = emailDomain.LastIndexOf(".", StringComparison.Ordinal);
            if (indexOfFirstLevelDomain < 0)
            {
                return emailDomain;
            }
            //should be org
            var subDomain = emailDomain.Substring(0, indexOfFirstLevelDomain);

            var alphaNumericRegex = new Regex("[^a-zA-Z0-9]");
            //we need regex to make sure that subdomain name does not contains any special characters, 
            //for example for emailDomain: org.co.uk sub domain should be orgco, but not org.co
            subDomain = alphaNumericRegex.Replace(subDomain, string.Empty);
            return subDomain;
        }

        public void ExpireRegistration(ExpireRegistration c)
        {
            var email = c.Email.ToLowerInvariant();

            if (State.DeletedRegistrations.Contains(email))
                return;

            if (!State.InitiatedRegistrations.ContainsKey(email))
                return;

            Apply(new RegistrationExpired()
            {
                Email = email,
                Id = RegistrationState.Id,
                TenantId = c.TenantId,
                UserId = c.Id
            });
        }

        public void DeleteUserRegistration(DeleteUserRegistation c)
        {
            Apply(new UserRegistrationDeleted()
            {
                Id = c.Id,
                Email = c.Email,
                DeleteContent = c.DeleteContent
            });
        }
    }

    public static class Email
    {
        public static string GetDomain(string email)
        {
            var indexOfAtCharInEmail = email.IndexOf("@", StringComparison.Ordinal) + 1;

            return email.Substring(indexOfAtCharInEmail, email.Length - indexOfAtCharInEmail);
        }
    }
}
