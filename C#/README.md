# Toll Fee Calculator (C#)

En produktionsklar kalkylator för trängselskatt, skriven i C# med tydlig tarifflogik, testbar arkitektur och omfattande enhetstester.

## Krav

- [.NET 8 SDK](https://dotnet.microsoft.com/download) eller senare

## Kom igång

```bash
cd C#
dotnet restore
dotnet build
dotnet test
```

## Projektstruktur

```
C#/
├── TollFeeCalculator.sln
├── src/TollFeeCalculator/
│   ├── TollCalculator.cs
│   ├── Policies/
│   │   ├── ITollFeePolicy.cs
│   │   ├── TollFeePolicy.cs
│   │   ├── IVehicleExemptionPolicy.cs
│   │   ├── VehicleExemptionPolicy.cs
│   │   ├── IHolidayCalendar.cs
│   │   └── SwedishHolidayCalendar.cs
│   └── Vehicles/
│       ├── IVehicle.cs
│       ├── VehicleType.cs
│       ├── Car.cs
│       ├── Motorbike.cs
│       └── GenericVehicle.cs
└── tests/TollFeeCalculator.Tests/
```

## Affärsregler


| Regel                                     | Implementation                                   |
| ----------------------------------------- | ------------------------------------------------ |
| Avgift 8–18 SEK beroende på tid           | `TollFeePolicy` med halvöppna intervall          |
| Max 60 SEK per kalenderdag                | Separat tak per datum, nollställs vid ny dag    |
| En avgift per rullande 60-minutersfönster | Gruppering med tidsdiff och omstart av intervall |
| Tider avrundas till hela sekunder         | Millisekunder trunkeras i `TollFeePolicy`        |
| Högsta avgift inom samma fönster          | `Math.Max` per intervall                         |
| Avgiftsfria fordon                        | `VehicleExemptionPolicy`                         |
| Helger och helgdagar avgiftsfria          | `SwedishHolidayCalendar`                         |


### Tariff (vardagar)


| Tid         | Avgift (SEK) |
| ----------- | ------------ |
| 06:00–06:29 | 8            |
| 06:30–06:59 | 13           |
| 07:00–07:59 | 18           |
| 08:00–08:29 | 13           |
| 08:30–14:59 | 8            |
| 15:00–15:29 | 13           |
| 15:30–16:59 | 18           |
| 17:00–17:59 | 13           |
| 18:00–18:29 | 8            |
| Övrig tid   | 0            |


### Avgiftsfria fordon

Motorbike, Tractor, Emergency, Diplomat, Foreign, Military

## Användning

```csharp
using TollFeeCalculator;
using TollFeeCalculator.Vehicles;

var calculator = new TollCalculator();
var car = new Car();

var passes = new[]
{
    new DateTime(2013, 3, 18, 7, 0, 0),
    new DateTime(2013, 3, 18, 7, 30, 0),
    new DateTime(2013, 3, 18, 8, 30, 0),
};

var totalFee = calculator.GetTollFee(car, passes); // 26 SEK
```

## Arkitektur

Lösningen är uppdelad i små, testbara komponenter:

- `**ITollFeePolicy**` — tariff per tidpunkt
- `**IVehicleExemptionPolicy**` — undantag per fordonstyp
- `**IHolidayCalendar**` — helger och helgdagar
- `**TollCalculator**` — orkestrerar dagsberäkningen

Policies kan injiceras för testning och framtida regeländringar utan att ändra kärnlogiken.

## Tester

Testsviten täcker:

- Alla tariffzoner med DataRow-liknande testfall per zongräns
- Timfönster och högsta avgift inom intervall
- Dagstak på 60 SEK per kalenderdag
- Halvöppna tariffintervall och avrundning till hela sekunder
- Avgiftsfria fordon och helgdagar
- Att hela juli inte är avgiftsfritt (tidigare bugg)
- Tom array, null-validering och osorterade passager

Kör tester:

```bash
dotnet test --verbosity normal
```

## Förbättringar jämfört med ursprunglig kod

- Korrekt tidsberäkning (`TotalMinutes` i stället för `Millisecond`)
- `intervalStart` uppdateras när ett nytt 60-minutersfönster börjar
- Komplett tariff utan luckor mellan 08:30 och 14:59
- Helgdagar per datum i stället för hela juli
- Dynamisk helgdagsberäkning för valfritt år (påsk, midsommar, alla helgons dag)
- Typ-säkra fordon med `VehicleType`-enum
- Separata policies för testbarhet och underhåll

## Bonus

Gifen i det ursprungliga README:t kommer från filmen *Hackers* (1995).