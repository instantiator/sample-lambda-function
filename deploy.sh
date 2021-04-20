#!/bin/sh

dotnet build

pushd src/SampleLambdaFunction
dotnet lambda deploy-function --profile sample-admin --region eu-west-2 SampleLambdaFunction --function-role sample-lambda-role
popd
