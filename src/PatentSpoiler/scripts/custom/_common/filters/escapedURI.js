///<reference path="~/scripts/custom/_common/UtilsModule.js"/>
'use strict';
window.utilsModule.filter('escapedURI', ['$window', function($window) {
    return window.encodeURIComponent;
}]);
 