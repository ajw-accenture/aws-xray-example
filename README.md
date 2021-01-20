# Employee Product

## Initialize environment

* Install dotnet core 3.1: https://dotnet.microsoft.com/download/dotnet-core
* Install Python 3: https://www.python.org/downloads/
* Install Terraform CLI: https://www.terraform.io/downloads.html
* Install AWS lambda tools into the dotnet CLI: `dotnet tool install -g Amazon.Lambda.Tools`.

## How to deploy

1. Get AWS credentials so that the deployment process can access your AWS instance.
2. `cd` into the `tf/` directory and run `terraform init`.
3. `cd` back to the root of the repository.
4. Deploy the database: run `python3 scripts/deploy_non_service_resource.py --resource employee_document_db`
5. Deploy the SNS topic: run `python3 scripts/deploy_non_service_resource.py --resource employee_merge_save_data_bus`
6. Deploy UpdateEmployee lambda: run `python3 scripts/deploy_service.py --service update_employee`
7. Deploy FetchEmployee lambda: run `python3 scripts/deploy_service.py --service fetch_employee`
8. Deploy MergeEmployee lambda: run `python3 scripts/deploy_service.py --service merge_employee`

## How to run

1. Trigger the `update_employee` lambda via the AWS console or AWS CLI.
2. Visit the AWS X-Ray console page and then click Traces to see traces from the execution of the lambda.
