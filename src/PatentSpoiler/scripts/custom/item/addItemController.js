///<reference path="module.js" />
'use strict';
angular.module('item').controller('AddItemController', ['$scope', '$window', 'addItemService', function ($scope, $window, addItemService) {

    $scope.working = false;
    $scope.category = $window.category;
    $scope.item = {};
    $scope.submitted = false;
    $scope.working = false;

    $scope.add = function() {
        $scope.submitted = true;
        
        if (!$scope.addForm.$valid) {
            $scope.addForm.$setPristine();
            return;
        }

        $scope.working = true;

        var handleSaveResult = function () {
            alert('Yeah!');
        };

        var handleError = function () {
            alert('An error occured whilst saving your item');
        };

        addItemService.addItem($scope.item, $scope.category)
            .then(handleSaveResult, handleError)
            .finally(function () { $scope.working = false; });
    }
}]);