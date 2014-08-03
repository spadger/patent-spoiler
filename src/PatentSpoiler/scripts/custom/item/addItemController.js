///<reference path="module.js" />
'use strict';
angular.module('item').controller('AddItemController', ['$scope', '$window', 'addItemService', function ($scope, $window, addItemService) {

    $scope.working = false;
    $scope.initialCategory = $window.category;
    $scope.item = {categories: {}, name:'', description:'' };
    $scope.submitted = false;
    $scope.working = false;

    if ($scope.initialCategory) {
        $scope.item.categories[$scope.initialCategory] = { Id: $scope.initialCategory };
    }

    $scope.removeCategory = function(id) {
        delete $scope.item.categories[id];
    }

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

        addItemService.addItem($scope.item)
            .then(handleSaveResult, handleError)
            .finally(function () { $scope.working = false; });
    }
}]);