// =============================
// F U N C T I O N S   S T A R T
// =============================

function closeTaskManager() {
    if (nav.hasClass('minimizedLeft')) {
        navigations.off('click', taskManager, flash);
    } else {
        if (!tm) {
            taskManagerItem.slideDown();
            taskManager.siblings().children('ul').slideUp();
            tm = true;
            um = false;
            st = false;
        } else {
            taskManagerItem.slideUp();
            tm = false;
        }
    }
}

function closeUserManager() {
    if (nav.hasClass('minimizedLeft')) {
        navigations.off('click', userManager, flash);
    } else {
        if (!um) {
            userManagerItem.slideDown();
            userManager.siblings().children('ul').slideUp();
            um = true;
            st = false;
            tm = false;
        } else {
            userManagerItem.slideUp();
            um = false;
        }
    }
}

function closeSettings() {
    if (nav.hasClass('minimizedLeft')) {
        navigations.off('click', settings, flash);
    } else {
        if (!st) {
            settings.siblings().children('ul').slideUp();
            settingsItem.slideDown();
            st = true;
            um = false;
            tm = false;
        } else {
            settingsItem.slideUp();
            st = false;
        }
    }
}

function minimizeNav() {
    if (!isMinimized) {
        nav.removeClass('coLeft').addClass('minimizedLeft');
        content.removeClass('coRight').addClass('minimizedRight');
        crud.css('display', 'none');
        mlx.css('font-size', '18px');
        logoName.css('line-height', '49px');
        navName.css('display', 'none');
        listName.css('display', 'none');
        angle.css('display', 'none');
        navigations.css('margin-top', '0px');
        navigations.children().children('ul').css('display', 'none');
        isMinimized = true;
    } else {
        nav.removeClass("minimizedLeft").addClass("coLeft");
        content.removeClass('minimizedRight').addClass('coRight');
        crud.css('display', 'inline-block');
        mlx.css('font-size', '20px');
        logoName.css('line-height', '50px');
        navName.css('display', 'block');
        listName.css('display', 'inline-block');
        angle.css('display', 'inline-block');
        navigations.css('margin-top', '-1px');
        isMinimized = false;
    }
}

function showMinimizedDashboard() {
    if (nav.hasClass('minimizedLeft')) {
        minimizedDashboard.css('display', 'block');
    }
}

function hideMinimizedDashboard() {
    minimizedDashboard.css('display', 'none');
}

function showMinimizedTaskManager() {
    if (nav.hasClass('minimizedLeft')) {
        minimizedTaskManager.css('display', 'block');
    }
}

function hideMinimizedTaskManager() {
    minimizedTaskManager.css('display', 'none');
}

function showMinimizedUserManager() {
    if (nav.hasClass('minimizedLeft')) {
        minimizedUserManager.css('display', 'block');
    }
}

function hideMinimizedUserManager() {
    minimizedUserManager.css('display', 'none');
}

function showMinimizedSettings() {
    if (nav.hasClass('minimizedLeft')) {
        minimizedSettings.css('display', 'block');
    }
}

function hideMinimizedSettings() {
    minimizedSettings.css('display', 'none');
}

// =========================
// F U N C T I O N S   E N D
// =========================

// =============================
// V A R I A B L E S   S T A R T
// =============================

var isOpen = false;
var tm = false;
var us = false;
var st = false;
var isMinimized = false;
var taskManager = $('#taskManager');
var taskManagerHead = $('#taskManager>a');
var userManagerHead = $('#userManager>a');
var settingsHead = $('#settings>a');
var taskManagerItem = $('#taskManager ul');
var userManager = $('#userManager');
var userManagerItem = $('#userManager ul');
var settings = $('#settings');
var settingsItem = $('#settings ul');
var bars = $("#bars a");
var nav = $("#nav");
var crud = $('#crud');
var content = $('#content');
var mlx = $('#mlx');
var navName = $('#navName');
var logoName = $('#logoName');
var listName = $('#navigations li>a span');
var angle = $('#navigations>li>a span').next();
var navigations = $('#navigations');
var dashboard = $('#dashboard');
var minimizedDashboard = $('#minimizedDashboard');
var minimizedTaskManager = $('#minimizedTaskManager');
var minimizedUserManager = $('#minimizedUserManager');
var minimizedSettings = $('#minimizedSettings');

// =========================
// V A R I A B L E S   E N D
// =========================

// =======================
// E V E N T S   S T A R T
// =======================

taskManagerHead.click(closeTaskManager);
userManagerHead.click(closeUserManager);
settingsHead.click(closeSettings);
bars.click(minimizeNav);
dashboard.mouseover(showMinimizedDashboard);
dashboard.mouseout(hideMinimizedDashboard);
taskManager.mouseover(showMinimizedTaskManager);
taskManager.mouseout(hideMinimizedTaskManager);
userManager.mouseover(showMinimizedUserManager);
userManager.mouseout(hideMinimizedUserManager);
settings.mouseover(showMinimizedSettings);
settings.mouseout(hideMinimizedSettings);
minimizedDashboard.mouseover(showMinimizedDashboard);
minimizedDashboard.mouseout(hideMinimizedDashboard);
minimizedTaskManager.mouseover(showMinimizedTaskManager);
minimizedTaskManager.mouseout(hideMinimizedTaskManager);
minimizedUserManager.mouseover(showMinimizedUserManager);
minimizedUserManager.mouseout(hideMinimizedUserManager);
minimizedSettings.mouseover(showMinimizedSettings);
minimizedSettings.mouseout(hideMinimizedSettings);

// ===================
// E V E N T S   E N D
// ===================
