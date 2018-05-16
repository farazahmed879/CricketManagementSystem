// Write your JavaScript code.
$.fn.datepicker.defaults.format = "mm/dd/yyyy";
$('.datepicker').datepicker({
    startDate: '-3d'
});

$('select').selectize();
//$('select').chosen({ disable_search_threshold: 10 });

$("#sidebarCollapse").on("click", () => {
    $("#sidebar").toggleClass("hidden-left-side-sideBar");
    $("#content").toggleClass("hidden-left-side-body");
    $("table").toggleClass("hidden-left-side-table");
    event.preventDefault();
})