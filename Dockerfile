FROM microsoft/dotnet:sdk AS base

COPY ./Training24Admin/release /app
WORKDIR /app
ENTRYPOINT [ "dotnet", "Training24Admin.dll" ]