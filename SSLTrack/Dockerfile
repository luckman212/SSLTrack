FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SSLTrack/SSLTrack.csproj", "SSLTrack/"]
RUN dotnet restore "./SSLTrack/SSLTrack.csproj"
COPY . .

ARG TARGETPLATFORM

RUN if [ "$TARGETPLATFORM" = "linux/amd64" ]; then \
    RID=linux-x64 ; \
    elif [ "$TARGETPLATFORM" = "linux/arm64" ]; then \
    RID=linux-arm64 ; \
    elif [ "$TARGETPLATFORM" = "linux/arm/v7" ]; then \
    RID=linux-arm ; \
    fi \
    && dotnet publish "SSLTrack/SSLTrack.csproj" -c Release -o /app/publish -r $RID --self-contained false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY ["assets/data.db", "./"]

ENTRYPOINT ["./SSLTrack"]