🏟️ FRESHKIT 🏆
Benvenuto/a a FRESHKIT! 👕 Un e-commerce dedicato ai fan delle maglie di calcio. ⚽ Qui potrai trovare le maglie delle squadre dei migliori campionati europei!

🌍 Descrizione del progetto
FRESHKIT è un sito e-commerce costruito in ASP.NET Core che permette agli utenti di navigare e acquistare maglie da calcio. Il sito consente la visualizzazione delle maglie in base ai diversi campionati come Serie A, Premier League, La Liga, Bundesliga e Ligue 1.

🎯 Funzionalità principali:
🛒 Sistema di carrello semplice per gestire gli acquisti
🏆 Visualizzazione delle maglie suddivise per campionato
👕 Personalizzazione delle maglie con nome e numero
📊 Pannello di amministrazione per la gestione delle maglie e degli utenti
🔐 Sistema di autenticazione con ruoli Admin e User


🚀 Istruzioni per il setup
Segui questi semplici passaggi per clonare e avviare FRESHKIT in locale:

1️⃣ Clona il repository
Per clonare il progetto esegui il seguente comando nel tuo terminale:


git clone https://github.com/Frattins/ProgettoFRESHKIT.git


2️⃣ Naviga nella cartella del progetto
Una volta clonato il repository, entra nella directory del progetto:


cd ProgettoFRESHKIT/JerseyShop


3️⃣ Configura il database
Prima di avviare il progetto, configura il database. Vai nel file appsettings.json e aggiorna la stringa di connessione con le credenziali del tuo database:


"ConnectionStrings": {
  "DefaultConnection": "InserisciIltuoDatabase;"
}


4️⃣ Applica le migrazioni
Per creare correttamente le tabelle nel database, esegui il seguente comando:


add-migration FreshKit

update-database


5️⃣ Avvia il progetto
Dopo aver configurato tutto, avvia il progetto con il comando cliccando su "https" 

👨‍💻 Autore
Progetto sviluppato da Davide Frattini come esame finale per il corso FullStack Developer di Epicode, con l'obiettivo di creare un e-commerce completo per i fan delle maglie di calcio! 🏅








