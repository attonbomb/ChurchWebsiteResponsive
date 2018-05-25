// Contents of file "/Scripts/PromptForBookmark.js"
$(document).ready(function () 
{
  // this script should only be enabled if you are using the 
  // apple-mobile-web-app-capable=yes option
  var cookie_name = "PromptForBookmarkCookie";
  var cookie_exists = false;
  documentCookies = document.cookie;
  if (documentCookies.length > 0) 
  {
    cookie_exists = (documentCookies.indexOf(cookie_name + "=")
     != -1);
  }
  if (cookie_exists == false) 
  {
    // if it's an iOS device, then we check if we are in a 
    // full-screen mode, otherwise just move on
    if ((navigator.userAgent.indexOf("iPhone") > 0 ||
         navigator.userAgent.indexOf("iPad") > 0 ||
         navigator.userAgent.indexOf("iPod") > 0)) {
      if (!navigator.standalone) 
      {
        window.alert('This app is designed to be used in full screen mode. For best results, click on the Create Bookmark icon in your toolbar and select the Add to Home Screen option and start this app from the resulting icon.');
      }
    }
    //now that we've warned the user, set a cookie so that 
    //the user won't be asked again
    document.cookie = cookie_name +
       "=Told You So;expires=Monday, 31-Dec-2029 05:00:00 GMT";
  }
});
