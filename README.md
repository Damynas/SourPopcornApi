# SourPopcornApi

<h2>SourPopcorn</h2>

1.	Sprendžiamo uždavinio aprašymas

    1.1.	Sistemos paskirtis

    Šios sistemos tikslas – suteikti naudotojui naują, patogią, vizualiai patrauklią ir tik nuo kitų kinomanų įvertinimų priklausančią platformą, skirtą kino filmų bei televizijos serialų paieškai, jų vertinimui, aptarimui. Tik šios sistemos naudotojai galės vertinti filmus, taip siekiant išgauti kuo tikslesnį, nuo kino kritikų nepriklausantį filmų ar serialų įvertinimą.
    
    Ši sistema suteiks galimybę diskutuoti apie mėgstamiausius filmus ar serialus su kitais naudotojais, padės geriau suprasti, kaip skiriasi kinomanų skonis. Tai bus puiki vieta filmų bei serialų entuziastams bendrauti ir dalintis savo nuomonėmis apie kino pasaulį, surasti naujų, dar nepažintų žanrų, praplėsti savo akiratį kino sferoje.


    1.2.	Funkciniai reikalavimai

    •	Visi naudotojai galės:
    1.	Peržiūrėti svetainės reprezentacinį puslapį;
    2.	Peržiūrėti filmų sąrašą;
    3.	Peržiūrėti konkretų filmą;
    4.	Atlikti paiešką pagal pasirinktus kriterijus.

    •	Neregistruotas sistemos naudotojas galės:
    1.	Susikurti paskyrą.

    •	Registruotas sistemos naudotojas galės:
    1.	Prisijungti prie svetainės;
    2.	Atsijungti nuo svetainės;
    3.	Peržiūrėti, redaguoti, šalinti savo paskyrą;
    4.	Įvertinti filmą;
    5.	Pakomentuoti filmą;
    6.	Įvertinti kitų naudotojų vertinimus.

    •	Moderatorius galės:
    1.	Kurti, redaguoti, šalinti filmus;
    2.	Ištrinti netinkamus vertinimus bei komentarus.

    •	Administratorius galės:
    1.	Peržiūrėti, kurti, redaguoti, šalinti vartotojus;
    2.	Kurti, redaguoti, šalinti filmus;
    3.	Ištrinti netinkamus vertinimus bei komentarus.

    •	Sistema galės:
    1.	Šifruoti slaptažodžius;
    2.	Autentifikuoti ir autorizuoti naudotojus.

2.	Sistemos architektūra

    •	Sistemos sudedamosios dalys:
    1.	Kliento pusė (ang. Front-End) – kuriama naudojant React.js biblioteką su TypeScript;
    2.	Serverio pusė (angl. Back-End) – kuriama naudojant .NET Core;
    3.	Duomenų bazė – PostgreSQL.
