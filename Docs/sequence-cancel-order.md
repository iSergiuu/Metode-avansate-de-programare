&lt;pre&gt;&lt;code&gt;```mermaid
sequenceDiagram
    participant C as Client
    participant SC as ServiciuComenzi
    participant RC as IRepositoryComenzi

    C-&gt;&gt;SC: POST /comenzi/{id}/anulare
    SC-&gt;&gt;RC: incarcaComanda(id)
    RC--&gt;&gt;SC: date comanda (inclusiv status)
    
    alt comanda este deja &quot;Expediata&quot;
        SC--&gt;&gt;C: 400 Bad Request (Nu se poate anula)
    else comanda este &quot;Noua&quot; sau &quot;In procesare&quot;
        SC-&gt;&gt;SC: modificaStatus(&quot;Anulata&quot;)
        SC-&gt;&gt;RC: salveaza(comanda)
        RC--&gt;&gt;SC: confirmare actualizare DB
        SC--&gt;&gt;C: 200 OK (Comanda anulata cu succes)
    end
```&lt;/code&gt;&lt;/pre&gt;
