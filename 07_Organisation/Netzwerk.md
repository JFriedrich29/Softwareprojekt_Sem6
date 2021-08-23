# Netzwerkfähigkeit

Aufgaben die erledigt werden müssen

- TCP-Abstraktion von Christoph anpassen: Infrastructure.TCPServer.cs, Infrastructure.TCPClient.cs + EventLoop Unklarheiten

- Server Logic: Infrastructure.BB84Server.cs

  - Weiterleiten der Quanten-Telegramme an die nächste Rolle
  - Weiterleiten der Public-Telegramme an alle anderen Rollen
  - Implementieren der QuantumPipe Interfaces
  - Implementierten des PublicNetwork Interface
  - Verwalten von RemoteRole-Objekte

- Client Logik:  Infrastructure.BB84Client.cs

  - Implementieren der QuantumPipe Interfaces
  - Implementierten des PublicNetwork Interface
  - Verbindungsaufbau zum Server

- Nachrichten

  - Telegramme für Quantenkanal und Public Channel serialisieren und deserialisieren: System.Runtime.Serialization.Json
  - Telegramme für Sitzungsbeginn mit Namen der ausgewählten Rolle
  - Fehlernachrichten: Rolle schon vorhanden, Andere Rolle disconnected

- GUI

  - Andere Rollen müssen im Rollenmenü als vergeben angezeigt werden
  - NetworkModeViewModel.cs
    - Option zum Hosten noch zusätzlich zum Verbinden
    - Steuern von RemoteSimulationManger
  - Fehlerdialoge bei Netzwerkfehlern
  - Wenn eine andere Rolle disconnected: Zurück zum Hauptmenü-Dialog

- RemoteSimulationManager:

  - Methode: StartHost(role, port): Erstellen eines BB84Server und ein Rollenobjekt
  - Methode: ConnectToHost(role, ip, port): Erstellen eines BB84Client und ein Rollenobjekt und Verbindung aufbauen
- Methode: StartSimulation()


## Offene Punkte

- Fehlermeldungen von Verbindungsfehlern
- Woher bekommen Teilnehmer die Information, welche anderen Teilnehmer vorhanden sind
- Unit-Testing
- Telegram Design


## Zeitabschätzung

| Komponente in chronologischer Reihenfolge  | Zeit |
| ------------------------------------------ | ---- |
| Telegram Design                            | 2h   |
| TCPServer.cs, TCPClient.cs                 | 4h   |
| Nachrichten                                | 2h   |
| BB84Server.cs                              | 3h   |
| BB84Client.cs                              | 2h   |
| Integrationstest Server Client + Bug Fixes | 6h   |
| GUI                                        | 8h   |
| RemoteSimulationManager                    | 3h   |
| Integrationstest + Bug Fixes               | 5h   |
| Review + Refactoring                       | 2h   |
| **Gesamt**                                 | 37h  |