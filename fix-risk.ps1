$connectionString = "Server=localhost\SQLEXPRESS;Database=QudraTechDb;Trusted_Connection=True;TrustServerCertificate=True"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()
$command = $connection.CreateCommand()
$command.CommandText = @"
UPDATE Risks
SET DescriptionAr = N'انقطاع الإنترنت أو الكهرباء أثناء الجلسات التدريبية',
    Mitigation = N'الانتقال للتدريب الورقي ومحاكاة الأوامر (Offline Prompt Simulation)',
    ContingencyPlan = N'التنسيق المسبق مع مراكز مجتمعية قريبة لمصادر طاقة بديلة'
WHERE Id = '4fa87ff0-b0d2-4610-9213-7a26d6facd64'
"@
$command.ExecuteNonQuery()
$connection.Close()
Write-Host "Risk Arabic text fixed"