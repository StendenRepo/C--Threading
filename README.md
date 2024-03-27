# C# threading

## Opdrachtomschrijving

Het idee is om een app te ontwikkelen voor een chiptuningbedrijf, waarbij klanten kunnen zien hoeveel vermogenswinst ze kunnen behalen aan de hand van hun kenteken. De klant voert zijn kenteken in, waarna er een berekening wordt uitgevoerd om te bepalen hoeveel extra vermogen hij of zij kan krijgen. Wanneer de klant tevreden is, kan hij een offerte aanvragen. Deze wordt dan op de achtergrond gegenereerd. Ook worden er een of meerdere grafieken getoond om het verschil in pk en koppel te laten zien. Daarnaast is er de mogelijkheid om de en de getunede auto over het scherm te laten rijden met een timer, zodat je het verschil in snelheid kan zien voor en na het tunen.

Voor het genereren van de grafieken gebruiken we een threadpool
VOor de visuele race gebruiken we multi-threading
Voor de het ophalen van tuning data gebruiken we I/0 omdat we dit in een apart bestand stand zetten (fake data)
Voor het ophalen van data van de auto zelf gebruiken we een api met async await

## Requirements

**Must have**

- Ophalen van kenteken gegevens
- Berekenen van vermogenswinst
- Genereren offerte

**Should have**

- user interface

**Could have**

- vermogenswinst tonen in een grafiek
- Offerte automatisch naar klant mailen

**Wont have**

- Auto gegevens ophalen via merk/model/uitvoering
