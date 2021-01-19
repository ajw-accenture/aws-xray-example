using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Shared.Models;

namespace FetchEmployee
{
    public class Function
    {
        static Function()
        {
            Initialize();
        }

        static void Initialize()
        {
            AWSSDKHandler.RegisterXRayForAllServices();
        }

        [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
        public async Task<Employee> Invoke(object input, ILambdaContext context)
        {
            var dynamo = new AmazonDynamoDBClient();
            var employees = await dynamo.ScanAsync(new ScanRequest
            {
                TableName = "employee_document_db",
                Limit = 1
            });

            var firstEmployee = employees.Items.First();

            return new Employee
            {
                PersonnelId = firstEmployee["personnel_id"].S,
                Name = firstEmployee["name"].S,
                Department = firstEmployee["department"].S
            };
        }
    }
}
