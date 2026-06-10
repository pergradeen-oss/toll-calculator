# Toll Fee Calculator

En kalkylator för trängselskatt med C#-implementation, enhetstester och fixad produktionslogik.

Ursprungligt uppdrag från [EvolveTechnology/toll-calculator](https://github.com/EvolveTechnology/toll-calculator). Denna fork innehåller en komplett omskriven C#-lösning.

## Snabbstart

```bash
cd C#
dotnet test
```

## Dokumentation

Se [C#/README.md](C#/README.md) för:

- Affärsregler och tarifftabell
- Projektstruktur och arkitektur
- Användningsexempel
- Testinstruktioner

## Lösning

| Område | Status |
|--------|--------|
| Korrekt rullande timdebitering | Fixad |
| Halvöppna tariffintervall | Implementerad |
| Kalenderdags-tak 60 SEK | Implementerad |
| Dynamiska helgdagar (alla år) | Implementerad |
| Sekundtrunkering i tariff | Implementerad |
| Enhetstester (70 st) | Inkluderade |
| Testbar arkitektur | Policies + DI |

## Bonus

Gifen i det ursprungliga uppdraget kommer från filmen *Hackers* (1995).
