# KDR

## Architektoniczne pomysły/plany

* Publish realizuje zapis do bazy danych, oraz przekazanie wiadomości do wysłania od razu (w pamięci)
    * (+) Dzięki takiej realizacji unikamy kolejnego odczytu wiadomości z bazy (jak dużo?)
    * (+) Zdecydowanie mniej pracy wkładanej w realizacje poolingu z bazy (pozostałe dispatchery mogą działać rzadko, bo są tylko sposobem radzenia sobie z problemami a nie naturalnym działaniem serwisu)
    * (-) Tracimy miejsce na kolejny load balancing (ale dla samej akcji wysyłki wydaje się to być mniejszy problem)
    * (-) Musimy rozwiązać problem straty wiadomości inMemory na czas zamknięcia serwisu i ich odzyskanie po restarcie seriwsu
        * można po prostu zrobić osobny dispatcher który pobiera co jakiś czas wiadomości nie wysłane z bazy (istniejące już jakiś czas). Wciąż istnieje problem gdzie kolejka mogła być tak duża, że wiadomości nie zdążyły się wysłać (ale przy dobrze dobranym czasie nie powinno być dużym problemem, plus mówimy tylko o sytuacji gdzie serwis się wyłączył). Najgorsze co może się wydarzyć to podwójne wysłanie wiadomości, a to możemy rozwiązywać z koleji znów przy odbiorze wiadomości i sprawdzeniu czy nie ma takiej już odebranej.
    


## Rozważane użycia bibliotek 

* KDR
    * Microsoft.Logging.Abstractions
        * Sposób dostarczania logów w bibliotece
    * Microsoft.Dependency.Abstractions
        * Pozwoliłby na wykorzystanie scopów - dostarczanie potrzebnych zależności w dalszych częściach implementacji, w tym połączeń bazodanowych(sqlConnection, dbContext, czy nhibernate)
        * Pozwoliłby ułatwić kod przekazywania obiektów w wyższych warst do niższych
    * Newtonsoft.Json
    * System.Text.Json
        * Jakoś musimy serializować obiekty, System.Text.Json jest szybszy, ale też aktualnie mniej znany i wykorzystywany
* KDR.Persistence.SqlServer
    * Dapper
    * Microsoft.Logging.Abstractions
* KDR.Transport.ServiceBus
    * Microsoft.Logging.Abstractions
    * Microsoft.ServiceBus
    * Microsoft.WindowsAzure.Configuration