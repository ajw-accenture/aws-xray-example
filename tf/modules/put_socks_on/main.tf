resource "aws_iam_role" "role_for_put_socks_on_nanoservice" {
  name = "role_for_put_socks_on_nanoservice"

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

resource "aws_lambda_function" "put_socks_on_nanoservice" {
  function_name = "put_socks_on_nanoservice"
  runtime       = "dotnetcore3.1"
  timeout       = 60

  handler       = "Zapatos.Chaussures.Schuhe.Function.Handler.PutSocksOn"
  filename      = ""
  role          = aws_iam_role.role_for_put_socks_on_nanoservice.arn
}
