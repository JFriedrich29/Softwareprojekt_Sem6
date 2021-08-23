# Ressourcen

In diesem Dokument sind diverse Ressourcen für den ganzen Entwicklungsablauf aufgelistet.

## Dokumente zur Kollaboration

- [UML Diagramm](https://lucid.app/lucidchart/95056fe8-7679-4f53-99aa-7481a3f3d7e2/edit?shared=true&page=0_0#)
- [Konzeptionelles Datenmodell](https://lucid.app/lucidchart/invitations/accept/inv_24cdf953-5d81-413e-ad19-3f347d3c2af7)
- [UI Mockup Lucidchart](https://lucid.app/lucidchart/invitations/accept/inv_dc9784f7-dee2-401e-b383-3e16b65e4d45)
- [UI Mockup Adobe](https://xd.adobe.com/view/0fe3e101-8b98-4ac0-9fd8-b45ed2d8f04f-5ab0/)
- [Fehlerfälle Beschreibung](https://othaw-my.sharepoint.com/:w:/g/personal/d44a_oth-aw_de/Ed7fqyIMNsFEngpJEu2XFAEB7sBXX6x7sdo12GwujCsn7g?rtime=ftW7Gg8E2Ug)

## Git

- [gitflow](https://www.atlassian.com/de/git/tutorials/comparing-workflows/gitflow-workflow)
- https://docs.gitlab.com/ee/user/project/issues/managing_issues.html#closing-issues-automatically
- https://chris.beams.io/posts/git-commit/

## Entwicklung

**WICHTIG: April 2021 Free Month Event von Pluralsight (Professionelle Lernplattform)!**

### Frontend

- [MaterialDesign](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
- [Tutorial für ein Custom Control für eine Tabelle](https://www.youtube.com/watch?v=jRG3d4ZPV5w)
- Referenzprojekt mit den verwendeten Technologien Stylet und MaterialDesign: https://github.com/Tyrrrz/YoutubeDownloader

### WPF MVVM

#### Pluralsight

- WPF MVVM in Depth (Pluralsight): https://app.pluralsight.com/library/courses/wpf-mvvm-in-depth/table-of-contents

#### Freie Alternativen

- Tim Corey: "WPF in C# with MVVM using Caliburn Micro" (youtube)

  https://www.youtube.com/watch?v=laPFq3Fhs8k

  Hier wird zwar ein depricated MVVM-Framework "Caliburn Micro" verwendet, allerdings sind die allgemeinen Konzepte, wie die ViewModels mit den Views zusammenspielen trotzdem sehr relevant. Wir verwenden als leightweight MVVM-Framework vorerst "Stylet" und wenn am Ende etwas Zeit bleibt implementieren wir unser Eigenes.
  Stylet, baut auf "Caliburn Micro" auf und behauptet von sich, dass es etwas intransparente "Magie" von Caliburn Micro entfernt und benutzerfreundlicher ist:
  https://github.com/canton7/Stylet/wiki

- https://stackoverflow.com/questions/15439841/mvvm-in-wpf-how-to-alert-viewmodel-of-changes-in-model-or-should-i

### Unit-Testing

#### Pluralsight

- Kurs 1 (Empfehlenswert): "Introduction to .NET Testing with NUnit 3"

    Wer das Test-Framework "NUnit3" näher kennen lernen will, welches wir verwenden.

    https://app.pluralsight.com/paths/skill/c-unit-testing-with-nunit


- Kurs 2 (Optional): Mocking with Moq and NUnit

    Hier wird erklärt, wie man Fake-Daten verwenden kann bzw, wie man Abhängigkeiten im Code (Datenbanken, Logger, ...) "simuliert/mockt",
    da man beim Testen diese nicht ansprechen und abstrahieren möchte.
    Wichtig wäre in dem Kurs nur die verschiedenen Grund-Konzepte von dem Mocking-Framework "MOQ" zu verstehen.
    Wir verwenden als Mocking-Framework "NSubstitute", welches die gleichen Konzepte besitzt, aber meiner Meinung nach eine deutlich intuitiver und einfachere Syntax verwendet:
    https://nsubstitute.github.io/help.html

- Kurs 7 (Optional Optional): Improving Unit Tests with Fluent Assertions

    Leichter zu lesende Assertions mit "FluentAssertions", was eigentlich fast standardmäßig bei Testprojekten anstatt den out-of-the-box Assertions von den bekannten Test-Frameworks verwendet wird.

#### Freie Alternativen


- Programming with Mosh: Unit Testing C# Code - Tutorial for Beginners

    https://www.youtube.com/watch?v=HYrXogLj7vg
    Hier werden sehr schön Grundlagen zum Unit-Testen erklärt.

- Tim Corey: Mocking in C# Unit Tests - How To Test Data Access Code and More

    https://www.youtube.com/watch?v=DwbYxP-etMY

- Nick Chapsas (Empfehlenswert): Clean mocking for unit tests using NSubstitute in .NET (Core, Framework, Standard)

    https://www.youtube.com/watch?v=LcQYv0cBWk0 Einführung in NSubstitute; Ignorieren das hier mit .NET Core und async Methoden gearbeitet wird

- Nick Chapsas: How to write cleaner unit tests with Fluent Assertions in .NET Core (Framework, Standard)

    https://www.youtube.com/watch?v=b2zxl5zNjlA

## VSCode

Erweiterungen für die (C#) Entwicklung:

- C#
    Mit settings.json:
    ```json
        //
        // ### C Sharp ###
        //
        "csharp.semanticHighlighting.enabled": true,
        "omnisharp.enableMsBuildLoadProjectsOnDemand": true,
        "omnisharp.disableMSBuildDiagnosticWarning": true,
        "omnisharp.enableRoslynAnalyzers": true,
        "omnisharp.enableEditorConfigSupport": true,
        "csharp.referencesCodeLens.enabled": true,
        "omnisharp.projectLoadTimeout": 2400,
        "omnisharp.enableImportCompletion": true,
    ```

- C# -EditorConfig for VS Code

- vscode-solution-explorer

- Easy Snippet

- Bracket Pair Colorizer

- Better Comments

- Git Graph

- GitHub Pull Requests and Issues

- GitLens

- GitHub Theme

- Material Icon Theme
    Mit den zwei Zeilen in der settings.json:
    ```json
    "material-icon-theme.folders.theme": "classic",
    "material-icon-theme.folders.color": "#e3a624",
    ```