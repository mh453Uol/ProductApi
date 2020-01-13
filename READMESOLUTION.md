## Getting Started
You need to have the following software to build and run the web app
1. dotnet core 2.2.* sdk - https://dotnet.microsoft.com/download/dotnet-core/2.2

To run the web app, inside your command line do
1. `dotnet restore`
2. `dotnet run`
3. In your browser navigate to `https://localhost:5001/api/products` and you should see some json

## Documentation
You can view the documentation for the api by navigating to `https://localhost:5001/swagger/index.html` 

## Implementation Details
1. Repository Pattern - Using this pattern if we want to change our datastore, we dont have to refactor code in the controllers, we just need to reimplement the interface. So in short our application becomes persistent mechanism ignorant, for example rather than writing raw queries we can easily switch other to a ORM like EntityFramework.
2. Reduce the boilerplate code with connecting to database i.e 
    ```sh 
    var conn = Helpers.conn.Open();
    var cmd = conn.CreateCommand();
    cmd.CommandText = $"select id from Products {where}";

    var rdr = cmd.ExecuteReader();
    while (rdr.Read())
    {
        var id = Guid.Parse(rdr.GetString(0));
        Items.Add(new Product(id));
    }
    ```
    and populating the model by creating `SqliteUtil.ExecuteReader` and `SqliteUtil.NonQuery`.

3. Global error handling and logging to disk `C:\temp\nlog-*-*.log`) using NLog
