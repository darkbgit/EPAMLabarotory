const loc = document.getElementById("localeName").value;

jQuery.datetimepicker.setLocale(loc);

$(function () {
    $('#datepickerFrom').datetimepicker();
});

$(function () {
    $('#datepickerTo').datetimepicker();
});