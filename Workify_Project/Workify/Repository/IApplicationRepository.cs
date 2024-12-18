using System;
using Workify.Models;

namespace Workify.Repository;

public interface IApplicationRepository : IRepository<Application>
{
    Task<IEnumerable<Application>> GetApplicationsByJobSeekerIdAsync(int jobSeekerId);
    Task<IEnumerable<Application>> GetApplicationsByJobListingIdAsync(int jobListingId);
}
