# NEXCHAT 💬

**NEXCHAT** is a modern, real-time messaging application built with a high-performance .NET backend and a dynamic frontend powered by Blazor WebAssembly and Tailwind CSS. The system utilizes SignalR to enable bidirectionally streamed live chat features seamlessly across environments.

## 🚀 Key Features

*   **Real-Time Messaging**: Built on ASP.NET Core SignalR for instantaneous encrypted messaging, live typing indicators, read receipts, and real-time status tracking (Online/Offline).
*   **Blazor WebAssembly Client**: The UI runs natively within the browser using C# (WebAssembly), providing a seamlessly rapid SPA (Single Page Application) experience.
*   **Tailwind CSS Integration**: Styled comprehensively via a Node-free standalone Tailwind CSS CLI pipeline mapping directly to Blazor Razor components, ensuring an optimized build format.
*   **Clean Architecture**: A perfectly decoupled backend split gracefully into `Core`, `UseCases`, `Infrastructure` (Entity Framework), and `API/Server` boundaries making the app exceptionally maintainable and testable.
*   **Rich Interactive Chat Flow**: Outfitted with contextual chat reactions, live message editing, message deletion propagation, drag-and-drop media capability, and cross-application sound effects bridging through Native JS-Interop.
*   **Secure Authentication**: Implements standard ASP.NET Identity, but ditches insecure local storage JWT formats in favor of strictly tracked, securely flagged HttpOnly cookies mitigating XSS vulnerabilities.

## 🛠️ Technology Stack

*   **Frontend**: Blazor WebAssembly (WASM), C#, HTML5, JavaScript
*   **Styling**: Tailwind CSS (via Tailwind Standalone CLI v3.4+)
*   **Backend Application**: ASP.NET Core Web API, C#
*   **Real-Time Engine**: ASP.NET Core SignalR
*   **Database ORM**: Entity Framework Core

## 📦 Getting Started

### Prerequisites
*   [.NET 8.0 SDK](https://dotnet.microsoft.com/download) (or newer)
*   Visual Studio 2022 / VS Code
*   SQL Server / SQLite (Depending on your EF configuration environment)

### Installation & Setup

1.  **Clone the Repository**
    ```bash
    git clone https://github.com/Simeoncollins/NEXCHAT.git
    cd NEXCHAT
    ```

2.  **Restore Dependencies**
    ```bash
    dotnet restore
    ```

3.  **Database Migration Setup**
    If the project relies on Entity Framework, update the database to apply migrations:
    ```bash
    cd NEXCHAT.Server
    dotnet ef database update
    ```
    *Note: Verify that your AppSettings.json contains valid SQL connection strings matching your local environment.*

4.  **Run the Solution**
    For development, you can run both projects simultaneously using Visual Studio (Multiple Startup Projects configuration), or via CLI:
    ```bash
    # Open one terminal for the backend
    cd NEXCHAT.Server
    dotnet run

    # Open a second terminal for the frontend
    cd NEXCHAT.Client
    dotnet watch run
    ```
    *Note: The frontend project evaluates an MSBuild `.csproj` event handling the Tailwind CSS compile build target automatically on execution.*

## 🧪 Demo Details
If you belong to a portfolio review board evaluating this project without registering an account, you can quickly skip the sign-up process leveraging explicit system accounts:

Click directly on **Demo Credentials** directly inside the primary Login Portal screen, or manually test:

*   **User 1:** 
    *   Username: `johndoe`
    *   Password: `John1234.`
*   **User 2:**
    *   Username: `janesmith`
    *   Password: `Jane1234.`

## 🛡️ License

This project operates as an Open-Source demonstration portfolio project. Feel absolutely free to clone, inspect, or branch the application logic for learning purposes.
