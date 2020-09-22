"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/NotifySticker").build();

connection.on("ClinetNotifySticker", function (message) {

    document.getElementById("log").append(message + "\r");
});

connection.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});
