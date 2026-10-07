# RamInfo

RamInfo ist eine Windows-Desktopanwendung zur Aufzeichnung und Auswertung des Arbeitsspeicherverbrauchs laufender Prozesse. Das Projekt wurde mit C# und Windows Forms für .NET Framework 4.8 entwickelt.

## Funktionen

- Aufzeichnung des Arbeitsspeicherverbrauchs pro Prozess im Sekundenintervall.
- Tabellarische Auswertung mit Prozessname, PID, erster Erfassung und RAM-Statistiken.
- Diagramme zum Vergleich einzelner Prozessverläufe.
- Zusammenfassung gleichnamiger Prozesse und Anzeige eines Gesamtverlaufs.
- Suche nach Prozessnamen, Filter für Diagrammprozesse und RAM-Schwellenwertfilter. Die Filter können gemeinsam verwendet werden.
- Weiterlaufende Aufzeichnung bei Minimierung in den Infobereich der Taskleiste; Wiederherstellung und Beenden über das Tray-Menü.

## Voraussetzungen

Zum Ausführen:

- Windows mit .NET Framework 4.8.

Zum Entwickeln und Bauen:

- Visual Studio mit der Workload **.NET-Desktopentwicklung** und dem .NET Framework 4.8 Targeting Pack.
- Für die automatischen Tests: Windows PowerShell 5.1.

## Projekt bauen und starten

1. `RamInfo.sln` in Visual Studio öffnen.
2. Die Konfiguration **Debug** oder **Release** und die Plattform **Any CPU** oder **x64** wählen.
3. Die Projektmappe bauen und die Anwendung starten.

Alternativ in einer Developer PowerShell für Visual Studio aus dem Projektordner bauen:

```powershell
MSBuild.exe .\RamInfo.sln /t:Build /p:Configuration=Debug '/p:Platform=Any CPU'
```

Das Ergebnis liegt bei dieser Konfiguration unter `RamInfo/bin/Debug/RamInfo.exe`. Ein Release-Build für x64 liegt unter `RamInfo/bin/x64/Release/`.

## Bedienung

1. Mit **Start/Stop** die Aufzeichnung beginnen. Während der Messung heißt die Schaltfläche **Aufzeichnung Stoppen**.
2. Die Aufzeichnung stoppen, um die Statistik in der Tabelle anzuzeigen. Eine neue Aufzeichnung ersetzt die bisherigen Messdaten.
3. Mit **Zum Chart** einen Prozessverlauf ins Diagramm aufnehmen; mit **Im Chart** wieder entfernen.
4. Mit **Dublikate Addieren** gleichnamige Prozesse zusammenfassen und den Gesamtverlauf ergänzen. Erneutes Kombinieren baut die Ergebniszeilen neu auf, ohne die Rohdaten doppelt zu zählen.
5. Über das Suchfeld, **Nur ausgewählte Prozesse anzeigen** und den RAM-Schwellenwert die Tabelle filtern. Die Schaltfläche für den RAM-Filter heißt anfangs **Max Verbrauch**, filtert aber nach dem durchschnittlichen RAM-Verbrauch.

Die Messdaten bleiben im Arbeitsspeicher der Anwendung. Ein Datei-Export ist derzeit nicht implementiert.

## Messwerte und Statistik

Die Anwendung liest `Process.WorkingSet64`: den aktuell im physischen Arbeitsspeicher befindlichen Arbeitssatz eines Prozesses. Angezeigte Werte werden durch 1.048.576 geteilt; die Oberfläche bezeichnet diese Einheit als MB, rechnerisch handelt es sich um MiB.

| Spalte | Bedeutung |
| --- | --- |
| Prozess / PID | Erfasster Prozessname und Prozess-ID |
| Seit-start | Zeitpunkt der ersten Erfassung relativ zum Beginn der Aufzeichnung; kein tatsächlicher Betriebssystem-Startzeitpunkt |
| Durchschnitt (MB) | Arithmetischer Mittelwert der gespeicherten Werte |
| 1% Low (MB) | Mittelwert der niedrigsten 1 % der Werte, abgerundet auf eine ganze Anzahl, mindestens ein Wert |
| 1% High (MB) | Mittelwert der höchsten 1 % der Werte, abgerundet auf eine ganze Anzahl, mindestens ein Wert |

Kombinierte Verläufe berücksichtigen die jeweiligen Zeitpositionen der Messwerte. **RAM Gesamt** ist die Summe der erfassten Prozessarbeitssätze. Geteilte Speicherseiten können dabei mehrfach gezählt werden; die Summe entspricht deshalb nicht der gesamten physischen RAM-Belegung im Task-Manager.

## Automatische Tests

Nach einem Debug-Build aus dem Projektordner ausführen:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\Tests\Regression.ps1
```

Zum Prüfen eines Release-Builds für x64:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\Tests\Regression.ps1 -AssemblyPath .\RamInfo\bin\x64\Release\RamInfo.exe
```

Die Tests prüfen Gesamtsummen, Zeitpositionen, wiederholtes Kombinieren, die 1-%-Kennzahlen, unveränderte Rohdaten und das Zurücksetzen einer Aufzeichnung. Sie verwenden feste Testdaten und laufen nicht beim normalen Start der Anwendung. Bei Erfolg erscheint `PASS`, bei einem Fehler wird das Skript mit einer Fehlermeldung beendet. Die Oberfläche und die Live-Prozesserfassung werden dabei nicht automatisch getestet.

## Projektstruktur

```text
RamInfo/
├── RamInfo/
│   ├── RamInfo.csproj
│   ├── Program.cs
│   ├── RamForm.cs
│   ├── RamForm.Designer.cs
│   ├── RamForm.resx
│   ├── Logic/
│   │   ├── RamDataModel.cs
│   │   ├── RamRecorder.cs
│   │   ├── RamStatistics.cs
│   │   └── RamChartManager.cs
│   └── Properties/
├── README.md
└── RamInfo.sln

```

`RamRecorder` erfasst Prozesse im Hintergrund und übernimmt vollständige Messungen anschließend auf dem aufrufenden Kontext. `RamDataModel` hält die Messdaten, `RamStatistics` berechnet Statistiken und Summen, und `RamChartManager` verwaltet die Diagrammserien. `RamForm` steuert die Oberfläche, Aufzeichnungen und Filter.

### Orientierung im Code

- `RamForm.StartRecording` setzt die Auswertung zurück; `StopRecordingAsync` wartet vor der Auswertung auf die letzte Messung.
- `RamRecorder.CaptureSnapshot` liest Prozesse aus; `TickAsync` ordnet die Messwerte ihrer Aufzeichnungssekunde zu.
- `RamStatistics.AddToTimeline` richtet Summen zeitlich aus. Die Kennzahlenberechnung sortiert eine Kopie und erhält die Reihenfolge der Rohdaten.
- Kurze deutsche XML-Kommentare beschreiben die Klassen und zentralen Methoden direkt im Editor.

## Einschränkungen

- Prozesse, die zwischen zwei Messungen starten und enden, können unbemerkt bleiben. Sehr kurze RAM-Spitzen werden möglicherweise nicht erfasst.
- Nicht zugängliche oder bereits beendete Prozesse werden bei der Abfrage übersprungen. Lücken vor einem späteren Messwert werden mit null aufgefüllt und können die Statistik beeinflussen.
- Die Messung verwendet Prozess-IDs als Schlüssel. Eine Wiederverwendung derselben PID während einer langen Aufzeichnung wird nicht als neuer Prozess unterschieden.
- Messdaten wachsen mit der Dauer der Aufzeichnung; eine feste Speicherbegrenzung ist nicht implementiert.

## Hinweise zum Hochladen

Für ein Quellcode-Repository die Projektmappe, Quelltexte, Ressourcen, Konfigurationsdateien, diese README und den Tests-Ordner aufnehmen. Lokale Visual-Studio-Dateien (`.vs/`), Build-Ausgaben (`bin/`, `obj/`) und den lokalen Ordner `BackUp/` nicht mit hochladen.

Eine Lizenzdatei ist derzeit nicht enthalten. Vor einer Veröffentlichung zur freien Weiterverwendung sollte eine passende Lizenz ergänzt werden.
