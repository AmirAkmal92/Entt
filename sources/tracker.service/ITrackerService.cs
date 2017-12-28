using System;
using System.Threading.Tasks;

namespace Bespoke.Entt.Tracker.Service
{
    public interface ITrackerService
    {
        Task<bool> GetStatusAsync(string hash, string consignmentNo, DateTime? datetime, string eventName);
        Task<int> AddStatusAsync(string hash, string consignmentNo, DateTime? datetime, string eventName);
    }
}
