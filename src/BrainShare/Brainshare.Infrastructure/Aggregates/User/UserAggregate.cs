using System;
using System.Collections.Generic;
using Brainshare.Infrastructure.Aggregates.User.Commands;
using Brainshare.Infrastructure.Aggregates.User.Events;
using Brainshare.Infrastructure.Platform.Domain;

namespace Brainshare.Infrastructure.Aggregates.User
{
    public class UserAggregate : Aggregate<UserState>
    {
        public void Create(RegisterUser c)
        {
            Apply(new UserCreated
            {
                Id = c.Id,
                Email = c.Email,
                PasswordHash = c.PasswordHash,
                PasswordSalt = c.PasswordSalt,
                CreatedOn = DateTime.UtcNow,
                FirstName = c.FirstName,
                LastName = c.LastName,
                TimeZone = AppConstants.DefaultTimeZone
            });

            if (c.Expertises != null && c.Expertises.Count > 0)
            {
                Apply(new ExpertiseSet
                {
                    Id = c.Id,
                    Areas = c.Expertises
                });
            }
        }

        public void ChangePassword(ChangePassword c)
        {
            Apply(new PasswordChanged
            {
                Id = State.Id,
                PasswordHash = c.NewPasswordHash,
                PasswordSalt = c.NewSalt
            });
        }

        public void MarkUserAsDeleted(MarkUserAsDeleted c)
        {
            Apply(new UserMarkedAsDeleted
            {
                Id = State.Id
            });
        }        
        
        public void Delete(DeleteUser c)
        {
            Apply(new UserDeleted
            {
                Id = State.Id
            });
        }

        public void UpdateUserDetails(UpdateUserDetails c)
        {

            if (State.FirstName != c.FirstName || State.LastName != c.LastName)
            {
                Apply(new UserFirstLastNameChanged
                {
                    Id = State.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName
                });
            }

            if (State.Location != c.Location)
            {
                Apply(new UserLocationChanged()
                {
                    Id = State.Id,
                    Location = c.Location
                });
            }

            if (State.Headline != c.Title)
            {
                Apply(new UserHeadlineChanged()
                {
                    Id = State.Id,
                    Headline = c.Title
                });
            }            
            
            if (State.Department != c.Department)
            {
                Apply(new UserDepartmentChanged()
                {
                    Id = State.Id,
                    Department = c.Department
                });
            }
        }

        public void SetExpertise(List<string> areas)
        {
            Apply(new ExpertiseSet()
            {
                Id = State.Id,
                Areas = areas
            });
        }

        public void SetInterests(List<string> interests)
        {
            Apply(new InterestsSet()
            {
                Id = State.Id,
                Interests = interests
            });
        }

        public void UpdateWorks(UpdateWorkExperiences c)
        {
            foreach (var work in c.Items)
            {
                if (string.IsNullOrEmpty(work.WorkId))
                {
                    Apply(new WorkExperienceAdded
                        {
                            Id = State.Id,
                            WorkId = work.RandomId,
                            Company = work.Company,
                            Location = work.Location,
                            Title = work.Title,
                            Start = work.Start,
                            End = work.End,
                            Added = DateTime.UtcNow
                        });
                }
                else
                {
                    Apply(new WorkExperienceUpdated
                    {
                        Id = State.Id,
                        WorkId = work.WorkId,
                        Company = work.Company,
                        Location = work.Location,
                        Title = work.Title,
                        Start = work.Start,
                        End = work.End,
                        UpdatedOn = DateTime.UtcNow
                    });   
                } 
            }

            foreach (var workId in c.RemoveWorkIds)
            {
                Apply(new WorkExperienceRemoved
                {
                    Id = State.Id,
                    WorkId = workId
                });
            }
        }

        public void UpdateInstitutes(UpdateEducations c)
        {
            foreach (var education in c.Items)
            {
                if (string.IsNullOrEmpty(education.EducationId))
                {
                    Apply(new EducationAdded
                    {
                        Id = State.Id,
                        EducationId = education.RandomId,
                        InstituteName = education.InstituteName,
                        Degree = education.Degree,
                        Location = education.Location,
                        StartYearMonth = education.Start,
                        EndYearMonth = education.End,
                        Added = DateTime.UtcNow
                    });
                }
                else
                {
                    Apply(new EducationUpdated
                    {
                        Id = State.Id,
                        EducationId = education.EducationId,
                        InstituteName = education.InstituteName,
                        Degree = education.Degree,
                        Location = education.Location,
                        StartYearMonth = education.Start,
                        EndYearMonth = education.End,
                        Updated = DateTime.UtcNow
                    });
                }
            }

            foreach (var instituteId in c.RemoveEducationIds)
            {
                Apply(new EducationRemoved
                {
                    Id = State.Id,
                    EducationId = instituteId,
                });
            }

        }

        public void UpdateSummary(string summary)
        {
            Apply(new SummaryUpdated()
            {
                Id = State.Id,
                Summary = summary
            });
        }

        public void UpdateResetPasswordCode(string code)
        {
            if (State.Activated)
            {
                Apply(new ResetPasswordCodeUpdated()
                {
                    Id = State.Id,
                    Code = code
                });
            }
        }

        public void ChangeUserAvatar(ChangeUserAvatar c)
        {
            Apply(new UserAvatarChanged()
            {
                Id = State.Id,
                AvatarImageId = c.AvatarImageId,
            });
        }

        public void SendEmailInvitions(SendEmailInvitions c)
        {
            Apply(new EmailInvitationsSent()
            {
                Id = State.Id,
                SendInvitesTo = c.SendInvitesTo,
                OptionalMessage = c.OptionalMessage
            });
        }


        public void AddRole(AddUserRole c)
        {
            if (State.IsUserHasRole(c.Role))
                return;

            Apply(new UserRoleAdded()
            {
                Id = c.Id,
                Role = c.Role
            });
        }

        public void RemoveRole(RemoveUserRole c)
        {
            if (!State.IsUserHasRole(c.Role))
                return;

            Apply(new UserRoleRemoved()
            {
                Id = c.Id,
                Role = c.Role
            });
        }


        public void Activate(ActivateUser c)
        {
            Apply(new UserActivated()
            {
                Id = c.Id,
                Email = c.Email
            });
        }

        public void Deactivate(DeactivateUser c)
        {
            Apply(new UserDeactivated()
            {
                Id = c.Id,
                Email = c.Email
            });
        }
        public void CreateSystemUser(CreateSystemUser c)
        {
            Apply(new SystemUserCreated()
            {
                Id = c.Id,
                TenantId = c.TenantId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                CreatedOn = c.CreatedOn,
                PasswordHash = c.PasswordHash,
                PasswordSalt = c.PasswordSalt
            });
        }
    }
}
