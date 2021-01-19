using System.Threading.Tasks;
using Shared.Models;

namespace UpdateEmployee
{
    public interface IFetchEmployeeService {
        Task<Employee> ByPersonnelId(string id);
    }
}
