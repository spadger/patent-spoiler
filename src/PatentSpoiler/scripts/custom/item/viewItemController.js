///<reference path="module.js" />
'use strict';
angular.module('item').controller('ViewItemController', ['$scope', '$window', 'itemService', function ($scope, $window, itemService) {

    $scope.initHistory = function () {
        $scope.$broadcast('OpenHistory');
    }
}]);