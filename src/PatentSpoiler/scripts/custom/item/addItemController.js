///<reference path="module.js" />
'use strict';
angular.module('item').controller('AddItemController', ['$scope', '$window', 'addItemService', function ($scope, $window, addItemService) {

    $scope.working = false;
    $scope.category = $window.category;
    $scope.item = {};

    $scope.add = function() {
        alert('a');
    }

}]);