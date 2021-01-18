resource "aws_iam_role" "role_for_nanoservice" {
  name = "role_for_${var.service_name}_nanoservice"

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

resource "aws_lambda_function" "nanoservice" {
  function_name = "${var.service_name}_nanoservice"
  runtime       = "dotnetcore3.1"
  timeout       = 60

  handler       = var.service_handler
  filename      = "${var.service_name}_pkg.zip"
  source_code_hash = filebase64sha256("${var.service_name}_pkg.zip")

  role          = aws_iam_role.role_for_nanoservice.arn
}
