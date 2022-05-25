FROM mcr.microsoft.com/dotnet/sdk:2.1.811-focal AS build
WORKDIR webapp
COPY ./*csproj ./
RUN dotnet restore
COPY . .
#RUN dotnet publish -c Release -o out
CMD dotnet run

#FROM mcr.microsoft.com/dotnet/sdk:2.1.811-focal
#WORKDIR /webapp
#COPY --from=build /webapp/out /webapp
#CMD ["dotnet", "/webapp/protecta.laft.api.dll"]
