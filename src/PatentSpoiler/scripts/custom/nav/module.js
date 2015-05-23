///<reference path="~/scripts/angular.js" />
/// <reference path="~/scripts/ui-bootstrap.js" />
'use strict';
angular.module('nav', ['ui.bootstrap']);

angular.module('nav').controller('nav', ['$scope', function($scope) {

    $scope.abc = 'def';

}]);