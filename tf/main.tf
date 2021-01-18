provider "aws" {
  alias = "usr1"
  region = "us-east-1"
}

module "update_employee" {
  source = "./modules/update_employee"
  providers = {
    aws = aws.usr1
  }
}

module "fetch_employee" {
  source = "./modules/fetch_employee"
  providers = {
    aws = aws.usr1
  }
}

module "merge_employee" {
  source = "./modules/merge_employee"
  providers = {
    aws = aws.usr1
  }
}
