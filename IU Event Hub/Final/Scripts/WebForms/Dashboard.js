$(document).ready(function () {
    $("#cardAttendedevent1").click(function () {
        window.location.href = '/Pages/Event?EVENT=' + $('#MainContent_attendedEvent1')[0].textContent;
       
    });
    $("#cardAttendedevent2").click(function () {
        window.location.href = '/Pages/Event?EVENT=' + $('#MainContent_attendedEvent2')[0].textContent;

    });
    $("#cardAttendedevent3").click(function () {
        window.location.href = '/Pages/Event?EVENT=' + $('#MainContent_attendedEvent3')[0].textContent;

    });

    $("#cardBookedEvent1").click(function () {
        window.location.href = '/Pages/Event?EVENT=' + $('#MainContent_bookedEvent1')[0].textContent;

    });

    $("#cardBookedEvent2").click(function () {
        window.location.href = '/Pages/Event?EVENT=' + $('#MainContent_bookedEvent2')[0].textContent;

    });

    $("#cardBookedEvent3").click(function () {
        window.location.href = '/Pages/Event?EVENT=' + $('#MainContent_bookedEvent3')[0].textContent;

    });
})
