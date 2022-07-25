Inhaltsverzeichnis
==========================
- [Inhaltsverzeichnis](#inhaltsverzeichnis)
- [Einleitung](#einleitung)
- [Architektur und Technologien der Anwendung](#architektur-und-technologien-der-anwendung)
  - [Nginx-Webserver](#nginx-webserver)
  - [Angular](#angular)
  - [ASP.NET](#aspnet)
  - [EntityFramework und die Datenbank](#entityframework-und-die-datenbank)
- [Bereitstellung](#bereitstellung)
  - [Bereitgestellte Komponenten](#bereitgestellte-komponenten)
  - [Carbonara-Server](#carbonara-server)
- [Erstmaliges Öffnen der Anwendung](#erstmaliges-öffnen-der-anwendung)


# Einleitung
Dies ist das Repository für die Carsharing-Anwendung "Carbonara". 
Sie besteht aus einem Frontend- und einem Backend-Anteil, wessen Technologien im folgenden erläutert werden. Anschließend findet sich eine kurze Erklärung zum Bereitstellen der Anwendung



# Architektur und Technologien der Anwendung
Nach Anforderung des Kunden handelt es sich bei Carbonara um eine Webanwendung, welche von Benutzern, also Kunden und Mitarbeitern gleichermaßen über einen Internet-Browser wie Chrome, Firefox oder Safari aufgerufen und bedient werden kann. Nach Absprache des Kunden wurde sich darauf geeinigt hierbei einen spezielleren Weg zu gehen: Durch Vorerfahrungen mit den betroffenen 
Technologien wurde sich entschieden eine s.g. Single-Page-Application (SPA) zu entwickeln, welche zum Darstellen von Informationen oder nach Nutzerinteraktionen Anfragen an einen verarbeitenden Server sendet. Im Folgenden wird dies weiter erläutert.

## Nginx-Webserver
Vom Benutzer aus gedacht ist die erste und einzige offensichtliche Berührung mit unserer Anwendung der Webserver im „Frontend“. Über eine sichere HTTPS-Verbindung können über verschiedene Anfragen Daten und Dateien des Servers erhalten werden. Dabei handelt es sich um den Webserver „NGINX“, welcher gleichzeitig auch als Reverse-Proxy dient. Da es für die Art der anfallenden Anfragen entscheidend ist, sollte noch einmal kurz erklärt werden was eine SPA ist: Der Name ist bereits
ein Hinweis darauf, dass zum Öffnen der Anwendung einmal eine einzige, dafür etwas größere Seite eingeladen wird. Nach dem Einlesen werden weitere statische Inhalte, wie Bilder und Styledateien eingeladen, wonach die Anwendung aufgebaut und benutzbar ist. Interagiert und navigiert der Nutzer nun innerhalb der Anwendung, wird innerhalb der Stammseite der Inhalt entsprechend mit den am
Anfang eingeladenen Daten ausgetauscht. Erst bei tatsächlicher inhaltlicher Interaktion mit dem System, bei dem dynamische Daten benötigt oder ein Nutzer Änderungen an Daten vornehmen möchte, muss weiter mit dem Server kommuniziert werden. Daraus ableitbar sind die beiden Anfragetypen, welche Nginx entgegennehmen muss: Statische Inhalte, wie die Webseite und ihre Ressourcen, die
sich nicht verändern, sowie dynamische Inhalte, die vom Server berechnet werden müssen. Erstere werden direkt beim Nginx gespeichert und sind meist durch Caching-Verfahren besonders schnell erreichbar. Eine Anfrage an die dynamischen Daten kann jedoch nicht selbst beantwortet werden. Hierbei dient die Reverse-Proxy-Funktion, mit der Nginx die Anfrage an die hier in der Grafik betitelte „REST API“ weiterleitet und darauf wartet dessen Antwort herauszugeben.

## Angular
[Angular](https://angular.io/) ist ein von Google entwickeltes Framework für Single-Page-Applications, welches hier für die Anwendung zum Einsatz kommt. Typisch wäre hierbei, dass neben der Beschreibung der Darstellung der Seiten mit HTML und CSS auch Javascript als defacto-Standard für dynamische Seitenprogrammierung im Internet genutzt würde. Doch hier hat man sich für [Typescript](https://www.typescriptlang.org/), einem typisierten Javascript (bzw. ECMAScript) Ableger, entschieden, der als Mischung aus Javascript, Java und C# bezeichnet werden könnte. Weiterhin ist Angular komponentenbasiert, wodurch alle darstellbaren Einzelteile der Anwendung für sich ihren Aufbau, ihr Aussehen und ihr Verhalten mit klar definierten Abhängigkeiten beschreiben. Eine Komponente kann außerdem durch diese Modularisierung selbst andere Komponenten in ihrer Darstellung einbinden. Als SPA besitzt Angular als Hauptseite auch eine Komponente, die nun aus der aufgerufenen URL mit vorher definierten Routen heraus entscheidet welche Komponente nun in ihr eingebunden wird. Als fertige Anwendung entsteht so ein komplexes, aber übersichtliches und mächtiges Gesamtsystem, welches nach den Wünschen der Kunden auch komplexe Anforderungen umsetzbar macht. Für das (kontinuierliche) Ausliefern werden dann alle Komponenten durch den Typescript-Compiler gebündelt und als einzelnes Modul mit wenigen Dateien in Javascript und HTML übersetzt, welches dann über den e.g. Nginx-Webserver als statischer Inhalt ausgeliefert werden kann.

## ASP.NET
Für dynamische Anfragen ist eine Komponente erforderlich, die diese entgegennimmt und verarbeitet. Bei Carbonara ist dies ein in C# programmiertes Backend, welches als „dotnet 6.0“-„ASP.NET“ Projekt mit seinen REST-Controllern eine einheitliche API bereitstellt. Intern nehmen die verschiedenen Controller diese Anfragen nach einer Autentifizierungs- und Autorisierungsprüfung entgegen und stellen Informationen über übergebene Parameter bereit. Entweder werden daraufhin bei simpleren Anfragen direkt Ergebnisse, bspw. aus der Datenbank, geliefert, oder es werden weiter Services aufgerufen, die ein Ergebnis berechnen und/oder Manipulationen an der Datenbank vornehmen. Letztendlich wird ein Ergebnis optionalerweise auch mit Inhalt, sowie einem Statuscode zurückgegeben,
welches dann vom Nginx an den Aufrufer als Antwort gesendet wird.

## EntityFramework und die Datenbank
Für die Kommunikation mit der Datenbank wird nicht direkt mit SQL-Befehlen im Programmcode gearbeitet. Für ein angenehmeres typisiertes Programmieren, um  Fehlerfälle zu minimieren und um eine SQL-Injection auszuschließen, wird ein sogenannter Object-Relational-Mapper verwendet, der aus vorher angegebenen Typen, ihren Attributen und Beziehungen sich automatisiert mit einer Datenbank verbindet und - wenn noch nicht vorhanden - selbstständig Tabellendefinitionen generiert, sowie anschließend anlegt. Mit [EntityFramework](https://docs.microsoft.com/en-us/aspnet/entity-framework), welches diese Aufgabe in Carbonara übernimmt, stehen bekannte semantische Strukturen aus C# bereit, mit denen komfortabel auf die Daten der Datenbank durch schnell zu erstellende und leicht lesbare Queries zugegriffen werden kann, die sowohl typisierte Parameter entgegennehmen als auch typisierte Antworten liefern. Für die Persistenz selbst wurde MariaDB als Datenbankmanagementsystem ausgewählt, da dieses als beliebter quelloffener Nachfolger des bewährten MySQL-Servers keine Lizenzen benötigt und gleichzeitig schnell und ausgereift ist.


# Bereitstellung
Um die Anwendung bereitzustellen wurde viel vorgearbeitet, sodass dies leicht möglich ist.

Folgende Voraussetzungen sollten erfüllt sein:
- Docker CLI ist installiert
- Docker-Compose ist installiert
- Der Docker-Daemon läuft
- Eine Internet-Verbindung zum Herunterladen der Basisimages besteht
- Eine Konsole ist verfügbar und geöffnet
- Das Repository wurde bereits heruntergeladen
- Genug Speicherplatz ist vorhanden
  
Befolgen Sie nun folgende Schritten **zum Starten** der Anwendung:

1. Öffnen Sie in der Konsole den Wurzelordner des Repositories
2. Navigieren Sie in den Ordner "Integration":
   
        cd Integration
3. Zum Bauen kann folgender Befehl ausgefüllt werden:

        docker-compose build
4. Führen Sie jetzt folgenden Befehl aus, um die Anwendung hochzufahren:
   
        docker-compose up -d -V

Beim Starten wird durch die angegebenen Parameter die Anwendung unabhängig von der Konsole und mit neuen Volumes gestartet. 


Befolgen Sie folgenden Schritt für das **Abschalten** der Anwendung:
1. Führen Sie folgenden Befehl im Ordner "Integration" aus:

        docker-compose down -v

Beim Beenden werden die angelegten Volumes wieder gelöscht. Sollte dies nicht erwünscht sein, kann das "-v" im letzten Schritt ausgelassen werden.

## Bereitgestellte Komponenten
Nach dem Start über Docker-Compose werden vier Container hochgefahren. Darunter die Datenbank "MariaDB" _(mariadb)_ und das dazugehörige Administrationstool "phpMyAdmin" _(pma)_, welches standardmäßig über Port 8081 erreichbar ist. Die Zugangsdaten hierfür lauten erstmal:

        Benutzer:       carbonara
        Passwort:       bpx04sYqIlcVbJRC
Anschließend wird die ASP.NET WebAPI unter dem Containernamen _webapi_ gestartet, welche die Datenbank initialisiert. Daraufhin startet der Nginx-Server _(angular)_, der die statischen Inhalte der Single-Page-Application bereitstellt, und Anfragen für das Backend entgegennimmt. Dieser ist über den Port 8080 erreichbar.


## Carbonara-Server
Die Anwendung wird laufend auf [www.car-bonara.de](https://www.car-bonara.de) bereitgestellt.


# Erstmaliges Öffnen der Anwendung
Standardmäßig öffnet sich die Anwendung

Nach dem Start befinden sich bereits Daten in der Datenbank. Darunter fällt auch ein Administrationskonto mit dem Namen "ENTWICKLER KONTO". Wie für jeden Mitarbeiter ist das Standardpasswort hierfür anfänglich "{Vorname}123" und sollte anschließend dringend geändert werden. Somit sollte sich nach dem erstmaligen Hochfahren der Anwendung mit dem folgenden Konto anmeldet werden:

        Email:          contact@car-bonara.de
        Passwort:       ENTWICKLER123

Nun können die Realdaten über die entsprechende Übersicht eingefügt werden. 

