using System.Threading.Tasks;
using Shared.Models;

namespace UpdateEmployee.Services
{
    public interface IFetchEmployeeService {
        Task<Employee> ByPersonnelId(string id);
    }
}
