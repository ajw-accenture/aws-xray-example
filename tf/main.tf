provider "aws" {
  alias = "usw1"
  region = "us-west-1"
}

module "update_employee" {
  source = "./modules/update_employee"
  providers = {
    aws = aws.usw1
  }
}

module "fetch_employee" {
  source = "./modules/fetch_employee"
  providers = {
    aws = aws.usw1
  }
}

module "merge_employee" {
  source = "./modules/merge_employee"
  providers = {
    aws = aws.usw1
  }
}
