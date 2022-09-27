let chosenSeatsIds = [];

function loadEventAreaDescriptionWithSeats(id) {
    let areaChecked = document.getElementsByClassName("event-area-checked").item(0);

    if (areaChecked != null) {
        areaChecked.className = "event-area-unchecked";
    }
    document.getElementById('eventArea_' + id).className = "event-area-checked";

    $.ajax({
        type: 'GET',
        url: '/EventSeat/Index/',
        data: {
            eventAreaId: id
        },
        success: function (response) {
            console.log('success!');
            $('#eventSeatsContainer').html(response);
        }
    });
}

function checkSeat(id) {
    let seat = document.getElementById(id);
    seat.className = seat.className.includes("event-seat-chosen") ? "event-seat-free" : "event-seat-chosen";
}

function buyTickets() {
    let form = $('#__AjaxAntiForgeryForm');
    let token = $('input[name="__RequestVerificationToken"]', form).val();

    getSeats();

    $.ajax({
        type: 'POST',
        url: '/Order/Create/',
        traditional: true,
        dataType: 'json',
        data: {
            __RequestVerificationToken: token,
            eventSeatsIds: chosenSeatsIds
        },
        success: function (response) {
            console.log('success!');
            clearSeats();
            $('#eventSeatsResultContainer').html(response);
        },
        error: function (response) {
            clearSeats();
            $('#eventSeatsResultContainer').html(response.responseText);
        } 
    });
}

function clearSeats() {
    chosenSeatsIds.forEach(el => {
        checkSeat('eventSeat_' + el);
    });
}

function getSeats() {
    let seats = document.getElementsByClassName("event-seat-chosen");

    chosenSeatsIds = [];

    for (const seat of seats) {
        let pureId = Number(seat.id.split('_')[1]);
        chosenSeatsIds.push(pureId);
    };
}