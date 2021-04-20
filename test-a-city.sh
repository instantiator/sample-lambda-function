#!/bin/sh

dotnet lambda invoke-function SampleLambdaFunction --profile sample-admin --region eu-west-2 --payload 'London, UK'

