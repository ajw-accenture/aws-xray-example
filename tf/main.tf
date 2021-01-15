provider "aws" {
  alias = "usw1"
  region = "us-west-1"
}

module "put_socks_on" {
  source = "./modules/put_socks_on"
  providers = {
    aws = aws.usw1
  }
}

module "put_shoes_on" {
  source = "./modules/put_shoes_on"
  providers = {
    aws = aws.usw1
  }
}

module "tie_shoes" {
  source = "./modules/tie_shoes"
  providers = {
    aws = aws.usw1
  }
}
