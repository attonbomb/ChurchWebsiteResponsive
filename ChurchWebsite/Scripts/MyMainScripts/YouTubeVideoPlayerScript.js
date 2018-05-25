$(document).ready(function () {
    //intialise clicks on video list
    initialiseClickEvents()
    //first time the page loads get the top video
    changeVideoDisplay($(".sermonsVidList li:first").find('textarea').text());
    //will have to make the list of videos a partial updated by ajax

    $('.videosMoreButton a').click(function (event) {
        event.preventDefault();
        getMoreVideos();
    });
})

function initialiseClickEvents() {
    $(".sermonsVidListItem").click(function (event) {
        if (event.target != this) {
            changeVideoDisplay($(event.target).parent().find('textarea').text());
        } else {
            changeVideoDisplay($(event.target).find('textarea').text());
        }
    });
}

function getMoreVideos() {
    $.ajax({
        // edit to add steve's suggestion.
        url: '/Media/_VideosList?getMore=true',
        type: 'Get',
        success: function (data) {
            $("#sermonVidList").empty().append(data);
            initialiseClickEvents();
        },
        error: function () {
            alert("Error loading Video List!!!!");
        }
    });
}

function changeVideoDisplay(aVideoIdString) {
    $.ajax({
        // edit to add steve's suggestion.
        url: '/Media/VideoPlayer?vidId='+aVideoIdString,
        type: 'Get',
        success: function (data) {
            $("#sermonVidTarget").empty().append(data);
        },
        error: function () {
            alert("Error loading Video!!!!");
        }
    });
}