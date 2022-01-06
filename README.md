Implementirati Publish-Subscribe model u okviru koga PubSubEngine servis može da opslužuje
proizvoljan broj klijenata. Postoje dva tipa klijenata: 1) Publisher-i 2) Subscriber-i, a
komponente se međusobno autentifikuju koristeći sertifikate (tip validacije je custom). Custom
validacija u ovom zadatku podrazumeva sledeće: PubSubEngine da ima svoj self-signed
certificate (CN=PubSubEngine), od koga se generišu sertifikati za Publishere i Subscribere.
PubSubEngine prilikom validacije proverava da li je klijentski sertifikat istekao i da li je njegov
sertifikat izdavalac sertifikata. Klijentska validacija podrazumeva proveru da li je sertifikat
istekao, da li je self-signed i da li CN odgovarajući.
Prilikom startovanja, Publisher se prijavljuje na PubSubEngine, pritom definišući topic koji
objavljuje. S druge strane, prilikom startovanja Subscriber definiše topic-e na koje se pretplaćuje.
Topic je definisan podatkom tipa Alarm, koji sadrži sledeće informacije 1) kada je alarm
izgenerisan 2) poruka o alarmu - definisati standardne poruke u okviru resursnog fajla 3) rizik –
predstavljen brojem u opsegu 1-100. Subscriberi se pretplaćuju na alarme u zavisnosti od nivoa
rizika (npr. rizik 1-10, od 11-30, itd.). Prilikom gašenja, Subscriber se odjavljuje sa topica na koji
se pretplatio. Prilikom dodavanja/uklanjanja Publisher-a, svi Subscriberi treba da budu
obavešteni kako bi se eventualno pretplatili na nove Publishere.
Sve poruke u sistemu treba da budu kriptovane AES algoritmom u ECB modu.
Na unapred definisani period vremena (koji se definiše prilikom instanciranja), Publisheri
objavljuju novi podatak, zajedno sa digitalnim potpisom. Pre bilo kakve obrade, svaki subscriber
verifikuje da li je pristigla poruka validna. Ukoliko jeste validna, subscriber smešta podatak
(informacije o alarmima) u internu bazu podataka (tekstualni fajl), bez digitalnog potpisa.
Dodatno, u okviru custom Windows Event Loga Subscriberi loguju informaciju da je novi
podatak upisan u bazu podataka. Log treba da obuhvati sledeće informacije: Timestamp, Ime
baze podataka (tekstualnog fajla), ID entiteta u bazi, digitalni potpis i javni ključ publishera
podatka.