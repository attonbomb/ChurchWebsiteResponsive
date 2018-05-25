var selectedImageId = -1;
var recurranceEvent;

/* This is for KnockoutJS dynamically 
creates client side model*/
var DynamicModelLoading = function (data) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);
    /*self.displayFullName = function (model) {
        var fullName = model.firstName() + " " + model.lastName();
        alert(fullName);
    };*/
};

/*************** FullCalendar.js implementation************************/
$('#calendar').fullCalendar({
    header: {
        left: 'prev,next today',
        center: 'title',
        right: ''
        /*right: 'month,agendaWeek,agendaDay'*/
    },

    dayClick: function (date, jsEvent, view) {
        /*alert('a day has been clicked!');*/
        //alert('Clicked on: ' + date.format());
        //alert('Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY);
        //alert('Current view: ' + view.name);
        $.ajax({
            // edit to add steve's suggestion.
            //url: "/ControllerName/ActionName",
            url: "/Admin/Calendar/_EventDialogue?evtDate=" + date.toISOString(),
            type: 'Get',
            dataType: 'html',
            success: function (data) {
                var eventDialogueButtons = new Object();
                eventDialogueButtons["Create Event"] = function () { $("form").submit(); };
                $("#createForm").dialog('option', 'buttons', eventDialogueButtons);
                $("#createForm").dialog("open");
                $("#createForm").empty().append(data);
                $("#createForm").dialog('option', 'title', 'Edit Event');
               
                selectedImageId = -1;
                hookUpEventFormDialogFunctions('create');
                //$("#editForm").hide();
            },
            error: function (errorData) {
                alert("something seems wrongwith the day click");
            }
        });
    },

    eventClick: function (calEvent, jsEvent, view) {
        //alert('Click Event: ' + calEvent.title);
        //alert('Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY);
        //alert('View: ' + view.name);
        onEditEvent(calEvent, false);
    },

    eventMouseover: function (event, jsEvent, view) {
        //alert('Mouse over Event: ' + event.title);
        //alert('Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY);
        //alert('View: ' + view.name);
    },

    eventMouseout: function (event, jsEvent, view) {

    },

    eventDragStart: function (event, jsEvent, ui, view) {
        alert("Started Dragging!!!");
    },

    eventDrop: function(event,dayDelta,minuteDelta,allDay,revertFunc) {

        alert(
            event.title + " was moved " +
            dayDelta + " days and " +
            minuteDelta + " minutes."
        );

        if (allDay) {
            alert("Event is now all-day");
        }else{
            alert("Event has a time-of-day");
        }

        if (!confirm("Are you sure about this change?")) {
            revertFunc();
        }

    },
    theme: false,
    editable: true,
    events: {
        url: '/Admin/Calendar/MonthEvents',
        dataType: "json",
        type: "GET"
    }
});

/***********************Event Dialog************************/
$("#createForm").dialog({
    autoOpen: false,
    modal: true,
    width: 550
});

$("#editRecEventDialog").dialog({
    autoOpen: false,
    modal: true,
    width: 550,
    buttons: {
        "Edit All Instances": function () {
            onEditEvent(recurranceEvent, true);
            $(this).dialog("close");
        },
        "Edit Just this Instance": function () {
            onEditIndividualInstance(recurranceEvent);
            $(this).dialog("close");
        },
        "Cancel": function () {
            $(this).dialog("close");
            viewModel = null;
        }
    }
});

/*******************Functions*************************/
function launchEditRecurringCheck(data) {
    $("#editRecEventDialog").dialog("open");
    $("#editRecEventDialog").empty().append(data);
    $("#editRecEventDialog").dialog('option', 'title', 'Edit Recurring Event');
    //hookUpEventFormDialogFunctions('edit');
}

function showEditEventDialog(data, calEvent) 
{
    var eventDialogueButtons = new Object();
    eventDialogueButtons["Update"] = function () { $("form").submit();};
    eventDialogueButtons["Delete"] = function () { onDeleteEvent();};
    $("#createForm").dialog('option', 'buttons', eventDialogueButtons);
    $("#createForm").dialog("open");
    $("#createForm").empty().append(data);
    $("#createForm").dialog('option', 'title', 'Edit Event');
    hookUpEventFormDialogFunctions('edit');
}

function onEventFormSubmissionSuccess(response) {
    window.location.href = response.Url;
}

function onDeleteEvent()
{
    $.ajax({
        url: "/Admin/Calendar/_DeleteEventRequested?evtId=" + viewModel.id._latestValue,
        type: 'Get',
        success: function (data) {
            showConfirmEventDeletePopup(data);
        }
    });
}

function onEditEvent(calendarEvent, ignoreRecurringCheck) {
    var checkAsInt = ignoreRecurringCheck ? 1 : 0;
    $.ajax({
        url: "/Admin/Calendar/_EventEdit?evtId=" + calendarEvent.id + "&ignoreRecChk=" + checkAsInt,
        type: 'Get',
        cache: false,
        success: function (data) {
            if (calendarEvent.recurranceType != 0 && !ignoreRecurringCheck) {
                recurranceEvent = calendarEvent;
                launchEditRecurringCheck(data);
            } else {
                showEditEventDialog(data, calendarEvent);
            }
        },
        error: function (data) {
            alert("something seems wrong");
        }
    });
}

function onEditIndividualInstance(event) {
    $.ajax({
        // edit to add steve's suggestion.
        //url: "/ControllerName/ActionName",
        url: "/Admin/Calendar/_EventCreateInstance?evtId=" + event.id + "&evtDate=" + event._start.toISOString(),
        type: 'Get',
        success: function (data) {
            showEditEventDialog(data);
        },
        error: function () {
            alert("something seems wrong when creating an individual instance");
        }
    });
}

function hookUpEventFormDialogFunctions(mode)
{
    if (mode == 'edit' && viewModel.imgId._latestValue != null) {
        //adding the image to the form
        addImageRefToForm(viewModel.imgId._latestValue);
    }

    var myButton = $('#imgLib_button');
    $('#imgLib_button').click(function (e) {
        $.ajax({
            url: '/Admin/Image/_ImageLib?imgTyp=1',
            type: 'Get',
            success: function (data) {
                $("#imageLib").dialog("open");
                $("#imageLib").empty().append(data);
                $("#imageLib").dialog('option', 'title', 'Event Images');
                hookUpImageLibraryFunctions();
            },
            error: function (data) {
                alert("something seems wrong");
            }
        });
    });

    $("#imageLib").dialog({
        autoOpen: false,
        modal: true,
        width: 550,
        buttons: {
            "Add New Image": function () {
                launchImageUploader();
            },
            "Confirm": function () {
                //$(this).dialog("close");
                addImageRefToForm(selectedImageId);
                $(this).dialog("close");
            },
            "Delete Image": function () {
                if (selectedImageId > -1) {
                    launchAreYouSureForImgDel(selectedImageId);
                }
            }
        }
    });

    $("#eventDeleteDialog").dialog({
        autoOpen: false,
        modal: true,
        width: 550,
        buttons: {
            "Yes": function () {
                deleteEvent(viewModel.id._latestValue);
            },
            "No": function () {
                $(this).dialog("close");
            }
        }
    });
}

function addImageRefToForm(imgRef) {
    $('form input#imgId').val(imgRef);
    $.ajax({
        // edit to add steve's suggestion.
        cache: false,
        type: "GET",
        url: '/Admin/Image/GetBase64Image/' + imgRef + '?width=10&height=10',
        contentType: 'application/json',
        dataType: "json",
        success: function (data) {
            imgs = data;
            displayImage(imgs.base64imgage);
        },
        error: function (xhr) {
            alert("Error occurred while loading the image. "
                + xhr.responseText);
        }
    });
}

var displayImage = function (base64Data) {
    var imag = "<img "
             + "src='" + "data:image/jpg;base64,"
             + base64Data + "'/>";

    $("#imagePlaceholder").html(imag);
};

function launchImageUploader()
{
    $.ajax({
        // edit to add steve's suggestion.
        url: '/Admin/Image/_ImageUploadForm?imgTyp=1',
        type: 'Get',
        success: function (data) {
            $("#imageUploadDialog").dialog("open");
            $("#imageUploadDialog").empty().append(data);
            $("#imageUploadDialog").dialog('option', 'title', 'Images Upload');
        },
        error: function () {
            alert("Error loading Image Uploader");
        }
    });
}

function launchAreYouSureForImgDel(imageId) {
    $.ajax({
        url: '/Admin/Image/_DeleteImageRequested?imgId=' + imageId,
        type: 'Get',
        success: function (data){
            $("#deleteAreYouSureDiog").dialog("open");
            $("#deleteAreYouSureDiog").empty().append(data);
            $("#deleteAreYouSureDiog").dialog('option', 'title', 'Delete Image');
        },
        error: function () {
            alert("Error loading Image Delete Confirm");
        }
    });
}

function hookUpImageLibraryFunctions()
{
    $('.boxInner').click(function (e) {
        selectedImageId = $(this).find('img').attr("id");
        removeSelecteClassFromAll();
        $(this).addClass('boxInnerSelected');
    });

    $("#imageUploadDialog").dialog({
        autoOpen: false,
        modal: true,
        width: 550
    });

    $("#deleteAreYouSureDiog").dialog({
        autoOpen: false,
        modal: true,
        width: 550,
        buttons: {
            "Yes": function () {
                $(this).dialog("close");
                deleteSelectedImage(selectedImageId);
            },
            "No": function () {
                $(this).dialog("close");
            }
        }
    });
}

function removeSelecteClassFromAll() {
    $("div.wrap").children().children(".boxInner").removeClass('boxInnerSelected');
}

function deleteSelectedImage(imageId) {
    $.ajax({
        url: '/Admin/Image/Delete?id=' + imageId,
        type: 'POST',
        success: function (data) {
            $("#imageLib").empty().append(data);
            $("#imageLib").dialog('option', 'title', 'Event Images');
            hookUpImageLibraryFunctions();
        },
        error: function (data) {
            alert("something seems wrong");
        }
    });
}

function deleteEvent(evtId) {
    $.ajax({
        url: "/Admin/Calendar/_DeleteEvent?id=" + evtId,
        type: 'POST',
        cache: false,
        success: function (response) {
            currentEventId = -1;
            window.location.href = response.Url;
        },
        error: function (response) {
            alert("something seems wrong when deleting an event");
        }
    });
}

function showConfirmEventDeletePopup(htmlData)
{
    $("#eventDeleteDialog").dialog("open");
    $("#eventDeleteDialog").empty().append(htmlData);
    $("#eventDeleteDialog").dialog('option', 'title', 'Delete Event');
}