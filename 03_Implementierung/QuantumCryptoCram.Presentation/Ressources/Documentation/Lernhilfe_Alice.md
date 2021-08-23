# Alice

Übersicht über die Rolle Alice. Sie sendet Photonen über einen Quantenkanal (Dargestellt durch eine rote Line). Diese werden verwendet um einen gemeinsamen Schlüssels mit Bob zu vereinbaren. Die Oberfläche ist unterteilt in vier Bereiche.

![Alice Oberfläche](./Ressources/Documentation/AliceObEr.png)

## Bereiche

### Topbar

In diesem Bereich wird durch das Programm navigiert.

#### Zurück-Pfeil(Links)

Wechselt zurück in die Simulationsübersicht.

#### Name(Mitte)

Gibt den Namen der aktuellen Oberfläche an.

#### Dark/Light Mode(Rechts)

Invertiert das Farbtheme der Anwendung. Wenn es Hell ist wird die Anwendunf dunkel und umgekehrt

### Photonen erzeugen

In diesem Bereich können Photonen erzeugt werden. Dies kann automatisch oder manuel erfolgen.

Manuelle erzeugung:

- Polarisation auswählen mit "X" oder "+"-Button
- Datenbit auswählen mit "0" oder "1"-Button
- Mit "Photonen übernehmen"-Butten wird das Photon erzeugt und im Notebook eingetragen

Automatische erzeugung:

- Eine belibige Anzahl an zu erzeugenenden Photonen kann im Textfeld angegeben werden
- Durch klick auf den "Zufällige Photonen übernehmen"-Button werden die Photonen  mit zufülligem Datenbit und Polarisation erzeugt und ins Notebook eingetragen

### Tabellenbereich

Hier werden alle Informationen die Alice zur Verfügung stehen gespeichert und angezeigt.

- ID: Identifikationsnummer des Photons
- Eigene Datenbits: Die Datenbits der erzeugten Photonen. Gesendete Photonen werden fett geschrieben.
- Eigene Polarisation: Die Polarisation der erzeugten Photonen. Gesendete Polarisation werden mit einem Pfeil markiert. Übereinstimmungen mit den von Bob erhaltenen Polarisationen werden farbig dargestellt:
    - Rot markiert: Keine Übereinstimmung
    - Grün markiert: Übereinstimmung
- Vorschau: Aktuell ungenutzt
- Polarisation des Partners: Die von Bob empfangenen Polarisation. Übereinstimmungen mit den eigenen Polarisationen werden farbig dargestellt:
    - Rot markiert: Keine Übereinstimmung
    - Grün markiert: Übereinstimmung
- Übereinstimmung Polarisationen: Hier kann markiert werden ob die Eigenen mit den Polarisationen des Partners übereinstimmen. Die Auswahl ist nur möglich wenn beide Polarisationen im Notebook eingetragen sind. Die Auswahl wird gesperrt wenn der Eintrag zum PreKey Vergleich verwendet wurde. Mit dem "Auto Check"-Button kann der Vergleich automatisch durchgeführt werden.
- Eigener PreKey: Hier werden die Datenbits des eigenen PreKeys angezeigt. Wenn sie für den PreKey Vergleich gesendet wurden, werden sie fett markiert.
- Eigene PreKey Auswahl: Hier werden die PreKey-Bits ausgewählt die zum Vergleich an den Partner gesendet werden. PreKey-Bits werden farblich markiert.
    - Rot markiert: Keine Übereinstimmung
    - Grün markiert: Übereinstimmung
- PreKey Auswahl des Partrners: Hier wird die Auswahl an PreKeys angezeigt und farblich markiert.
    - Rot markiert: Keine Übereinstimmung
    - Grün markiert: Übereinstimmung
- Übereinstimmung PreKey-Bits:  Hier kann markiert werden ob die Eigenen mit den PreKey-Bits des Partners übereinstimmen. Die Auswahl ist nur möglich wenn beide PreKeys im Notebook eingetragen sind. Mit dem "Auto Check"-Button kann der Vergleich automatisch durchgeführt werden.
- FinalKey: Zeigt die Datenbits des eigenen FinalKey an. Zum Vergleich verwendete PreKey-Datenbist werden nicht für den Finalkey verwendet.
- "Finaler Schlüssel fertig"-Button sperrt alle Interaktionen in der aktuellen Oberfläche. Er zeigt dem Programm an dass der Schlüssel fertig erstellt wurde. In der Simulationsübersicht wird der "Nachricht versenden"-Button freigeschalten

### Kanalberich

- "Photonen Senden"-Button: Sendet alle im Nootbook vorhandenen, noch nicht gesendeten Photonen in den Quantenkanal.
- "Polarisation senden"-Button: Sendet alle im Nootbook vorhandenen, noch nicht gesendeten Polaristionen über den öffentlichen Kanal.
- "Pol. Übereinstimmung senden"-Button: Sendet alle aktuell angewählten Polarisationsübereinstimmungen über den öffentlichen Kanal.
- "PreKey Auswahl senden"-Button: Sendet alle aktuell angewählten, noch nicht gesendeten PreKey-Datenbits über den öffentlichen Kanal.
- "PreKey Über. senden"-Button:  Sendet alle aktuell angewählten PreKey-Übereinstimmungen über den öffentlichen Kanal.