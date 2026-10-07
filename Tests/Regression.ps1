param(
    [string]$AssemblyPath = "$PSScriptRoot\..\RamInfo\bin\Debug\RamInfo.exe"
)

$ErrorActionPreference = 'Stop'
[System.Reflection.Assembly]::LoadFrom((Resolve-Path -LiteralPath $AssemblyPath).Path) | Out-Null

# Feste Messwerte machen Fehler in Zeitpositionen und Summen reproduzierbar.
function Assert-Equal($Actual, $Expected, [string]$Description) {
    if ($Actual -ne $Expected) {
        throw "${Description}: Erwartet '$Expected', erhalten '$Actual'."
    }
}

function Add-Samples($Model, [int]$PidValue, [string]$Name, [int]$Start, [long[]]$Values) {
    $samples = New-Object 'System.Collections.Generic.List[long]'
    $samples.AddRange($Values)
    $Model.RamData.Add($PidValue, $samples)
    $Model.ProcessNames.Add($PidValue, $Name)
    $Model.ProcessStartTimes.Add($PidValue, $Start)
}

$model = New-Object RamInfo.Logic.RamDataModel
$statistics = New-Object RamInfo.Logic.RamStatistics -ArgumentList $model
Add-Samples $model 1 'Browser' 1 @(10, 20, 30)
Add-Samples $model 2 'Browser' 2 @(4, 5)
Add-Samples $model 3 'Editor' 0 @(2, 3)
Add-Samples $model -1 'Abgeleitete Summe' 0 @(999)

Assert-Equal ($statistics.BerechneGesamtverlauf() -join ',') '2,13,24,35' 'Gesamtverlauf'
$combined = $statistics.KombiniereRamVerläufeNachName()
Assert-Equal ($combined['Browser'] -join ',') '0,10,24,35' 'Zeitversetzte Prozesse'
Assert-Equal $combined.Count 2 'Abgeleitete Zeilen ausschließen'
Assert-Equal ($statistics.BerechneFrühesteStartzeiten()['Browser']) 1 'Erste Erfassung'
Assert-Equal ($statistics.KombiniereRamVerläufeNachName()['Browser'] -join ',') '0,10,24,35' 'Wiederholtes Kombinieren'
Assert-Equal ($model.RamData[1] -join ',') '10,20,30' 'Rohdaten unverändert'

# Bei 200 Messwerten umfassen die Randbereiche jeweils zwei Werte.
$values = New-Object 'System.Collections.Generic.List[long]'
$values.AddRange([long[]](200..1))
$result = $statistics.BerechneStatistik($values)
Assert-Equal $result.Item1 100.5 'Mittelwert'
Assert-Equal $result.Item2 199.5 'Höchstes Prozent'
Assert-Equal $result.Item3 1.5 'Niedrigstes Prozent'
Assert-Equal $values[0] 200 'Originalreihenfolge erhalten'
$single = New-Object 'System.Collections.Generic.List[long]'
$single.Add(42)
$result = $statistics.BerechneStatistik($single)
Assert-Equal $result.Item1 42 'Einzelwert Mittelwert'
Assert-Equal $result.Item2 42 'Einzelwert High'
Assert-Equal $result.Item3 42 'Einzelwert Low'
$result = $statistics.BerechneStatistik($null)
Assert-Equal $result.Item1 0 'Fehlende Messwerte'
$empty = New-Object 'System.Collections.Generic.List[long]'
$result = $statistics.BerechneStatistik($empty)
Assert-Equal $result.Item1 0 'Leere Messwerte'

$model.IsRecording = $true
$model.ShowOnlyTag1 = $true
$model.SecondsCounter = 4
$model.ActiveChartPIDs.Add(1) | Out-Null
$model.Reset()
Assert-Equal $model.RamData.Count 0 'Messwerte zurückgesetzt'
Assert-Equal $model.ProcessNames.Count 0 'Namen zurückgesetzt'
Assert-Equal $model.ProcessStartTimes.Count 0 'Startzeiten zurückgesetzt'
Assert-Equal $model.ActiveChartPIDs.Count 0 'Diagrammauswahl zurückgesetzt'
Assert-Equal $model.SecondsCounter 0 'Zeitachse zurückgesetzt'
Assert-Equal $model.IsRecording $false 'Aufzeichnung beendet'
Assert-Equal $model.ShowOnlyTag1 $true 'Filterauswahl beibehalten'

Write-Output 'PASS: Zeitverläufe, Kennzahlen, unveränderte Rohdaten und Aufzeichnungsreset.'
