# Simulationsübericht

Wilkommen in der Hauptansicht des Programms. Dies ist die Zentrale Übersicht für die Simulation des BB84 Protokolls. Hier können Rollen ausgewählt werden und eine Testnachricht versendet, geknackt und empfangen werden.
Die folgenden Auswahlmöglichkeiten stehen zur Verfügung:

## Top Bar (Obererbereich):

### Zurück-Pfeil(Links)

Bricht die Simulation ab und wechselt zurück ins Hauptmenü. Alle bisher eingegebenen Daten gehen verloren.

### Name(Mitte)

Gibt den Namen der aktuellen Oberfläche an.

### Dark/Light Mode(Rechts)

Invertiert das Farbtheme der Anwendung. Wenn es Hell ist wird die Anwendunf dunkel und umgekehrt

## Buttons

### Alice

Durch drücken diese Buttons gelangen Sie nach einer Passwort Eingabe in die Oberfläche der Alice Rolle.

### Bob

Durch drücken diese Buttons gelangen Sie nach einer Passwort Eingabe in die Oberfläche der Bob Rolle.

### Eve

Durch drücken diese Buttons gelangen Sie nach einer Passwort Eingabe in die Oberfläche der Eve Rolle.

### Nachricht versenden

Durch drücken diese Buttons gelangen Sie nach einer Passwort Eingabe in die Oberfläche mit der Alice eine Nachricht verschlüsseln und versenden kann. Das Passwort ist das gleiche wie für Alice. Der Button ist erst aktiv, wenn die Schlüsselerzeugung bei Alice abgeschlossen ist.

### Nachricht knacken

Durch drücken diese Buttons gelangen Sie nach einer Passwort Eingabe in die Oberfläche mit der Eve versucht die von Alice versendete Nachricht zu entschlüsseln. Das Passwort ist das gleiche wie für Eve. Der Button ist erst aktiv, wenn Eve alle Photonen gemessen hat.

### Nachricht empfangen

Durch drücken diese Buttons gelangen Sie nach einer Passwort Eingabe in die Oberfläche mit der Bob versucht die von Alice versendete Nachricht zu entschlüsseln. Das Passwort ist das gleiche wie für Bob. Der Button ist erst aktiv, wenn die Schlüsselerzeugung bei Bob abgeschlossen ist.


## Regulärer Protokollablauf

1. Alice versendet Photonen.

2. Bob und Eve messen die Photonen.

3. Bob sendet die Polarisationen mit denen er gemessen hat an Alice und Eve hört mit.

4. Alice vergleicht die empfangenen Polarisationen mit den Polaristionen der gesendeten Photonen.

5. Alice markiert die übereinstimmenden Polarisationen.

6. Alice sendet die übereinstimmenden Polarisationen an Bob und Eve hört mit.

7. Die Datenbits aller Übereinstimmungen werden in den PreKey übernommen.

8. Bob wählt einige PreKey-Datenbits aus und sendet diese Alice, Eve hört mit.

9. Alice vergleicht die empfangenen PreKey-Datenbits mit ihren Eigenen und markiert Übereinstimmungen.

10. Alice sendet Bob welche PreKey-Datenbist übereingestimmt haben, Eve hört mit.

11. Die nicht zum vergleich verwendeten PreKey-Datenbits ergeben den FinalKey.


# Zusätzliches Lernmaterial zum BB84 Protokol

## Wikipedia artikel zum BB-84 Protokoll:

https://en.wikipedia.org/wiki/BB84