provider "aws" {
  alias  = "usr1"
  region = "us-east-1"
}

module "update_employee" {
  source = "./modules/nanoservice"
  providers = {
    aws = aws.usr1
  }

  dotnet_service_name = var.dotnet_service_name
  service_name        = var.service_name
  service_handler     = var.service_handler
}

module "fetch_employee" {
  source = "./modules/nanoservice"
  providers = {
    aws = aws.usr1
  }

  dotnet_service_name = var.dotnet_service_name
  service_name        = var.service_name
  service_handler     = var.service_handler
}

module "merge_employee" {
  source = "./modules/nanoservice"
  providers = {
    aws = aws.usr1
  }

  dotnet_service_name = var.dotnet_service_name
  service_name        = var.service_name
  service_handler     = var.service_handler
}

module "employee_document_db" {
  source = "./modules/document_db"
  providers = {
    aws = aws.usr1
  }

  db_name = var.db_name
}

module "employee_merge_save_data_bus" {
  source = "./modules/data_bus"
  providers = {
    aws = aws.usr1
  }

  data_bus_name = var.data_bus_name
}
