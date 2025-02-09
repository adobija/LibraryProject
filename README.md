\bin\Debug\net8.0\*bin files from root folder*

# Library_project

Wymagania dotyczące projektu<br>
-Projekt jest oparty na programowaniu obiektowym i implementuje dziedziczenie, klasy abstrakcyjne, interfejsy, kompozycje (Obowiązkowo)
 
-Program spełnia zasady SOLID oraz jego struktura oparta jest na wybranym wzorcu projektowym (Obowiązkowo)
 
-Program zawiera obsługę wyjątków (Zalecane)
 
-Praktyczne zastosowanie wybranych kolekcji: List, Dictionary, Stack itd. (Obowiązkowo)
 
-Zastosowanie LINQ do przeszukiwania zaimplementowanych kolekcji (Obowiązkowo)
 
-Zapis i odczyt danych z plików tekstowych, serializacja i deserializacja. Delegaty (Zalecane)
 
-Wykorzystanie wielowątkowości z zastosowaniem obiektów Task, klasy Parrallel oraz wykorzystującego programowanie asynchroniczne  (Zalecane)
 
-Tworzenie grafiki bitmapowej z wykorzystaniem wybranej biblioteki oraz wykorzystanie jej do graficznej prezentacji wyników uprzednio opracowanych zapytań LINQ (Opcjonalne)
 

 # Description 
 Olek: <br>
 <del>Administrator tworzenie ksiazek, </del><br>
 <del>usuwanie ksiazek,</del> <br>
 <del>Mozliwosc tworzenia kont uzytkownikow zapisywanych w osobnym pliku <del>JSON</del> txt (temporarily) ( login haslo)</del><br>
 <del>menu</del>
 <br>
<br>
 Jakub: <br>
 <del>przegladac ksiazki,</del> <br>
 <del>statystyki ksaizek,</del><br>
  <del>Generowanie statystyk - Bitmap</del><br>
 <del>wypozyczac,</del><br>
 <del>zwracac,</del><br>
  <del>Panel wypozyczen</del><br>
 <del>wyswietlanie kiedy ksiazka została wypozyczona</del><br>
<br>

- bazy danych ksiazek jako osobny plik JSON, podział na kategorie, 
- oparta na wzorcu projektowym Factory
- Mozliwosc tworzenia kont uzytkownikow zapisywanych w osobnym pliku txt,1 admin (login haslo)
- Administrator tworzenie ksiazek, usuwanie ksiazek (-Olek), zarzadzanie userami, statystyki ksiazek [ Zapisuj dane o najczęściej wypożyczanych książkach i prezentuj je za pomocą grafiki bitmapowej (np. wykres kołowy lub słupkowy).  ] , Śledzenie historii wypożyczeń, 
- user - przegladac ksiazki, statystyki ksaizek, wypozyczac, zwracac, wyswietlanie dni ile ksiazka jest wypozyczona, - JK
- zapis i odczyt danych o ksiazkach przechowywanych w JSON 
- 


# Update

- Utworzony plik book.json zawierajacy ksiazki
- Utworzony plik users.json zawierajacy userów
- w LibraryActions.cs - działa  Przeglądanie książek z podziałem na kategorie (opcja 1)

# Made by Jakub Kruźlak && Aleksander Dobija

