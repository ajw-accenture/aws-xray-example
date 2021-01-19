variable "dotnet_service_name" {
  type        = string
  description = "The service name that houses the C# code.  Ex: FetchEmployee"
}

variable "service_name" {
  type        = string
  description = "The simple form of the service name.  Ex: fetch_employee"
}

variable "service_handler" {
  type        = string
  description = "Full path to handler"
}

variable "aws_account_id" {
  type = string
}

variable "aws_region_name" {
  type = string
}
