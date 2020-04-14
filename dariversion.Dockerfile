FROM mcr.microsoft.com/dotnet/core/aspnet:2.1

# Expecting the release publish folder to be in Training24Api/release the same directory as the Dockerfile
COPY ./Training24Admin/releaseDari /app
WORKDIR /app
ENTRYPOINT [ "dotnet", "Training24Admin.dll" ]