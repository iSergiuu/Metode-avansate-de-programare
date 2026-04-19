# Tema Laborator 2 MAP - UML 

Aici sunt diagramele UML pentru sistemul de gestiune a comenzilor. Ideea principală a fost să analizez un cod scris destul de prost la început (cuplat puternic) și să propun un design mai curat pe baza principiilor pe care le-am discutat la curs.

## Fisiere si decizii luate:

**1. Diagrama Use Case (`docs/use-case.png`)**
Am identificat 3 actori: Clientul, Adminul și Sistemul de Plată (actor extern). Am legat acțiunile între ele folosind `include` pentru chestiile obligatorii (cum e plata și emailul la plasarea unei comenzi) și `extend` pentru pașii opționali (aplicarea unui voucher de reducere).

**2. Diagramele de Clasă**
* Pentru starea inițială (`docs/class-diagram-before.png`), am scos in evidenta problema majoră: clasa `ManagerComenzi` e un fel de "God Class" care face absolut tot, de la logica de afaceri pana la conexiuni cu BD sau API-uri. De asemenea, clasa `Comanda` era doar o cutie de date goala (anemic domain model).
* Pentru starea refactorizată (`docs/class-diagram-after.png`), am spart monolitul. Am introdus interfețe (`IRepoComenzi`, `IProcesarePlata`) ca sa decuplez logica de implementarile fizice, iar clasa `Comanda` si-a primit metodele ei (gen `calculeazaTotal()`) ca sa fie un model de domeniu cum trebuie.

**3. Diagramele de Secvență**
Astea sunt scrise direct în Mermaid ca să se randeze automat:
* `docs/sequence-place-order.md` - Aici e fluxul complet pentru o comanda noua. M-am folosit de un bloc `alt` ca sa tratez cazul in care nu sunt destule produse pe stoc, si de `opt` ca sa trimit mailul opțional la final.
* `docs/sequence-cancel-order.md` - Fluxul de anulare. Aici e prinsă restricția aia din cerință: am folosit `alt` ca sa blochez anularea daca statusul comenzii este deja "Expediata", altfel o lasa sa treaca.
