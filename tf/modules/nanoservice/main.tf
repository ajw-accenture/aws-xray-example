terraform {
  required_providers {
    aws = {
      source = "hashicorp/aws"
    }
  }
}

resource "aws_iam_role" "role_for_nanoservice" {
  name = "role_for_${var.service_name}_nanoservice"

  assume_role_policy = <<-EOF
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

resource "aws_iam_role_policy" "role_policy_for_nanoservice" {
  name = "role_policy_for_${var.service_name}_nanoservice"
  role = aws_iam_role.role_for_nanoservice.id

  policy = <<-EOF
  {
    "Version": "2012-10-17",
    "Statement": [
      {
        "Action": [
          "xray:PutTraceSegments",
          "xray:PutTelemetryRecords",
          "xray:GetSamplingRules",
          "xray:GetSamplingTargets",
          "xray:GetSamplingStatisticSummaries"
        ],
        "Effect": "Allow",
        "Resource": "*"
      },
      {
        "Action": [
          "logs:CreateLogGroup",
          "logs:CreateLogStream",
          "logs:PutLogEvents"
        ],
        "Effect": "Allow",
        "Resource": "arn:aws:logs:*:*:*"
      },
      {
        "Action": [
          "lambda:InvokeFunction",
          "lambda:InvokeAsync"
        ],
        "Effect": "Allow",
        "Resource": "*"
      },
      {
        "Action": [
          "dynamodb:GetItem",
          "dynamodb:BatchGetItem",
          "dynamodb:PutItem",
          "dynamodb:BatchWriteItem",
          "dynamodb:UpdateItem",
          "dynamodb:Scan"
        ],
        "Effect": "Allow",
        "Resource": "arn:aws:dynamodb:*:*:table/employee_document_db"
      },
      {
        "Action": [
          "sns:Publish"
        ],
        "Effect": "Allow",
        "Resource": "arn:aws:sns:*:*:employee_merge_save"
      }
    ]
  }
  EOF
}

data "aws_caller_identity" "current" {}
data "aws_region" "current" {}

resource "aws_lambda_function" "nanoservice" {
  function_name = "${var.service_name}_nanoservice"
  runtime       = "dotnetcore3.1"
  timeout       = 60
  memory_size   = 512

  handler          = var.service_handler
  filename         = "${var.service_name}_pkg.zip"
  source_code_hash = filebase64sha256("${var.service_name}_pkg.zip")

  role = aws_iam_role.role_for_nanoservice.arn

  tracing_config {
    mode = "Active"
  }

  environment {
    variables = {
      EMPLOYEE_MERGE_SAVE_SNS_ARN = "arn:aws:sns:${var.aws_region_name}:${var.aws_account_id}:employee_merge_save"
    }
  }
}
