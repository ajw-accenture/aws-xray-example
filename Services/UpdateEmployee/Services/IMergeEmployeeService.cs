using System.Threading.Tasks;
using Shared.Models;

namespace UpdateEmployee.Services
{
    public interface IMergeEmployeeService
    {
        Task MergeSave(Employee employee);
    }
}
