// Write your JavaScript code.
$.fn.datepicker.defaults.format = "mm/dd/yyyy";
$('.datepicker').datepicker({
    startDate: '-3d'
});

$('select').selectize();