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
