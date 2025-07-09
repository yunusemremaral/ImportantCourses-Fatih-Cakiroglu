$(document).ready(function () {

    const BroadcastMessageToAllClientHubMethod = "BroadcastMessageToAllClient";
    const ReceiveMessageForAllClientClientMethodCall = "ReceiveMessageForAllClient";

    const BroadcastMessageToCallerClient = "BroadcastMessageToCallerClient"
    const ReceiveMessageForCallerClient = "ReceiveMessageForCallerClient"

    const ReceiveConnectedClientCount = "ReceiveConnectedClientCount";
    const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub")
        .configureLogging(signalR.LogLevel.Information).build();

    function start() {
        connection.start().then(() => console.log("Hub ile baglantı kuruldu"));

    }

    try {
        start();
    }
    catch {
        setTimeout(() => start(), 5000)
    }

    connection.on(ReceiveMessageForAllClientClientMethodCall, (message) => {
        console.log("gelen mesaj",message);
    })

    var span_client_Coutn = $("#connected-client-count");

    connection.on(ReceiveConnectedClientCount, (count) => {
        span_client_Coutn.text(count);
        console.log("Connect Client Count", count);
    })

   

    $("#btn-send-message-all-client").click(function () {
        const message = "All Hello";

        connection.invoke(BroadcastMessageToAllClientHubMethod, message).catch(err => console.error("hata=error",err))
    })



    $("#btn-send-message-caller-client").click(function () {
        const message = "Caller";

        connection.invoke(BroadcastMessageToCallerClient, message).catch(err => console.error("hata=error", err))
    })
    connection.on(ReceiveMessageForCallerClient, (message) => {
        console.log("gelen caller  mesaj", message);
    })
})