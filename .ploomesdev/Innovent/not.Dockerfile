FROM mcr.microsoft.com/dotnet/core/sdk as build-stage
WORKDIR /src
ARG MSBUILD_CONFIGURATION
ENV ENV_MSBUILD_CONFIGURATION=$MSBUILD_CONFIGURATION
ENV TZ=America/Sao_Paulo
COPY . .
RUN dotnet build
RUN dotnet publish -c ${ENV_MSBUILD_CONFIGURATION} -o /publish

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 as serve-stage
WORKDIR /app
ENV TZ=America/Sao_Paulo
COPY --from=build-stage /publish/ .
#begin-links:serve
#end-links:serve
ENTRYPOINT ["dotnet", "Inoovent.dll"]
