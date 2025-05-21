# Uruchamianie

## 1. Backend

cd UserManagementSystem/UserManagementSystem.Server
dotnet restore
dotnet run

## 2.Frontend

cd UserManagementSystem/UserManagementSystem.Client
npm install       # lub: yarn install
npm run serve     # lub: yarn serve

Frontend uruchmi siê pod adresem https://localhost:56031/

## 3.Testy jednostkowe

cd UserManagementSystemTests
dotnet restore
dotnet test