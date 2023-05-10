using BackendTools.Common.Models;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction;

public interface IValidator<in TModel> where TModel: class
{
    Task<Result> ValidateAsync(TModel model);
}
