$(document).ready(function () {

    // Hub metotları ve client event isimleri
    const BroadcastMessageToAllClientHubMethod = "BroadcastMessageToAllClient";
    const ReceiveMessageForAllClientClientMethodCall = "ReceiveMessageForAllClient";

    const BroadcastMessageToCallerClient = "BroadcastMessageToCallerClient";
    const ReceiveMessageForCallerClient = "ReceiveMessageForCallerClient";

    const BroadcastMessageToOtherClient = "BroadcastMessageToOtherClient";
    const ReceiveMessageForOtherClient = "ReceiveMessageForOtherClient";

    const BroadcastMessageToSpesificClient = "BroadcastMessageToSpesificClient";
    const ReceiveMessageForSpesificClient = "ReceiveMessageForSpesificClient";

    const JoinGroup = "JoinGroup";
    const LeaveGroup = "LeaveGroup";
    const BroadcastMessageToGroup = "BroadcastMessageToGroup";
    const ReceiveMessageForGroup = "ReceiveMessageForGroup";

    const ReceiveConnectedClientCount = "ReceiveConnectedClientCount";

    // SignalR bağlantısı oluşturma
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/exampleTypeSafeHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    // Bağlantıyı başlatan fonksiyon
    function startConnection() {
        connection.start()
            .then(() => {
                console.log("Hub ile bağlantı kuruldu");
                $("#span-connectionid").html(`Connection Id: ${connection.connectionId}`);

                // Gruptan çık ve tekrar gruba katılma örneği (opsiyonel, kaldırabilirsin)
                connection.invoke(LeaveGroup, "grup-adi")
                    .catch(err => console.warn("Gruptan çıkarken hata:", err));

                connection.invoke(JoinGroup, "grup-adi")
                    .catch(err => console.warn("Gruba katılırken hata:", err));

                // Grupta mesaj gönder (örnek)
                connection.invoke(BroadcastMessageToGroup, "grup-adi", "Merhaba Grup!")
                    .catch(err => console.error("Gruba mesaj gönderme hatası:", err));
            })
            .catch(err => {
                console.error("Bağlantı kurulamadı, tekrar deneniyor...", err);
                setTimeout(() => startConnection(), 5000);
            });
    }

    // İlk bağlantı başlatılıyor
    startConnection();

    // Bağlı client sayısı span elemanı
    const $connectedClientCount = $("#connected-client-count");

    // Client sayısı güncellemesi dinleniyor
    connection.on(ReceiveConnectedClientCount, (count) => {
        $connectedClientCount.text(count);
        console.log("Bağlı Client Sayısı:", count);
    });

    // Tüm clientlara mesaj gönder butonu
    $("#btn-send-message-all-client").click(() => {
        const message = "All Hello";

        connection.invoke(BroadcastMessageToAllClientHubMethod, message)
            .catch(err => console.error("Tüm clientlara mesaj gönderme hatası:", err));
    });

    // Tüm clientlardan mesaj dinle
    connection.on(ReceiveMessageForAllClientClientMethodCall, (message) => {
        console.log("Tüm clientlardan gelen mesaj:", message);
    });

    // Diğer clientlara mesaj gönder butonu
    $("#btn-send-message-other-client").click(() => {
        const message = "Other Clients Hello";

        connection.invoke(BroadcastMessageToOtherClient, message)
            .catch(err => console.error("Diğer clientlara mesaj gönderme hatası:", err));
    });

    // Diğer clientlardan mesaj dinle
    connection.on(ReceiveMessageForOtherClient, (message) => {
        console.log("Diğer clientlardan gelen mesaj:", message);
    });

    // İstek yapan client'a mesaj gönder butonu
    $("#btn-send-message-caller-client").click(() => {
        const message = "Caller";

        connection.invoke(BroadcastMessageToCallerClient, message)
            .catch(err => console.error("Caller client mesaj gönderme hatası:", err));
    });

    // Caller client mesaj dinleme
    connection.on(ReceiveMessageForCallerClient, (message) => {
        console.log("Caller client'tan gelen mesaj:", message);
    });

    // Spesifik client'a mesaj gönder butonu
    $("#btn-send-message-spesific-client").click(() => {
        const targetConnectionId = $("#input-connectionid").val().trim();
        const message = "Spesifik client mesajı!";

        if (!targetConnectionId) {
            alert("Lütfen bir Connection ID girin.");
            return;
        }

        connection.invoke(BroadcastMessageToSpesificClient, targetConnectionId, message)
            .then(() => console.log("Mesaj spesifik client'a gönderildi:", targetConnectionId))
            .catch(err => console.error("Spesifik client mesaj gönderim hatası:", err));
    });

    // Spesifik client mesajlarını dinle
    connection.on(ReceiveMessageForSpesificClient, (message) => {
        console.log("Spesifik client mesajı alındı:", message);
    });

    // Grup mesajlarını dinle
    connection.on(ReceiveMessageForGroup, (message) => {
        console.log("Grup mesajı alındı:", message);
    });
    // Grup ile ilgili elementler
    const $inputGroupName = $("#input-groupname");

    // Gruba katılma
    $("#btn-join-group").click(() => {
        const groupName = $inputGroupName.val().trim();
        if (!groupName) {
            alert("Lütfen bir grup adı girin.");
            return;
        }

        connection.invoke(JoinGroup, groupName)
            .then(() => console.log(`"${groupName}" grubuna katıldı.`))
            .catch(err => console.error("Gruba katılma hatası:", err));
    });

    // Gruptan çıkma
    $("#btn-leave-group").click(() => {
        const groupName = $inputGroupName.val().trim();
        if (!groupName) {
            alert("Lütfen bir grup adı girin.");
            return;
        }

        connection.invoke(LeaveGroup, groupName)
            .then(() => console.log(`"${groupName}" grubundan çıktı.`))
            .catch(err => console.error("Gruptan çıkma hatası:", err));
    });

    // Gruba mesaj gönderme
    $("#btn-send-message-group").click(() => {
        const groupName = $inputGroupName.val().trim();
        if (!groupName) {
            alert("Lütfen bir grup adı girin.");
            return;
        }

        const message = prompt(`"${groupName}" grubuna gönderilecek mesajı girin:`);

        if (message) {
            connection.invoke(BroadcastMessageToGroup, groupName, message)
                .then(() => console.log(`"${groupName}" grubuna mesaj gönderildi.`))
                .catch(err => console.error("Gruba mesaj gönderme hatası:", err));
        }
    });

});
