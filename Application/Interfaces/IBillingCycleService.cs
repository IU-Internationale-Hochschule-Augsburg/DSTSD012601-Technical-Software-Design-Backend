using Subscription_Control_Backend.Contracts.Requests.BillingCycles;
using Subscription_Control_Backend.Contracts.Responses.BillingCycles;

namespace Subscription_Control_Backend.Application.Interfaces;

public interface IBillingCycleService : ICrudService<BillingCycleResponse, CreateBillingCycleRequest, UpdateBillingCycleRequest>
{
}
