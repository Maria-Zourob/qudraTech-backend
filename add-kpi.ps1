$connectionString = "Server=localhost\SQLEXPRESS;Database=QudraTechDb;Trusted_Connection=True;TrustServerCertificate=True"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()
$command = $connection.CreateCommand()
$command.CommandText = "DELETE FROM InitiativeKpis WHERE InitiativeId = '63f5dc66-0f1a-4cf3-ad95-888d15f2b6d0'"
$command.ExecuteNonQuery()
$connection.Close()
Write-Host "Old KPIs deleted"