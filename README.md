# Getting Started

## 1. Backend (ASP.NET Core API)

### 1.1 Apply Migrations
1. In **Visual Studio**, go to:  
   **Tools → NuGet Package Manager → Package Manager Console**
2. In the console, change the **Default Project** dropdown to:  
   `ToDoList.Infrastructure`
3. Run the following commands:

```powershell
add-migration <MigrationName>
update-database
```

### 1.2 Run the API
- Press F5 in Visual Studio to run
- If you are using Powershell:
  ```powershell
  dotnet run --project ToDoList.API
  ```
Once running, open your browser at: http://localhost:5084/swagger
(The port may differ, check your console output.)

## 2. Fronend (React)

### 2.1 Install Dependencies
1. Open Command Prompt
2. Navigate into the React project folder:
   ```powershell
   cd ToDoList.Web
   npm install
   npm run dev
   ```
By default the frontend runs at:
http://localhost:5173

You can change the API URL (or the frontend port) by editing the .env file in the ToDoList.Web folder:

```ini
VITE_API_URL=http://localhost:5084
```
(Change the port if your API runs on a different one.)

*** The React Frontend still have bugs, looking for fixes soon ***
