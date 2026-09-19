$connectionString = "Server=localhost\SQLEXPRESS;Database=QudraTechDb;Trusted_Connection=True;TrustServerCertificate=True"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()
$command = $connection.CreateCommand()
$command.CommandText = @"
UPDATE Initiatives
SET TitleAr = N'قدرات غزة الرقمية',
    DescriptionAr = N'تعليم الأطفال هندسة أوامر الذكاء الاصطناعي في خيام النزوح بغزة.',
    TargetGroupAr = N'أطفال 10-14 سنة'
WHERE Slug = 'gaza-qudratech'
"@
$command.ExecuteNonQuery()
$connection.Close()
Write-Host "Arabic text fixed"