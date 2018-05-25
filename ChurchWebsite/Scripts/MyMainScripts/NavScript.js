var currentRootMenuSelection;

$(document).ready(function () {

    $('#menu').slicknav({
        'label': ''
    });

    resetRootMenuBackgrounds();
    showSubMenu();

    $("#subNavContainer a").click(function (e) {
        e.preventDefault();
        scrollToPageLocation(e.target.innerHTML.toString(), 'a');  
    });

    //TODO for mobile if we have a hash then we need to nudge the content 44 px
    if (window.location.href.indexOf("#") > -1) {
        var myHashLocation = window.location.href.substring(window.location.href.indexOf("#") + 1);
        /*alert("We have a hash ########" + myHashLocation);*/
        $('html, body').animate({
            scrollTop: $('a[name=' + myHashLocation + ']').offset().top - 44
        }, 100);
    }

    /*notes for parallax
    use the h2 tags to postion the images in a loop incrementing their y position relative to the parallax amount
    */

    $('a.read_more').click(function (event) { /* find all a.read_more elements and bind the following code to them */
        event.preventDefault(); /* prevent the a from changing the url */
        $(this).parent().parent().find('.more_text').css("display", "block"); /* show the .more_text span */
        var mySpanName = $(this).parent().parent().find('.more_text').attr("name");
        scrollToPageLocation(mySpanName, 'a');
    });
});


$("#menu li").hover(function () { // Mouse over
    $(this).css("background-color", "#CCCCCC");
}, function () { // Mouse out
    resetRootMenuBackgrounds();
});

function scrollToPageLocation(myTargetString, tag) {
    myTargetString = myTargetString.replace(/\s+/g, '');
    $('html, body').animate({
        scrollTop: $(tag + '[name=' + myTargetString + ']').offset().top - ($('.Header').height() - $('.slicknav_menu').height())
    }, 1000);
}

function resetRootMenuBackgrounds(){
    currentRootMenuSelection = $('#menu li').filter(function () {
        $(this).css("background-color", "#FFFFFF");
        var myRootHref = $(this).find('a').attr('href').toString();
        var myLocationRoot;
        if (location.pathname.toString().length > 1 && myRootHref.lastIndexOf("/") == 0) {
            myLocationRoot = location.pathname.toString();
            if (location.pathname.toString().lastIndexOf("/") > 0) {
                myLocationRoot = myLocationRoot.substring(0, location.pathname.toString().lastIndexOf("/"));
            }
        }
        else {
            myLocationRoot = "/";
        }
        return myRootHref === myLocationRoot;
    }).css("background-color", "#CCCCCC");
}

function showSubMenu() {
    if ($(currentRootMenuSelection).find('ul').html() != undefined) {
        $('#subNavContainer').html('<ul class="submenuShowing">' + $(currentRootMenuSelection).find('ul').html() + '</ul>');
        $(currentRootMenuSelection).find('ul').html('');
    }

    $('#subNavContainer li').filter(function () {
        $(this).css("background-color", "#FFFFFF");
        return $(this).find('a').attr('href') === location.pathname;
    }).css("background-color", "#CCCCCC");
}

function css(a) {
    var sheets = document.styleSheets, o = {};
    for (var i in sheets) {
        var rules = sheets[i].rules || sheets[i].cssRules;
        for (var r in rules) {
            if (a.is(rules[r].selectorText)) {
                o = $.extend(o, css2json(rules[r].style), css2json(a.attr('style')));
            }
        }
    }
    return o;
}

function css2json(css) {
    var s = {};
    if (!css) return s;
    if (css instanceof CSSStyleDeclaration) {
        for (var i in css) {
            if ((css[i]).toLowerCase) {
                s[(css[i]).toLowerCase()] = (css[css[i]]);
            }
        }
    } else if (typeof css == "string") {
        css = css.split("; ");
        for (var i in css) {
            var l = css[i].split(": ");
            s[l[0].toLowerCase()] = (l[1]);
        }
    }
    return s;
}
