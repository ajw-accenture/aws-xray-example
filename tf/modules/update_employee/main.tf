resource "aws_iam_role" "role_for_update_employee_nanoservice" {
  name = "role_for_update_employee_nanoservice"

  assume_role_policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Principal": {
        "Service": "lambda.amazonaws.com"
      },
      "Effect": "Allow",
      "Sid": ""
    }
  ]
}
EOF
}

resource "aws_lambda_function" "update_employee_nanoservice" {
  function_name = "update_employee_nanoservice"
  runtime       = "dotnetcore3.1"
  timeout       = 60

  handler       = "UpdateEmployee::Function::Invoke"
  filename      = "update_employee_package.zip"
  source_code_hash = filebase64sha256("update_employee_package.zip")

  role          = aws_iam_role.role_update_employee_on_nanoservice.arn
}
