// This is the zero config. You can improve upon this.
// For example, it currently loads all the information at once. Meaning that if there is 1 million rows, 1 million rows will be loadead
// can you figure out a better way to do this? (Challenge)
$(document).ready(function () {
    $('#example').DataTable();
});

// Your challenge would be to figure out a Nuget package that is able to do date and time as well. (Challenge)
$('#date-input').datepicker({
    // Make sure your date format is consistent. This is very important when localisation comes into play.
    format: 'yyyy-mm-dd'


    
});


