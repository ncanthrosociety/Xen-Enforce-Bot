# FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:3.1 AS base
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS base
WORKDIR /src

COPY src/Xen-Enforce-Bot.csproj ./
RUN ["dotnet", "restore"]

FROM base as builder

COPY src .

CMD ["dotnet", "build", "-c", "-o", "/build"]

FROM builder AS publisher

RUN ["dotnet", "publish", "-c", "Release", "-o", "/publish"]

FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /app
COPY --from=publisher /publish .

CMD ["dotnet", "XenfbotDN.dll"]