///<reference path="module.js" />
/// <reference path="ItemService.js" />
'use strict';
angular.module('item').controller('AddItemController', ['$scope', '$window', 'itemService', function ($scope, $window, itemService) {

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

        var handleSaveResult = function (result) {
            $window.location.pathname = '/item/' + result.id + '/edit';
        };

        var handleError = function () {
            alert('An error occured whilst saving your item');
        };

        itemService.addItem($scope.item)
            .then(handleSaveResult, handleError)
            .finally(function () { $scope.working = false; });
    }
}]);