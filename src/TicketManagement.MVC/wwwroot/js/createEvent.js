//window.onload = function () {
//    changeVenueTimeZone();
//    getLayouts();
//};

function selectLayout(id, valueToSelect) {
    let element = document.getElementById(id);
    element.value = valueToSelect;
}

function removeAllSelectBoxOptions(selectBox) {
    while (selectBox.options.length > 0) {
        selectBox.remove(0);
    }
}

function onVenueSelectChange() {
    getLayouts();
    changeVenueTimeZone();
}

function onLayoutSelectChange() {
    getAreas();
}

function changeVenueTimeZone() {
    let id = document.getElementById("VenueId").value;
    let element = document.getElementById("venueTimeZoneSelectId");
    element.value = id;
} 


//function getLayouts() {
//    let form = $('#__AjaxAntiForgeryForm');
//    let token = $('input[name="__RequestVerificationToken"]', form).val();

//    let id = document.getElementById("VenueId").value;

//    $.ajax({
//        type: 'GET',
//        url: '/Layout/GetAll',
//        data: {
//            venueId: id
//        },
//        success: function (response) {
//            console.log('success!');
//            let selectBox = document.getElementById("LayoutId");
//            removeAllSelectBoxOptions(selectBox);
//            $.each(response.value,
//                function (idx, el) {
//                    let newOption = new Option(el.description, el.id);
//                    selectBox.add(newOption, undefined);
//                });

//            getAreas();
//        }
//    });
//}

function getAreas() {
    let form = $('#__AjaxAntiForgeryForm');
    let token = $('input[name="__RequestVerificationToken"]', form).val();

    let id = document.getElementById("LayoutId").value;

    $.ajax({
        type: 'GET',
        url: '/Area/GetAllAreasForCreateEvent',
        data: {
            layoutId: id
        },
        success: function (response) {
            $('#areaList').html(response);
        }
    });
}

document.getElementById('VenueId').addEventListener('change', (e) => {
    document.getElementById('LayoutId').innerHTML = "<option value=''>Select Layout</option>";
    fetch(`?handler=Layouts&venueId=${e.target.value}`)
        .then((response) => {
            return response.json();
        })
        .then((data) => {
            Array.prototype.forEach.call(data, function (item, i) {
                document.getElementById('LayoutId').innerHTML += `<option value="${item.id}">${item.description}</option>`
            });
        });
});