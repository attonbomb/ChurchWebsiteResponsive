﻿var isFirstLoad = true;

function UploadImage() {
    $("#ImgForm").submit();
}

function UploadImage_Complete() {
    //Check to see if this is the first load of the iFrame
    if (isFirstLoad == true) {
        isFirstLoad = false;
        return;
    }

    //Reset the image form so the file won't get uploaded again
    document.getElementById("ImgForm").reset();

    //Grab the content of the textarea we named jsonResult .  This shold be loaded into 
    //the hidden iFrame.
    var newImg = $.parseJSON($("#UploadTarget").contents().find("#jsonResult")[0].innerHTML);

    //If there was an error, display it to the user
    if (newImg.IsValid == false) {
        //alert(newImg.Message);
        $("#FormContainer").html(newImg.Message);
        return;
    }

    //Create a new image and insert it into the Images div.  Just to be fancy, 
    //we're going to use a "FadeIn" effect from jQuery
    var imgDiv = document.getElementById("Images");
    var img = new Image();
    img.src = newImg.ImagePath;

    //Hide the image before adding to the DOM
    $(img).hide();
    imgDiv.appendChild(img);
    //Now fade the image in
    $(img).fadeIn(500, null);
}