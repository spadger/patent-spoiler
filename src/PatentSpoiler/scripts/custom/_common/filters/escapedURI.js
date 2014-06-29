///<reference path="../module.js"/>
'use strict';
angular.module('utils').filter('escapedURI', ['$window', function ($window) {
    return $window.encodeURIComponent;
}]);
 