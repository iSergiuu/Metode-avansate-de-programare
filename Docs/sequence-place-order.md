<pre><code>```mermaid
sequenceDiagram
participant C as Client
participant SC as ServiciuComenzi
participant VS as IVerificareStoc
participant P as IProcesarePlata
participant RC as IRepositoryComenzi
participant E as IServiciuEmail

C-&gt;&gt;SC: POST /comenzi (date client, produse)
SC-&gt;&gt;VS: verificaStoc(produse)

alt stoc insuficient
    VS--&gt;&gt;SC: false
    SC--&gt;&gt;C: 400 Bad Request (Stoc epuizat)
else stoc ok
    VS--&gt;&gt;SC: true
    SC-&gt;&gt;P: proceseazaPlata(suma)
    P--&gt;&gt;SC: confirmare plata
    
    SC-&gt;&gt;RC: salveaza(comanda)
    RC--&gt;&gt;SC: comanda salvata
    
    opt trimitere email confirmare
        SC-&gt;&gt;E: trimiteEmail(client.email)
        E--&gt;&gt;SC: ok
    end
    
    SC--&gt;&gt;C: 201 Created (Comanda plasata)
end
