resource "aws_dynamodb_table" "document_db" {
  name           = var.db_name
  read_capacity  = 20
  write_capacity = 20
  hash_key       = "personnel_id"

  attribute {
    name = "personnel_id"
    type = "S"
  }
}
